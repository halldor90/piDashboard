using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayDash.Models
{
    public class BusSchedule
    { 
        [JsonPropertyName("stop_name")]
        public string StopName { get; set; }
        
        [JsonPropertyName("departures")]
        public BusLines BusLines { get; set; }
    }

    public class BusLines
    {
        [JsonPropertyName("m1")]
        public List<Bus> m1 { get; set; }

        [JsonPropertyName("m3")]
        public List<Bus> m3 { get; set; }
    }
    public class Bus
    {
        [JsonPropertyName("line_name")]
        public string BusName { get; set; }

        [JsonPropertyName("best_departure_estimate")]
        public string EstimatedDepartureTime { get; set; }  
 
    }
}