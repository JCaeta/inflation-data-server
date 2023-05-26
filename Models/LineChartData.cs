namespace InflationDataServer.Models
{
    public class LineChartData
    {
        public List<LineChartPoint> data { get; set; } = new List<LineChartPoint>();
    }

    public class LineChartPoint
    {
        public string time { get; set; }
        public float value { get; set; }
    }
}
