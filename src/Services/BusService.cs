using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DayDash.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace DayDash.Services
{
    public class BusService : IBusService
    {
        public async Task<List<Bus>> GetBusScheduleAsync(IOptions<AppSettings> appSettings)
        {
            var busUrl = CreateBusUrl(appSettings);

            BusLines busLines = await GetBusesAsync(busUrl); 

            return CreateBusList(busLines);
        }

        private string CreateBusUrl(IOptions<AppSettings> appSettings)
        {
            var busUrl = $"https://transportapi.com/v3/uk/bus/stop/{ appSettings.Value.BusStop }/live.json?app_id={ appSettings.Value.BusAppId }&app_key={ appSettings.Value.BusAppKey }&group=route&nextbuses=no";

            return busUrl;
        }

        private async Task<BusLines> GetBusesAsync(string busUrl)
        {
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(busUrl);

            var responseBody = response.Content.ReadAsStringAsync().Result;
            var busResponse = JsonConvert.DeserializeObject<BusSchedule>(responseBody);

            return busResponse.BusLines;
        }

        private List<Bus> CreateBusList(BusLines busLines)
        {
            List<Bus> buses = new List<Bus>();

            if (busLines.m1.Any())
            {
                buses = busLines.m1;

                if (busLines.m3?.Count() > 0)
                {
                    buses.AddRange(busLines.m3);
                }

                buses = buses.GroupBy(x => new {x.EstimatedDepartureTime, x.BusName}).Select(x => x.First()).OrderBy(b => b.EstimatedDepartureTime).ToList();
            }

            return buses;
        }
    }
}