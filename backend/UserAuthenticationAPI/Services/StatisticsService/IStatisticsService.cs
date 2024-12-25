namespace UserAuthenticationAPI.Services.StatisticsService
{
    public interface IStatisticsService
    {
        Task<ServiceResponse<int>> GetRegistrations(DateTime startDate, DateTime endDate);
        Task<ServiceResponse<int>> NotRegisteredWithinOneDay();
        Task<ServiceResponse<int>> GetOnlineUsers();
        Task<ServiceResponse<double>> GetAverageLoginTime(DateTime day);
    }
}
