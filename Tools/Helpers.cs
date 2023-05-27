namespace InflationDataServer.Tools
{
    static public class Helpers
    {
        static public Dictionary<int, string> StandardMessages = new Dictionary<int, string>()
        {
            {1,  "Succeeded"},
            {-1,  "Request failed to complete"},
            {-2,  "Communication with server failed"},
            {-3,  "Inflation is null"},
            {-4,  "Inflation updating failed"},
            {-5, "Inflation deletion failed" },
            {-6, "Access Denied: Invalid or expired token" },
            {-7, "Invalid password" }
        };
    }
}

/**
    Standard messages
    id: message
    1: Succeeded
    -1: Request failed to complete
    -2: Communication with server failed
    -3: Inflation is null
    -4: Inflation updating failed
    -5: Inflation deletion failed
    -6: Access Denied: Invalid or expired token
    -7: Invalid password
 */