using InflationDataServer.Models;

namespace InflationDataServer.ViewModels
{
    public class InflationResponse
    {
        public MessageResponse message { get; set; } = new MessageResponse();
        public List<Inflation> inflationList { get; set; } = new List<Inflation>();
    }
}
