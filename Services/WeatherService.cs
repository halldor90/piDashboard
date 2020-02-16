using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using DayDash.Models;
using DarkSky.Models;
using Microsoft.Extensions.Options;

namespace DayDash.Services
{
    public class WeatherService : IWeatherService
    { 
        public async Task<WeatherItem> GetForecastAsync(IOptions<AppSettings> appSettings)
        {
            WeatherItem weatherForecast = new WeatherItem();

            string apiKey = appSettings.Value.DarkSkyAPIKey;

            var darkSky = new DarkSky.Services.DarkSkyService(apiKey);

            var forecast = await darkSky.GetForecast(51.4557, -2.583,
                new OptionalParameters
                {
                    MeasurementUnits = "si"
                });

            if (forecast?.IsSuccessStatus == true)
            {
                weatherForecast.CurrentFeelsLikeTemperature = Convert.ToInt32(forecast.Response.Currently.ApparentTemperature);
                weatherForecast.CurrentSummary = forecast.Response.Currently.Summary;
                weatherForecast.CurrentTemperature = Convert.ToInt32(forecast.Response.Currently.Temperature);
                weatherForecast.CurrentIconPath = "/images/" + forecast.Response.Currently.Icon.ToString() + ".svg";

                weatherForecast.MinutelySummary = forecast.Response.Minutely.Summary;

                List<HourData> hourData = new List<HourData>();

                foreach (var hour in forecast.Response.Hourly.Data.GetRange(1, 3))
                {
                    HourData hdata = new HourData();
                    hdata.Time = DateTime.ParseExact(hour.DateTime.TimeOfDay.ToString(), "HH:mm:ss", new CultureInfo("en-GB")).ToString("HH:mm");
                    hdata.Summary = hour.Summary;
                    hdata.Temp = Convert.ToInt32(hour.Temperature);
                    hdata.Icon = "/images/" + hour.Icon.ToString() + ".svg";
                    hourData.Add(hdata);
                }

                weatherForecast.Hour = hourData;

                List<DayData> dayData = new List<DayData>();

                foreach (var day in forecast.Response.Daily.Data.GetRange(0, 4))
                {
                    DayData ddata = new DayData();
                    ddata.Day = DateTime.ParseExact(day.DateTime.Date.ToString(), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("dd.MM yyyy");
                    ddata.Summary = day.Summary;
                    ddata.Temp = Convert.ToInt32(day.TemperatureLow);
                    ddata.TempLow = Convert.ToInt32(day.TemperatureLow);
                    ddata.TempHigh = Convert.ToInt32(day.TemperatureHigh);
                    ddata.Icon = "images/" + day.Icon.ToString() + ".svg";
                    dayData.Add(ddata);
                }

                weatherForecast.Day = dayData;

                weatherForecast.Alerts = forecast.Response.Alerts;

            }
            else
            {
                weatherForecast.CurrentSummary = "No current weather data";
            }

            return weatherForecast;
        }
    }
}