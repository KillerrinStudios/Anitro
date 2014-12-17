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

namespace Anitro.APIs.Hummingbird
{
    public static class APIv2
    {
        public static event APIFeedbackEventHandler FeedbackEventHandler;
        public static event APICompletedEventHandler APICompletedEventHandler;

        public static class Get
        {
            public static async Task<APICompletedEventArgs> UserInfo(string username, bool fireEventOff = true)
            {
                // Create the arguments for future usage
                APICompletedEventArgs args;

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///
                Debug.WriteLine("UserInfo(): Entering");

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message                                                     
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("user_infos/" + username, HummingbirdAPILevel.None, true)); //"http://hummingbird.me/user_infos/" + username); 

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                try
                {
                    // Send the request to the server
                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("UserInfo(): Success Code");
                        // Just as an example I'm turning the response into a string here
                        string responseAsString = await response.Content.ReadAsStringAsync();

                        //Debug.WriteLine(responseAsString);
                        
                        JObject o = JObject.Parse(responseAsString); // This would be the string you defined above
                        Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfoRootObject userInfo = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfoRootObject>(o.ToString()); ;

                        Debug.WriteLine("GetAnime(): Exiting");

                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.UserInfo, userInfo);

                        if (fireEventOff)
                        {
                            APICompletedEventHandler(args.UserState, args);
                        }
                        return args;
                    }
                }
                catch (Exception)
                {
                    args = new APICompletedEventArgs(APIResponse.Failed, APIType.UserInfo);
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
