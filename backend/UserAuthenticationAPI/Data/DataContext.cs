namespace UserAuthenticationAPI.Data
{
    public class DataContext
    {
        private readonly IMongoDatabase _database;

        public DataContext(IConfiguration configuration)
        {
            string? connectionString = configuration.GetSection("MongoDb:ConnectionString")?.Value;
            string? databaseName = configuration.GetSection("MongoDb:DatabaseName")?.Value;

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string is not provided.");

            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentNullException(nameof(databaseName), "MongoDB database name is not provided.");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
