namespace UserAuthenticationAPI.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<User>>> GetAllUsers();
        Task<ServiceResponse<User>> GetUserByToken(string token);
        Task<ServiceResponse<string>> GetUserRole(string token);
    }
}
