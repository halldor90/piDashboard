using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DayDash.Models
{
    public class WeatherForecast
    {
        [JsonProperty("current")]
        public CurrentData Current { get; set; }

        [JsonProperty("hourly")]
        public List<HourData> Hour { get; set; }

        [JsonProperty("daily")]
        public List<DayData> Day { get; set; }
    }

    public class CurrentData
    {    
        [JsonProperty("temp")]
        public double CurrentTemperature { get; set; }

        [JsonProperty("feels_like")]
        public double CurrentFeelsLikeTemperature { get; set; }

        [JsonProperty("weather")]
        public List<WeatherData> CurrentWeather { get; set; }

    }

    public class WeatherData
    {
        [JsonProperty("description")]
        public string Summary { get; set; }

        [JsonProperty("main")]
        public string Icon { get; set; }
    }

    public class HourData
    {
        [JsonProperty("dt")]
        public string Time;

        [JsonProperty("temp")]
        public double Temp;

        [JsonProperty("weather")]
        public List<WeatherData> HourWeather { get; set; }
    }

    public class DayData
    {
        [JsonProperty("dt")]
        public string Day;

        [JsonProperty("temp")]
        public DayTempData DayTemp;

        [JsonProperty("weather")]
        public List<WeatherData> DayWeather { get; set; }
    }

    public class DayTempData
    {
        [JsonProperty("day")]
        public double Temp;

        [JsonProperty("min")]
        public double TempLow { get; set; }

        [JsonProperty("max")]
        public double TempHigh { get; set; }
    }
}
