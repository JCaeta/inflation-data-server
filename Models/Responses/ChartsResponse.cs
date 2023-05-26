using InflationDataServer.Models;

namespace InflationDataServer.Models.Responses
{
    public class ChartsResponse
    {
        public MessageResponse message { get; set; } = new MessageResponse();
        public ChartsData data { get; set; }
    }
}
