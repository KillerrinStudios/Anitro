using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Anitro.Data_Structures;
using Anitro.Data_Structures.API_Classes;

using Anitro.APIs.Events;

using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.ApplicationModel.Core;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.API_Classes.Hummingbird.Unofficial_APIs;

namespace Anitro.APIs.Hummingbird
{
    public static class APIUnofficial
    {
        public static event APIFeedbackEventHandler FeedbackEventHandler;
        public static event APICompletedEventHandler APICompletedEventHandler;

        public static class Get
        {
            public static async Task<APICompletedEventArgs> Calendar(string username, bool fireEventOff = true)
            {
                // Create the arguments for future usage
                APICompletedEventArgs args;

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///
                Debug.WriteLine("Calendar(): Entering");

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message                                                     
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://cal.hb.cybrox.eu/api.php?user="+ username);

                try
                {
                    // Send the request to the server
                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Calendar(): Success Code");
                        // Just as an example I'm turning the response into a string here
                        string responseAsString = await response.Content.ReadAsStringAsync();

                        Debug.WriteLine(responseAsString);
                        
                        JObject o = JObject.Parse(responseAsString); // This would be the string you defined above
                        CalendarRootObject rawCalendarInfo = JsonConvert.DeserializeObject<CalendarRootObject>(o.ToString()); ;

                        List<CalendarEntry> calendarEntryList = new List<CalendarEntry>();
                        foreach (var entry in rawCalendarInfo.dataset)
                        {
                            calendarEntryList.Add(new CalendarEntry(entry));
                        }

                        Debug.WriteLine("Calendar(): Exiting");

                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.CalendarInfo, calendarEntryList);

                        if (fireEventOff)
                        {
                            APICompletedEventHandler(args.UserState, args);
                        }
                        return args;
                    }
                }
                catch (Exception)
                {
                    args = new APICompletedEventArgs(APIResponse.Failed, APIType.CalendarInfo);
                    if (fireEventOff)
                    {
                        APICompletedEventHandler(args.UserState, args);
                    }
                    return args;
                }

                args = new APICompletedEventArgs(APIResponse.Failed, APIType.UserInfo);
                if (fireEventOff)
                {
                    APICompletedEventHandler(args.UserState, args);
                }
                return args;
            }
        }

        public static class Post
        {

        }
    }
}
