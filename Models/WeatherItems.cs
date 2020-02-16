using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DarkSky.Models;

namespace DayDash.Models
{
    public class WeatherItem
    {
        [Required]
        public string CurrentSummary { get; set; }

        public string CurrentIconPath { get; set; }

        public int CurrentTemperature { get; set; }

        public int CurrentFeelsLikeTemperature { get; set; }

        public string MinutelySummary { get; set; }

        public List<HourData> Hour { get; set; }

        public List<DayData> Day { get; set; }

        public List<Alert> Alerts { get; set; }
    }

    public class HourData
    {
        public string Time;

        public int Temp;

        public string Summary;

        public string Icon;
    }

    public class DayData
    {
        public string Day;

        public int Temp;

        public int TempLow { get; set; }

        public int TempHigh { get; set; }

        public string Summary;

        public string Icon;
    }
}
