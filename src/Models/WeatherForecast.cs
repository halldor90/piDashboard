using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DayDash.Models
{
    public class WeatherForecast
    {
        [JsonPropertyName("current")]
        public CurrentData Current { get; set; }

        [JsonPropertyName("hourly")]
        public List<HourData> Hour { get; set; }

        [JsonPropertyName("daily")]
        public List<DayData> Day { get; set; }
    }

    public class CurrentData
    {    
        [JsonPropertyName("temp")]
        public double CurrentTemperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double CurrentFeelsLikeTemperature { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherData> CurrentWeather { get; set; }
    }

    public class WeatherData
    {
        [JsonPropertyName("description")]
        public string Summary { get; set; }

        [JsonPropertyName("main")]
        public string Icon { get; set; }
    }

    public class HourData
    {
        [JsonPropertyName("dt")]
        public int Time { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherData> HourWeather { get; set; }
    }

    public class DayData
    {
        [JsonPropertyName("dt")]
        public int Day { get; set; }

        [JsonPropertyName("temp")]
        public DayTempData DayTemp { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherData> DayWeather { get; set; }
    }

    public class DayTempData
    {
        [JsonPropertyName("day")]
        public double Temp { get; set; }

        [JsonPropertyName("min")]
        public double TempLow { get; set; }

        [JsonPropertyName("max")]
        public double TempHigh { get; set; }
    }
}
