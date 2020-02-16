using System.Collections.Generic;
using Newtonsoft.Json;

namespace DayDash.Models
{
    public class BusSchedule
    { 
        [JsonProperty("stop_name")]
        public string StopName { get; set; }
        
        [JsonProperty("departures")]
        public BusLines BusLines { get; set; }
    }

    public class BusLines
    {
        [JsonProperty("m1")]
        public List<Bus> m1 { get; set; }

        [JsonProperty("m3")]
        public List<Bus> m3 { get; set; }
    }
    public class Bus
    {
        [JsonProperty("line_name")]
        public string BusName { get; set; }

        [JsonProperty("aimed_departure_time")]
        public string Time { get; set; }  
 
    }
}