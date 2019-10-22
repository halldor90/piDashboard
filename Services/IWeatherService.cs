using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using piDash.Models;

namespace piDash.Services
{
    public interface IWeatherService
    {
        Task<WeatherItem> GetForecastAsync();
    }
}
