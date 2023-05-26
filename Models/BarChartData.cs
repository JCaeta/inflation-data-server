namespace InflationDataServer.Models
{
    public class BarChartData
    {
        public List<string> labels { get; set; } = new List<string>();
        public List<float> data { get; set; } = new List<float>();
    }
}

