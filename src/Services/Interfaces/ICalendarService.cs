using System.Collections.Generic;
using System.Threading.Tasks;
using DayDash.Models;

namespace DayDash.Services
{
    public interface ICalendarService
    {
        Task<List<CalendarItem>> GetCalendarItemsAsync();
    }
}
