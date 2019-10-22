using System.Collections.Generic;

namespace piDash.Models
{
    public class DisplayViewModel
    {
        public List<CalenderItem> Items { get; set; }

        public WeatherItem Forecast { get; set; }

    }
}