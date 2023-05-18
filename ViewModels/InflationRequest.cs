using InflationDataServer.Models;
namespace InflationDataServer.ViewModels
{
    public class InflationRequest
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public Inflation? inflation { get; set; } = new Inflation();
    }
}
