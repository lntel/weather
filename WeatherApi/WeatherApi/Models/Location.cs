namespace WeatherApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class LocalNames
    {
        public string Sr { get; set; }
        public string Uk { get; set; }
        public string En { get; set; }
        public string Ur { get; set; }
        public string Lt { get; set; }
        public string Ru { get; set; }
        public string Ko { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public LocalNames LocalNames { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }


}
