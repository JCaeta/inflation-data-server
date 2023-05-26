namespace InflationDataServer.Models.Requests
{
    public class AdminRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string oldPassword { get; set; }
        public string token { get; set; }
    }
}
