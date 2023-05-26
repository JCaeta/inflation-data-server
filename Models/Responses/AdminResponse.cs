namespace InflationDataServer.Models.Responses
{
    public class AdminResponse
    {
        public MessageResponse message { get; set; } = new MessageResponse();
        public string? token { get; set; }
    }
}
