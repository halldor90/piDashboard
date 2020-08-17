using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DayDash.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace DayDash.Services
{
    public class DayCalendarService : ICalendarService
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "DayDashboard";
        public Task<List<CalendarItem>> GetCalendarItemsAsync()
        {
            UserCredential credential = GetCredential();

            CalendarService service = CreateCalenderService(credential);

            var calendarItems = new List<CalendarItem>();

            var calendarsList = service.CalendarList.List().Execute().Items;

            if (calendarsList.Count > 0)
            {
                calendarItems = GetAllCalendarItems(calendarsList, service);
            }            

            return Task.FromResult(calendarItems);
        }

        private UserCredential GetCredential()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            return credential;
        }

        private CalendarService CreateCalenderService(UserCredential credential)
        {
            //Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        private List<CalendarItem> GetAllCalendarItems(IList<CalendarListEntry> calendarsList, CalendarService service)
        {
            var calendarItems = new List<CalendarItem>();

            foreach (var calendar in calendarsList)
            {
                // Define parameters of request.
                EventsResource.ListRequest request = service.Events.List(calendar.Id);
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                Events events = request.Execute();

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        if (eventItem.Start.DateTime <= DateTime.Today.AddDays(3))
                        {
                            CalendarItem calenderItem = new CalendarItem();
                            calenderItem.Summary = eventItem.Summary;
                            calenderItem.ColorId = calendar.BackgroundColor;

                            calenderItem = SetCalendarItemDateTime(calenderItem, eventItem);

                            if (calendarItems.Count > 0 && calendarItems.Last().Date == calenderItem.Date)
                            {
                                calenderItem.IsSameDate = true;
                            }

                            calendarItems.Add(calenderItem);
                        }
                    }
                }
            }

            calendarItems = SortDateTime(calendarItems);

            return calendarItems;
        }

        private CalendarItem SetCalendarItemDateTime(CalendarItem calenderItem, Event eventItem)
        {
            string startTime = eventItem.Start.DateTime.ToString();

            if (String.IsNullOrEmpty(startTime))
            {
                startTime = eventItem.Start.Date;
            }

            if (eventItem.Start.DateTime?.Date == DateTime.Today.Date || ParseExactDateTime(startTime) == DateTime.Today.Date)
            {
                calenderItem.Date = "Today";
            }
            else if (eventItem.Start.DateTime?.Date == DateTime.Today.AddDays(1).Date || ParseExactDateTime(startTime) == DateTime.Today.AddDays(1).Date)
            {
                calenderItem.Date = "Tomorrow";
            }
            else
            {
                calenderItem.Date = ParseExactDateTime(startTime).ToString("dd MMMM");
            }

            calenderItem.Time = ParseExactDateTime(startTime).ToString("HH:mm");
            calenderItem.DateTime = ParseExactDateTime(startTime).ToString("dd MMMM HH:mm");

            return calenderItem;
        }

        private DateTime ParseExactDateTime(string startTime)
        {
            return DateTime.ParseExact(startTime, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB"));
        }

        private List<CalendarItem> SortDateTime(List<CalendarItem> calendarItems)
        {
            calendarItems = calendarItems.OrderBy(c => c.DateTime).ToList();

            for (int i = 1; i < calendarItems.Count; i++)
            {
                if (calendarItems[i - 1].Date == calendarItems[i].Date)
                {
                    calendarItems[i].IsSameDate = true;
                }
            }

            return calendarItems;
        }
    }
}