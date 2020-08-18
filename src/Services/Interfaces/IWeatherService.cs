using DayDash.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DayDash.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetForecastAsync(IOptions<AppSettings> appSettings);
    }
}
