namespace BLL.Models.History
{
    public class TempHumiData
    {
        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string Topic { get; set; }
    }
}