using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using piDash.Data;
using piDash.Models;
using Microsoft.EntityFrameworkCore;
using DarkSky.Models;

namespace piDash.Services
{
    public class WeatherService : IWeatherService
    { 
        public async Task<WeatherItem> GetForecastAsync()
        {
            WeatherItem weatherForecast = new WeatherItem();
            string apiKey = "";

            var fileStream = new FileStream("darkskyAPI.txt", FileMode.Open, FileAccess.Read);
            using (var stream = new StreamReader(fileStream))
            {
                apiKey = stream.ReadToEnd();
            }

            var darkSky = new DarkSky.Services.DarkSkyService(apiKey);

            var forecast = await darkSky.GetForecast(51.4557, 2.583,
                new OptionalParameters
                {
                    MeasurementUnits = "si"
                });

            if (forecast?.IsSuccessStatus == true)
            {
                weatherForecast.CurrentFeelsLikeTemperature = Convert.ToInt32(forecast.Response.Currently.ApparentTemperature);
                weatherForecast.CurrentSummary = forecast.Response.Currently.Summary;
                weatherForecast.CurrentTemperature = Convert.ToInt32(forecast.Response.Currently.Temperature);
                weatherForecast.CurrentTempLow = Convert.ToInt32(forecast.Response.Currently.ApparentTemperatureLow);
                weatherForecast.CurrentTempHigh = Convert.ToInt32(forecast.Response.Currently.ApparentTemperatureHigh);
                weatherForecast.CurrentIconPath = "/images/" + forecast.Response.Currently.Icon.ToString() + ".svg";

                weatherForecast.MinutelySummary = forecast.Response.Minutely.Summary;

                List<HourData> hourData = new List<HourData>();

                foreach (var hour in forecast.Response.Hourly.Data.GetRange(0, 4))
                {
                    HourData hdata = new HourData();
                    hdata.time = DateTime.ParseExact(hour.DateTime.TimeOfDay.ToString(), "HH:mm:ss", new CultureInfo("en-GB")).ToString("HH:mm");
                    hdata.summary = hour.Summary;
                    hdata.temp = Convert.ToInt32(hour.Temperature);
                    hdata.icon = "/images/" + hour.Icon.ToString() + ".svg";
                    hourData.Add(hdata);
                }

                weatherForecast.Hour = hourData;

                List<DayData> dayData = new List<DayData>();

                foreach (var day in forecast.Response.Daily.Data.GetRange(1, 4))
                {
                    DayData ddata = new DayData();
                    ddata.day = DateTime.ParseExact(day.DateTime.Date.ToString(), "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("dd.MM yyyy");
                    ddata.summary = day.Summary;
                    ddata.temp = Convert.ToInt32(day.TemperatureLow);
                    ddata.icon = "images/" + day.Icon.ToString() + ".svg";
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