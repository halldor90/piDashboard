using System.Collections.Generic;

namespace DayDash.Models
{
    public class DisplayViewModel
    {
        public List<CalendarItem> Items { get; set; }

        public WeatherForecast Forecast { get; set; }

        public BusSchedule BusSchedule { get; set; }

    }
}