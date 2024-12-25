using MongoDB.Bson;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserAuthenticationAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<ServiceResponse<List<User>>> GetAllUsers()
        {
            try
            {
                var serviceResponse = new ServiceResponse<List<User>>();
                var userList = await _context.Users.Find(u => true).ToListAsync();
                serviceResponse.Data = userList;
                serviceResponse.Success = true;
                return serviceResponse;
            }
            catch (Exception e)
            {
                return new ServiceResponse<List<User>> { Data = null, Message = e.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<User>> GetUserByToken(string token)
        {
            var serviceResponse = new ServiceResponse<User>();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid token.";
                    return serviceResponse;
                }

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User ID not found in token.";
                    return serviceResponse;
                }

                var userIdValue = userIdClaim.Value;
                if (!ObjectId.TryParse(userIdValue, out var userId))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid User ID format.";
                    return serviceResponse;
                }

                var user = await _context.Users.Find(u => u.Id == userId.ToString()).FirstOrDefaultAsync();
                if (user == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User not found.";
                    return serviceResponse;
                }
                serviceResponse.Success = true;
                serviceResponse.Data = (User?)user;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to retrieve user: {ex.Message}";
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> GetUserRole(string token)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Invalid token.";
                    return serviceResponse;
                }
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleClaim == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Role not found in token.";
                    return serviceResponse;
                }
                var roleValue = roleClaim.Value;
                serviceResponse.Success = true;
                serviceResponse.Data = roleValue;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Failed to retrieve user role: {ex.Message}";
            }
            return serviceResponse;

        }
    }
}
