namespace AlgoriaCore.Application.Configuration
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsTransactional { get; set; }
    }
}
