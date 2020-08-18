using DayDash.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDash.Services
{
    public interface ICalendarService
    {
        Task<List<CalendarItem>> GetCalendarItemsAsync();
    }
}
