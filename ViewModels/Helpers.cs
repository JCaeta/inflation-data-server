using InflationDataServer.Models;

namespace InflationDataServer.ViewModels
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
            {-5, "Inflation deletion failed" }
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
 */