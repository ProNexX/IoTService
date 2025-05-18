namespace IoT.DataAccess.Models
{
    public class TelemetryData
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
