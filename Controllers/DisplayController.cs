using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DayDash.Services;
using Microsoft.Extensions.Options;

namespace DayDash.Controllers
{
    public class DisplayController : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ICalenderService _calenderService;
        private readonly IWeatherService _weatherService;
        private readonly IBusService _busService;

        public DisplayController(IOptions<AppSettings> appSettings, ICalenderService calenderService, IWeatherService weatherService, IBusService busService)
        {
            _appSettings = appSettings;
            _calenderService = calenderService;
            _weatherService = weatherService;
            _busService = busService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _calenderService.GetCalenderItemsAsync();
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