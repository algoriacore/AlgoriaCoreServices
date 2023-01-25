namespace AlgoriaCore.WebUI.Helpers
{
    public class AppSettings
    {
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string IssuerSigningKey { get; set; }
        public string BaseUrl { get; set; }
        public DatabaseType DatabaseType { get; set; }
        public string CORSOrigins { get; set; }

        public double TokenExpires { get; set; }
        public int DatabaseCommandTimeout { get; set; }
    }

    public enum DatabaseType : byte
    {
        Sql = 0,
        MySql = 1,
        Postgres = 2
    }
}
