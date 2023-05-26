namespace InflationDataServer.Models
{
    public class MessageResponse
    {
        public int id { get; set; }
        public string message { get; set; } = String.Empty;
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
 */