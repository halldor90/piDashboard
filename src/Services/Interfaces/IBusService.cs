using DayDash.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDash.Services
{
    public interface IBusService
    {
        Task<List<Bus>> GetBusScheduleAsync(IOptions<AppSettings> appSettings);
    }
}
