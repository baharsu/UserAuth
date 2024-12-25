namespace UserAuthenticationAPI.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<User>> RegisterUser(RegisterDto registerDto);
        Task<ServiceResponse<string>> LoginUser(LoginDto loginDto);
        Task<ServiceResponse<string>> LogOutUser(string username);
        Task<ServiceResponse<string>> VerifyUser(string email, string verificationCode, string loginStartTime);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<string>> ResetPassword(string code, string newPassword);


    }
}
