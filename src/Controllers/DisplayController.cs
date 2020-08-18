using DayDash.Models;
using DayDash.Services;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DayDash.Controllers
{
    public class DisplayController : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ICalendarService _calendarService;
        private readonly IWeatherService _weatherService;
        private readonly IBusService _busService;

        public DisplayController(IOptions<AppSettings> appSettings, ICalendarService calendarService, IWeatherService weatherService, IBusService busService)
        {
            _appSettings = appSettings;
            _calendarService = calendarService;
            _weatherService = weatherService;
            _busService = busService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _calendarService.GetCalendarItemsAsync();
            var forecast = await _weatherService.GetForecastAsync(_appSettings);
            var buses = await _busService.GetBusScheduleAsync(_appSettings);

            dynamic model = new ExpandoObject();
            model.Events = events;
            model.Forecast = forecast;
            model.Buses = buses;

            return View(model);
        }
    }
}