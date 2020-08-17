using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DayDash.Models;

namespace DayDash.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetForecastAsync(IOptions<AppSettings> appSettings);
    }
}
