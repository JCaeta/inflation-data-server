namespace InflationDataServer.Models.Requests
{
    public class InflationRequest
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public Inflation? inflation { get; set; } = new Inflation();
        public string? token { get; set; }
    }
}
