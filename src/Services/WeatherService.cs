using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using DayDash.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;

namespace DayDash.Services
{
    public class WeatherService : IWeatherService
    { 
        public async Task<WeatherForecast> GetForecastAsync(IOptions<AppSettings> appSettings)
        {
            var weatherUrl = CreateWeatherUrl(appSettings);

            var weatherForecast = await GetWeatherForecastAsync(weatherUrl);

            return weatherForecast;
        }

        private string CreateWeatherUrl(IOptions<AppSettings> appSettings)
        {
            var weatherValues = appSettings.Value;

            var weatherUrl = $"https://api.openweathermap.org/data/2.5/onecall?lat={ weatherValues.Latitude }&lon={ weatherValues.Longitude }&units={ weatherValues.Unit }&appid={ weatherValues.OpenWeatherMapAPIKey }";

            return weatherUrl;
        }

        private async Task<WeatherForecast> GetWeatherForecastAsync(string weatherUrl)
        {
            HttpClient client = new HttpClient();

            WeatherForecast weatherForecast = new WeatherForecast();            

            var response = await client.GetAsync(weatherUrl);

            var responseBody = response.Content.ReadAsStringAsync().Result;
            weatherForecast = JsonConvert.DeserializeObject<WeatherForecast>(responseBody);

            return weatherForecast;
        }
    }
}