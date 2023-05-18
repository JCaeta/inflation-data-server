namespace InflationDataServer.Persistence
{
    static public class DatabaseInformation
    {
        static public Dictionary<string, string> Information = new Dictionary<string, string>
        {
            { "SERVER", "localhost" },
            { "DATABASE_NAME", "Inflation" },
            { "USER_ID", "postgres" },
            { "PASSWORD", "mypassword" }
        };
    }
}
