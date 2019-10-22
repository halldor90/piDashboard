using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using piDash.Models;

namespace piDash.Services
{
    public interface ICalenderService
    {
        Task<List<CalenderItem>> GetCalenderItemsAsync();
    }
}
