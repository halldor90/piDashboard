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
    public class CalenderService : ICalenderService
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        public Task<List<CalenderItem>> GetCalenderItemsAsync()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
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

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            List<CalenderItem> calenderItems = new List<CalenderItem>();

            var calendarList = service.CalendarList.List().Execute();
            IList<CalendarListEntry> calItems = calendarList.Items;

            List<Events> allEvents = new List<Events>();

            if (calItems.Count > 0)
            {
                foreach (var calItem in calItems)
                {
                    // Define parameters of request.
                    EventsResource.ListRequest request = service.Events.List(calItem.Id);
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
                                CalenderItem calenderItem = new CalenderItem();     
                                calenderItem.Summary = eventItem.Summary;
                                calenderItem.ColorId = calItem.BackgroundColor;
                                
                                string when = eventItem.Start.DateTime.ToString();
                                
                                if (String.IsNullOrEmpty(when))
                                {
                                    when = eventItem.Start.Date;
                                }
                                
                                if (eventItem.Start.DateTime?.Date == DateTime.Today.Date || DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")) == DateTime.Today.Date)
                                {
                                    calenderItem.Date = "Today";
                                }
                                else if (eventItem.Start.DateTime?.Date == DateTime.Today.AddDays(1).Date || DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")) == DateTime.Today.AddDays(1).Date)
                                {
                                    calenderItem.Date = "Tomorrow";
                                }
                                else
                                {
                                    calenderItem.Date = DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("dd MMMM");
                                }

                                calenderItem.Time = DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("HH:mm");

                                calenderItem.DateTime = DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("dd MMMM HH:mm");
                                
                                if (calenderItems.Count > 0 && calenderItems.Last().Date == calenderItem.Date)
                                {
                                    calenderItem.isSameDate = true;
                                }

                                calenderItems.Add(calenderItem);                              
                            }                    
                        }           
                    }                    
                }
            }

            calenderItems = calenderItems.OrderBy(c => c.DateTime).ToList();

            for (int i = 1; i < calenderItems.Count; i++)
            {
                if (calenderItems[i - 1].Date == calenderItems[i].Date)
                {
                    calenderItems[i].isSameDate = true;
                }
            }
            
            return Task.FromResult(calenderItems);
        } 
    }
}