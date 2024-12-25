namespace UserAuthenticationAPI.Services.StatisticsService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DataContext _context;

        public StatisticsService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<ServiceResponse<int>> GetRegistrations(DateTime startDate, DateTime endDate)
        {
            var serviceResponse = new ServiceResponse<int>();
            endDate = endDate.AddDays(1).AddTicks(-1);
            var registrations = await _context.Users.CountDocumentsAsync(u => u.RegisteredAt >= startDate && u.RegisteredAt <= endDate);
            serviceResponse.Data = (int)registrations;
            serviceResponse.Success = true;
            serviceResponse.Message = "Registrations within given day";
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> NotRegisteredWithinOneDay()
        {
            var serviceResponse = new ServiceResponse<int>();
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var notRegisteredUsersCount = await _context.Users.CountDocumentsAsync(u => u.VerificationCodeSentAt < yesterday && !u.IsRegisterVerified);
            serviceResponse.Data = (int)notRegisteredUsersCount;
            serviceResponse.Success = true;
            serviceResponse.Message = "Users not registered within one day";
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> GetOnlineUsers()
        {
            var serviceResponse = new ServiceResponse<int>();
            var onlineUsers = await _context.Users.Find(u => u.IsOnline).ToListAsync();
            serviceResponse.Data = onlineUsers.Count;
            serviceResponse.Success = true;
            serviceResponse.Message = "Online users";
            return serviceResponse;
        }

        public async Task<ServiceResponse<double>> GetAverageLoginTime(DateTime day)
        {
            var serviceResponse = new ServiceResponse<double>();
            var startOfDay = day.Date; 
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1); 

            var users = await _context.Users
                .Find(u => u.LoginStartTime.HasValue && u.LoginEndTime.HasValue && u.LoginStartTime.Value >= startOfDay && u.LoginStartTime.Value <= endOfDay)
                .ToListAsync();

            if (users.Count == 0)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = "No users found with login start time on the specified day";
                return serviceResponse;
            }

            

            int min = 0;
            int sec = 0;
            foreach (var user in users)
            {
                min += (user.LoginEndTime.Value.Minute - user.LoginStartTime.Value.Minute);
                sec += (user.LoginEndTime.Value.Second - user.LoginStartTime.Value.Second);
            }

            int totalLoginTimes = min * 60 + sec;

            if (totalLoginTimes <= 0)
            {
                serviceResponse.Success = true;
                serviceResponse.Message = "No users have a valid login end time for the specified day";
                return serviceResponse;
            }

            var averageLoginTime = totalLoginTimes / users.Count;
            serviceResponse.Success = true;
            serviceResponse.Data = averageLoginTime;
            return serviceResponse;
        }
    }
}
