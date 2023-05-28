namespace InflationDataServer.Persistence
{
    static public class DatabaseInformation
    {
        static public Dictionary<string, string> GetDbInfo()
        {
            var server = Environment.GetEnvironmentVariable("PGHOST") ?? "localhost";
            var databaseName = Environment.GetEnvironmentVariable("PGDATABASE") ?? "Inflation";
            var userId = Environment.GetEnvironmentVariable("PGUSER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("PGPASSWORD") ?? "mypassword";
            var port = Environment.GetEnvironmentVariable("PGPORT") ?? "5432";

            return new Dictionary<string, string>
            {
                { "SERVER", server },
                { "DATABASE_NAME", databaseName },
                { "USER_ID", userId },
                { "PASSWORD", password },
                { "PORT", port },
            };
        }
    }
}
