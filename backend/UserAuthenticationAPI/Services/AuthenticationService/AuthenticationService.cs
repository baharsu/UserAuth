
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserAuthenticationAPI.Dtos.User;

namespace UserAuthenticationAPI.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private static readonly ConcurrentDictionary<string, DateTime> LoginStartTimes = new();

        public AuthenticationService(DataContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<User>> RegisterUser(RegisterDto registerDto)
        {
            var serviceResponse = new ServiceResponse<User>();
            if (await _context.Users.Find(u => u.Email == registerDto.Email).AnyAsync())
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This email is already registered";
                return serviceResponse;
            }
            else if (await _context.Users.Find(u => u.Username == registerDto.Username).AnyAsync())
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This username is already taken";
                return serviceResponse;
            }
            else
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                var verificationCode = GenerateVerificationCode();

                var user = new User
                {
                    Name = registerDto.Name,
                    Surname = registerDto.Surname,
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    VerificationCode = verificationCode,
                    RegisteredAt = DateTime.UtcNow,
                    VerificationCodeSentAt = DateTime.UtcNow,
                    IsRegisterVerified = false,
                    VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(5),
                    Role = registerDto.Role
                };
                await _context.Users.InsertOneAsync(user);

                await SendVerificationEmail(user.Email, verificationCode);
      
                serviceResponse.Data = user;
                serviceResponse.Success = true;
                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<string>> LoginUser(LoginDto loginDto)
        {
            var serviceResponse = new ServiceResponse<string>();
            
            var user = await _context.Users.Find(u => u.Username == loginDto.Username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid username or password";
            }
            else
            {
                var verificationCode = GenerateVerificationCode();
                user.VerificationCode = verificationCode;
                await SendVerificationEmail(user.Email, verificationCode);
                user.VerificationCodeSentAt = DateTime.UtcNow;
                user.VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(5);
                await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                serviceResponse.Success = true;
                serviceResponse.Message = "Verification code sent to email";
            }
            
            return serviceResponse;

        }


        private string GenerateToken(User user)
        {
            try
            {
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return jwtToken;
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }

        private string GenerateVerificationCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        private async Task SendVerificationEmail(string email, string verificationCode)
        {
            var user = await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
            if(!user.IsRegisterVerified)
            {
                var subject = "Email Verification";
                var message = $"Email verification code: {verificationCode}. This code expires after 5 minutes.";
                await _emailService.SendEmailAsync(email, subject, message);
            }
            else
            {
                var subject = "Login Verification";
                var message = $"Login verification code: {verificationCode}. This code expires after 5 minutes.";
                await _emailService.SendEmailAsync(email, subject, message);
            }
        }

        private async Task SendPasswordResetEmail(string email, string verificationCode)
        {
            var subject = "Password Reset";
            var message = $"Password reset code: {verificationCode}. This code expires after 5 minutes.";
            await _emailService.SendEmailAsync(email, subject, message);
        }

        public async Task<ServiceResponse<string>> VerifyUser(string username, string verificationCode, string loginStartTime)
        {
            try
            {
                var serviceResponse = new ServiceResponse<string>();
                var user = _context.Users.Find(u => u.Username == username).FirstOrDefault();
                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found";
                }
                else if (user.VerificationCodeExpiresAt < DateTime.UtcNow)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Verification code expired. New code sent to email";
                    var newVerificationCode = GenerateVerificationCode();
                    user.VerificationCode = newVerificationCode;
                    await SendVerificationEmail(user.Email, newVerificationCode);
                    user.VerificationCodeSentAt = DateTime.UtcNow;
                    user.VerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(5);
                    await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                }
                else if (user.VerificationCode == verificationCode)
                {
                    if (!user.IsRegisterVerified)
                    {
                        user.IsRegisterVerified = true;
                    }
                    
                    user.VerificationCodeUsedAt = DateTime.UtcNow;
                    var token = GenerateToken(user);
                    user.IsOnline = true;
                    serviceResponse.Data = token;
                    user.LastLoginTime = DateTime.UtcNow;
                    user.LoginEndTime = DateTime.UtcNow;
                    user.LoginStartTime = DateTime.Parse(loginStartTime);
                    serviceResponse.Success = true;
                    await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                    serviceResponse.Success = true;
                    serviceResponse.Message = "Verified";
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid verification code";
                }
                
                return serviceResponse;
            }
            catch (Exception e)
            {
                return new ServiceResponse<string> { Data = e.Message, Message = e.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                var code = GenerateVerificationCode();
                user.VerificationCode = code;
                await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                await SendPasswordResetEmail(email, code);
                serviceResponse.Success = true;
                serviceResponse.Message = "Verification code sent to email";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<string>> ResetPassword(string code, string newPassword)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = _context.Users.Find(u => u.VerificationCode == code).FirstOrDefault();
            if (user != null)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
                serviceResponse.Success = true;
                serviceResponse.Message = "Password reset successful";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Invalid verification code";
            }
            return serviceResponse;
        }

 
        public async Task<ServiceResponse<string>> LogOutUser(string username)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = await _context.Users.Find(u => u.Username == username).FirstOrDefaultAsync();
            serviceResponse.Success = true;
            serviceResponse.Message = "User logged out";
            user.IsOnline = false;
            user.LastLogoutTime = DateTime.UtcNow;
            user.LoginEndTime = DateTime.UtcNow;
            await _context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return serviceResponse;
        }
        
    }
}
