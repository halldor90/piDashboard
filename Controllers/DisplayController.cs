using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using piDash.Services;
using piDash.Models;

namespace piDash.Controllers
{
    public class DisplayController : Controller
    {
        private readonly ICalenderService _calenderService;

        private readonly IWeatherService _weatherService;

        public DisplayController(ICalenderService calenderService, IWeatherService weatherService)
        {
            _calenderService = calenderService;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _calenderService.GetCalenderItemsAsync();
            var forecast = await _weatherService.GetForecastAsync();

            dynamic model = new ExpandoObject();
            model.Events = events;
            model.Forecast = forecast;

            return View(model);
        }
    }
}