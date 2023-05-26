using InflationDataServer.Models;

namespace InflationDataServer.Models.Responses
{
    public class InflationResponse
    {
        public MessageResponse message { get; set; } = new MessageResponse();
        public List<Inflation> inflationList { get; set; } = new List<Inflation>();
        public string? token { get; set; }
    }
}
