using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DayDash.Models;

namespace DayDash.Services
{
    public interface IBusService
    {
        Task<List<Bus>> GetBusScheduleAsync(IOptions<AppSettings> appSettings);
    }
}
