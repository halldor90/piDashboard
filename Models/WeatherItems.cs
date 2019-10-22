using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DarkSky.Models;

namespace piDash.Models
{
    public class WeatherItem
    {
        [Required]
        public string CurrentSummary { get; set; }

        public string CurrentIconPath { get; set; }

        public int CurrentTemperature { get; set; }

        public int CurrentFeelsLikeTemperature { get; set; }

        public int CurrentTempLow { get; set; }

        public int CurrentTempHigh { get; set; }

        public string MinutelySummary { get; set; }

        public List<HourData> Hour { get; set; }

        public List<DayData> Day { get; set; }

        public List<Alert> Alerts { get; set; }
    }

    public class HourData
    {
        public string time;

        public int temp;

        public string summary;

        public string icon;
    }

    public class DayData
    {
        public string day;

        public int temp;

        public string summary;

        public string icon;
    }
}
