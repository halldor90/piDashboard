using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DayDash.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Humanizer;
using Microsoft.Extensions.Options;

namespace DayDash.Services
{
    public class BusService : IBusService
    {
        private static readonly HttpClient client = new HttpClient();


        public async Task<List<Bus>> GetBusScheduleAsync(IOptions<AppSettings> appSettings)
        {
            var response = await client.GetAsync($"https://transportapi.com/v3/uk/bus/stop/{ appSettings.Value.BusStop }/live.json?app_id={ appSettings.Value.BusAppId }&app_key={ appSettings.Value.BusAppKey }&group=route&nextbuses=no");

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var busRequest = JsonConvert.DeserializeObject<BusSchedule>(responseBody);

            List<Bus> buses = busRequest.BusLines.m1;

            if (busRequest.BusLines.m3.Any())
            {
                buses.AddRange(busRequest.BusLines.m3);
            }

            buses = buses.GroupBy(x => new {x.Time, x.BusName}).Select(x => x.First()).OrderBy(b => b.Time).ToList();

            foreach (var bus in buses)
            {
                bus.Time = DateTime.Parse(bus.Time).Humanize();
            }

            return buses;
        }
    }
}