using System.Collections.Generic;

namespace DayDash.Models
{
    public class DisplayViewModel
    {
        public List<CalenderItem> Items { get; set; }

        public WeatherItem Forecast { get; set; }

        public BusSchedule BusSchedule { get; set; }

    }
}