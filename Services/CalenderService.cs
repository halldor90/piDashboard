using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using piDash.Data;
using piDash.Models;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace piDash.Services
{
    public class CalenderService : ICalenderService
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        public Task<List<CalenderItem>> GetCalenderItemsAsync()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentialsCalendar.json", FileMode.Open, FileAccess.Read))
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
                                calenderItem.StartsAt = DateTime.ParseExact(when, "dd/MM/yyyy HH:mm:ss", new CultureInfo("en-GB")).ToString("dddd, dd MMMM yyyy HH:mm");
                                calenderItems.Add(calenderItem);

                            }                    
                        }           
                    }                    
                }
            }
            
            calenderItems = calenderItems.OrderBy(x => DateTime.ParseExact(x.StartsAt, "dddd, dd MMMM yyyy HH:mm",new CultureInfo("en-GB"))).ToList();

            return Task.FromResult(calenderItems);
        } 
    }
}