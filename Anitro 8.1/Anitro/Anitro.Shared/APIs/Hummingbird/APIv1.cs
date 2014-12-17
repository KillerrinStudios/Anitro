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
    public static class APIv1
    {
        public static event APIFeedbackEventHandler FeedbackEventHandler;
        public static event APICompletedEventHandler APICompletedEventHandler;
        private static User user;

        public static class Get
        {
            public static class Parsers
            {
                public static bool ParseActivityFeed(ref User user, Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedList aF)
                {
                    Debug.WriteLine("ParseActivityFeed(): Entering");
                    try
                    {
                        //StorageTools.SaveFileFromServer(new Uri(aF.status_feed[0].user.avatar, UriKind.Absolute), StorageTools.StorageConsts.AvatarImage);

                        foreach (Anitro.Data_Structures.API_Classes.Hummingbird.V1.StatusFeedObject sFO in aF.status_feed)
                        {
                            Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject temp = new Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject { };
                            string contentString;

                            if (string.IsNullOrEmpty(user.AvatarURL))//Consts.settings.userAvatar))
                            {
                                Debug.WriteLine("Saving avatar");
                                //Storage.SaveFileFromServer(new Uri(sFO.user.avatar, UriKind.Absolute), "avatar.jpg");
                                //    Debug.WriteLine("Avatar done saving");

                                user.AvatarURL = sFO.user.avatar; //Storage.Settings.User.userAvatar.Value = sFO.user.avatar;
                                //Consts.settings.userAvatar = sFO.user.avatar;
                                //Storage.SaveSettingsInfo();
                            }

                            switch (sFO.story_type)
                            {
                                case "media_story":
                                    string tts = sFO.updated_at.Substring(0, sFO.updated_at.Length - 1);
                                    string[] tS = tts.Split('T');

                                    if (sFO.substories[0].substory_type == "watchlist_status_update")
                                    {
                                        switch (sFO.substories[0].new_status)
                                        {
                                            case "currently_watching":
                                                contentString = sFO.user.name + " is currently watching";
                                                break;
                                            case "plan_to_watch":
                                                contentString = sFO.user.name + " plans to watch";
                                                break;
                                            case "completed":
                                                contentString = sFO.user.name + " has completed";
                                                break;
                                            case "on_hold":
                                                contentString = sFO.user.name + " has placed on hold";
                                                break;
                                            case "dropped":
                                                contentString = sFO.user.name + " has dropped";
                                                break;
                                            default:
                                                contentString = "";
                                                break;
                                        }
                                    }
                                    else if (sFO.substories[0].substory_type == "watched_episode")
                                    {
                                        contentString = sFO.user.name + " watched episode " + sFO.substories[0].episode_number;
                                    }
                                    else { contentString = ""; }

                                    string storyImageString = "";

                                    temp = new Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject
                                    {
                                        slug = sFO.media.slug,
                                        storyImage = sFO.media.cover_image,
                                        header = sFO.media.title,
                                        content = contentString,
                                        timeStamp = DateTime.Parse(tts),//tS[0] + " at " + tS[1],
                                    };

                                    //Debug.WriteLine(temp.header + " | "+ tts);
                                    break;
                                case "comment":
                                    string tts2 = sFO.updated_at.Substring(0, sFO.updated_at.Length - 1);
                                    string[] tS2 = tts2.Split('T');

                                    temp = new Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject
                                    {
                                        storyImage = sFO.poster.avatar, //storyimage, //sFO.user.avatar,
                                        header = sFO.poster.name, //sFO.user.name,
                                        content = sFO.substories[0].comment,
                                        timeStamp = DateTime.Parse(tts2),//tS2[0] + " at " + tS2[1],
                                    };
                                    break;
                                case "followed":
                                    string tts3 = sFO.updated_at.Substring(0, sFO.updated_at.Length - 1);
                                    string[] tS3 = tts3.Split('T');

                                    temp = new Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject
                                    {
                                        storyImage = sFO.user.avatar, //storyimage, //sFO.user.avatar,
                                        header = sFO.user.name, //sFO.user.name,
                                        content = "has followed " + sFO.substories[0].followed_user.name,
                                        timeStamp = DateTime.Parse(tts3),//tS3[0] + " at " + tS3[1],
                                    };
                                    break;
                                default:
                                    Debug.WriteLine(sFO.story_type);
                                    break;
                            }

                            //activityFeedObject.Add(temp);
                            user.activityFeed.Add(temp);
                        }

                        Debug.WriteLine("ParseActivityFeed(): Exiting Success!");
                        return true;
                    }
                    catch (Exception) { Debug.WriteLine("ParseActivityFeed(): Exiting Failed"); return false; }
                }
                public static List<Anime> ParseSearchResult(string searchResponseAsString)
                {
                    Debug.WriteLine("ParseSearchResult(): Entering");
                    //Debug.WriteLine(searchResponseAsString);

                    if (String.IsNullOrWhiteSpace(searchResponseAsString)) { Debug.WriteLine("ParseSearchResult(): String is null or empty"); return new List<Anime>(); }
                    else
                    {
                        Debug.WriteLine("ParseSearchResult(): String is Populated");
                        string response = "{\"anime\":" + searchResponseAsString + "}";

                        JObject o = JObject.Parse(response); // This would be the string you defined above
                        //Debug.WriteLine("Parsed");
                        Anitro.Data_Structures.API_Classes.Hummingbird.V1.AnimeList ani = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.AnimeList>(o.ToString());
                        //Debug.WriteLine("Parsing Lib parsed");

                        Debug.WriteLine("ParseSearchResult(): Exiting");
                        return APIConverter.AnimeList(ani);
                    }
                }
                public static List<Anime> ParseFavouritesResult(string searchResponseAsString)
                {
                    Debug.WriteLine("ParseFavouritesResult(): Entering");
                    //Debug.WriteLine(searchResponseAsString);

                    if (String.IsNullOrWhiteSpace(searchResponseAsString)) { Debug.WriteLine("ParseFavouritesResult(): String is null or empty"); return new List<Anime>(); }
                    else
                    {
                        Debug.WriteLine("ParseFavouritesResult(): String is Populated");
                        string response = "{\"anime\":" + searchResponseAsString + "}";

                        JObject o = JObject.Parse(response); // This would be the string you defined above
                        //Debug.WriteLine("Parsed");

                        Anitro.Data_Structures.API_Classes.Hummingbird.V1.AnimeList ani = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.AnimeList>(o.ToString());
                        //Debug.WriteLine("Parsing Lib parsed");

                        Debug.WriteLine("ParseFavouritesResult(): Exiting");
                        return APIConverter.AnimeList(ani);
                    }
                }
                public static async Task<bool> ParseAnimeLibrary(string responseAsString, LibrarySelection status) //async Task<List<LibraryObject>>
                {
                    Debug.WriteLine("ParseLibrary(" + status.ToString() + "): Entering");

                    //Debug.WriteLine(responseAsString);

                    if (String.IsNullOrWhiteSpace(responseAsString))
                    {
                        Debug.WriteLine("ParseLibrary(): Exiting");
                        return false;
                    }
                    else
                    {
                        //Debug.WriteLine("Parsing Library");
                        string response = "{\"library\":" + responseAsString + "}";

                        //Debug.WriteLine(response);

                        JObject o = JObject.Parse(response); // This would be the string you defined above
                        //Debug.WriteLine("Parsed");
                        Anitro.Data_Structures.API_Classes.Hummingbird.V1.LibraryList lib = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.LibraryList>(o.ToString()); ;
                        //Debug.WriteLine("Parsing Lib parsed");

                        foreach (var lO in lib.library)
                        {
                            Debug.WriteLine("Parsed: " + lO.anime.title + " | " + lO.status);
                            Anitro.Data_Structures.API_Classes.Hummingbird.V1.LibraryObject tempAnimeObj = lO;

                            tempAnimeObj.anime.genres = new List<Anitro.Data_Structures.API_Classes.Hummingbird.V1.Genre> { new Anitro.Data_Structures.API_Classes.Hummingbird.V1.Genre { name = "" } };

                            if (!String.IsNullOrEmpty(tempAnimeObj.rating.value)) { tempAnimeObj.rating.valueAsDouble = System.Convert.ToDouble(tempAnimeObj.rating.value); }
                            else { tempAnimeObj.rating.valueAsDouble = 0.0; }

                            LibraryObject tempAnimeObject = new LibraryObject(tempAnimeObj);

                            switch (status)
                            {
                                case LibrarySelection.All:
                                case LibrarySelection.None:
                                    user.animeLibrary.AddToLibrary(
                                                        tempAnimeObject.Status,
                                                        tempAnimeObject,
                                                        false,
                                                        false);
                                    break;
                                case LibrarySelection.CurrentlyWatching:
                                    user.animeLibrary.AddToLibrary(LibrarySelection.CurrentlyWatching, tempAnimeObject, false, false);
                                    break;
                                case LibrarySelection.PlanToWatch:
                                    user.animeLibrary.AddToLibrary(LibrarySelection.PlanToWatch, tempAnimeObject, false, false);
                                    break;
                                case LibrarySelection.Completed:
                                    user.animeLibrary.AddToLibrary(LibrarySelection.Completed, tempAnimeObject, false, false);
                                    break;
                                case LibrarySelection.OnHold:
                                    user.animeLibrary.AddToLibrary(LibrarySelection.OnHold, tempAnimeObject, false, false);
                                    break;
                                case LibrarySelection.Dropped:
                                    user.animeLibrary.AddToLibrary(LibrarySelection.Dropped, tempAnimeObject, false, false);
                                    break;
                            }
                        }

                        Debug.WriteLine("ParseLibrary(" + status.ToString() + "): Exiting");

                        return true;
                    }
                }
            }
            public static class Streamlining
            {
                public static async Task<bool> GenerateAllAnimeLibraries(User _user, LibraryType libType, LibrarySelection library)
                {
                    APICompletedEventArgs args;
                    if (!Consts.IsConnectedToInternet() ||
                        Consts.isApplicationClosing) 
                    {
                        args = new APICompletedEventArgs(APIResponse.Failed, APIType.LoadLibrary);
                        APICompletedEventHandler(args.UserState, args);
                        return false; 
                    }

                    Debug.WriteLine("Generating Library: all");

                    user = new User(_user);
                    
                    Task<string> aniLibrary = Get.AnimeLibrary(LibrarySelection.All, true); //GetLibrary("");
                    string aniLibResult = await aniLibrary;

                    APICompletedEventArgs favEvtArgs = await Get.AnimeFavourites(user.Username, false);
                    switch (favEvtArgs.Result)
                    {
                        case APIResponse.Successful:
                            Debug.WriteLine("Generating Library(): Favourites Success!");

                            foreach (Anime a in (List<Anime>)favEvtArgs.ResultObject)
                            {
                                LibraryObject libraryObject = new LibraryObject(a);
                                user.animeLibrary.Favourites.Add(libraryObject);
                            }
                            break;
                        case APIResponse.Failed:
                            Debug.WriteLine("Generating Library(): Favourites Failed");
                            break;
                    }

                    // Finally, Save Favorites
                    Debug.WriteLine("Generating Library: favourites");
                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.LoadLibrary, user);

                    if (user.IsLoggedIn)
                    {
                        user.animeLibrary.lastPulled = DateTime.Now;

                        Consts.UpdateLoggedInUser(user);
                        bool savingResult = await Consts.LoggedInUser.Save();


                        Debug.WriteLine("GenerateAllAnimeLibraries(): Save Complete");
                        APICompletedEventHandler(args.UserState, args);

                        return savingResult;
                    }

                    APICompletedEventHandler(args.UserState, args);
                    return false;   
                }

                public static async Task<APICompletedEventArgs> AllUserInfo(string userName, bool fireEventOff = true)
                {
                    APICompletedEventArgs args;
                    UserInfo userinfo = new UserInfo();

                    Debug.WriteLine("AllUserInfo(): Getting UserInfo");

                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Grabbing User Info"));
                    APICompletedEventArgs userEventArgs = await Get.UserInfo(userName, false);
                    switch (userEventArgs.Result)
                    {
                        case APIResponse.Successful:
                            Debug.WriteLine("AllUserInfo(): UserInfo Recieved Successfully!");
                            userinfo = (userEventArgs.ResultObject as Data_Structures.API_Classes.UserInfo);
                            
                            //Debug.WriteLine(userinfo.name);
                            //Debug.WriteLine(userinfo.waifu);
                            //Debug.WriteLine(userinfo.waifu_slug);
                            //Debug.WriteLine(userinfo.waifu_char_id);

                            break;
                        default:
                            Debug.WriteLine("AllUserInfo(): UserInfo Failed");
                            break;
                    }

                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Attempting Extra User Info"));
                    APICompletedEventArgs undoc_userEventArgs;

                    try
                    {
                        Debug.WriteLine("Getting Undocumented UserInfo");
                        undoc_userEventArgs = await APIv2.Get.UserInfo(userName, false);
                        Debug.WriteLine("Recieved Undocumented User Info");

                        //Debug.WriteLine(undoc_userEventArgs.Result.ToString());
                        //if (undoc_userEventArgs.Result == APIResponse.Successful)
                        //    Debug.WriteLine("Successful");

                        switch (undoc_userEventArgs.Result)
                        {
                            case APIResponse.Successful:
                                Debug.WriteLine("AllUserInfo(): Undocumented_UserInfo Recieved Successfully!");
                                Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfo undoc_UserInfo = (undoc_userEventArgs.ResultObject as Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfoRootObject).user_info;
                                userinfo.AddUnDocumentedToDocumented(new UserInfo(undoc_UserInfo));

                                FeedbackEventHandler(null, new APIFeedbackEventArgs("Extra User Info Succeeded"));
                                break;
                            default:
                                Debug.WriteLine("AllUserInfo(): Undocumented_UserInfo Failed");
                                FeedbackEventHandler(null, new APIFeedbackEventArgs("Extra User Info Failed"));
                                break;
                        }
                    }
                    catch(Exception)
                    {
                        undoc_userEventArgs = new APICompletedEventArgs(APIResponse.Failed, APIType.UserInfo);
                        FeedbackEventHandler(null, new APIFeedbackEventArgs("Extra User Info Failed"));
                    }

                    Debug.WriteLine("AllUserInfo(): Exiting");
                    args = new APICompletedEventArgs(userEventArgs.Result, userEventArgs.Type, userinfo);
                    if (fireEventOff)
                    {
                        APICompletedEventHandler(args.UserState, args);
                    }
                    return args;
                }
            }

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
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("users/" + username, HummingbirdAPILevel.Version1, true)); //"https://hbrd-v1.p.mashape.com/users/" + username); //http://hummingbird.me/user_infos/killerrin

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                try
                {
                    // Send the request to the server
                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Response is Success Code");

                        // Just as an example I'm turning the response into a string here
                        string responseAsString = await response.Content.ReadAsStringAsync();

                        //Debug.WriteLine(responseAsString);

                        JObject o = JObject.Parse(responseAsString); // This would be the string you defined above
                        Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfo userInfo = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfo>(o.ToString()); ;

                        Debug.WriteLine("UserInfo(): Converting to Universal Data Structure");

                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.UserInfo, new UserInfo(userInfo));
                        if (fireEventOff)
                        {
                            APICompletedEventHandler(args.UserState, args);
                        }

                        Debug.WriteLine("UserInfo(): Exiting");
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

            public static async Task Anime(string _animeSlug)
            {
                // Create the arguments for future usage
                APICompletedEventArgs args;

                /// ------------------------------------------------ ///
                /// Double Check if _anime string is API Compliant.  ///
                /// ------------------------------------------------ ///
                string animeSearchTerm = Helpers.ConvertToAPIConpliantString(_animeSlug, ' ', '-');

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///
                Debug.WriteLine("GetAnime(): Entering: " + animeSearchTerm);

                // Create a client
                HttpClient httpClient = new HttpClient(); 

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("anime/" + animeSearchTerm + "/", HummingbirdAPILevel.Version1)); //"https://hbrd-v1.p.mashape.com/anime/" + animeSearchTerm); //"http://hummingbird.me/search?query="+uri);//

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                 
                if (response.IsSuccessStatusCode)
                {
                    // Just as an example I'm turning the response into a string here
                    string responseAsString = await response.Content.ReadAsStringAsync();

                    //Debug.WriteLine(responseAsString);

                    JObject o = JObject.Parse(responseAsString); // This would be the string you defined above
                    Anitro.Data_Structures.API_Classes.Hummingbird.V1.Anime animeObject = JsonConvert.DeserializeObject<Anitro.Data_Structures.API_Classes.Hummingbird.V1.Anime>(o.ToString()); ;

                    Debug.WriteLine("GetAnime(): Exiting");

                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.GetAnime);
                    APICompletedEventHandler(new Anime(animeObject), args);
                    return;
                }
                args = new APICompletedEventArgs(APIResponse.Failed, APIType.GetAnime);
                APICompletedEventHandler(new Anime(), args);
                return;
            }
            public static async Task SearchAnime(string searchTerm)
            {
                Debug.WriteLine("Entering");
                APICompletedEventArgs args;

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("search/anime?query=" + searchTerm, HummingbirdAPILevel.Version1, true)); //"https://hbrd-v1.p.mashape.com/search/anime?query=" + searchTerm);

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    // Just as an example I'm turning the response into a string here
                    string responseAsString = await response.Content.ReadAsStringAsync();

                    //Debug.WriteLine(responseAsString + "\n\n");

                    List<Anime> animeList = Parsers.ParseSearchResult(responseAsString);
                    Debug.WriteLine("Search Parsed");

                    foreach (Anime a in animeList)
                    {
                        Debug.WriteLine(a.RomanjiTitle);
                    }

                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.Search);
                    APICompletedEventHandler(animeList, args);
                    return;
                }

                args = new APICompletedEventArgs(APIResponse.Failed, APIType.Search);
                APICompletedEventHandler(args.UserState, args);
                return;
            }
            public static async Task StatusFeed(User user, int pageIndex = 1)
            {
                Debug.WriteLine("GetStatusFeed(): Entering");

                // Create the arguments for future usage
                APICompletedEventArgs args;

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("users/" + user.Username + "/feed?page=" + pageIndex, HummingbirdAPILevel.Version1, true)); //"https://hbrd-v1.p.mashape.com/users/" + userName + "/feed?page=" + pageIndex); // http://hummingbird.me/api/v1/users/killerrin/feed?page=1
                Debug.WriteLine("GetStatusFeed(): Getting: " + requestMessage.RequestUri.OriginalString);

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("GetStatusFeed(): Response successful");
                    // Just as an example I'm turning the response into a string here
                    string responseAsString = await response.Content.ReadAsStringAsync();

                    //Debug.WriteLine(responseAsString);
                    
                    responseAsString = "{\"status_feed\":" + responseAsString + "}";

                    Debug.WriteLine("GetStatusFeed(): Parsing Library To List");
                    //Debug.WriteLine(responseAsString);

                    JObject o = JObject.Parse(responseAsString); // This would be the string you defined above
                    Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedList activityFeed = JsonConvert.DeserializeObject<Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedList>(o.ToString());


                    bool parseResult = Parsers.ParseActivityFeed(ref user, activityFeed);
                    //Consts.activityFeed = aFO;



                    //Debug.WriteLine("GetActivityFeed(): Exiting Successful");
                    //byte[] data = Encoding.UTF8.GetBytes(responseAsString);
                    //return Encoding.UTF8.GetString(data, 0, data.Length);
                    //return responseAsString;
                    if (parseResult)
                    {
                        Debug.WriteLine("GetActivityFeed(): Exiting Success!");
                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.GetActivityFeed, user);
                        APICompletedEventHandler(args.UserState, args);
                        return;
                    }
                }

                Debug.WriteLine("GetActivityFeed(): Exiting Failed");
                args = new APICompletedEventArgs(APIResponse.Failed, APIType.GetActivityFeed);
                APICompletedEventHandler(args.UserState, args);
                return;
            }

            public static async Task<string> AnimeLibrary(LibrarySelection status, bool calledFromStreamline = false)
            {
                APICompletedEventArgs args;

                string statusString = APIConverter.LibrarySelectionToString(status);

                Debug.WriteLine("GetLibrary(): Entering");

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("users/" + user.Username + "/library?status=" + statusString, HummingbirdAPILevel.Version1, true));

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                //Debug.WriteLine("Test");
                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                //string responseAsString

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("GetLibrary(): Response Successful");

                    // Turn the response into a string for parsing later
                    string responseAsString = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseAsString);

                    // Due to json randomness, chop off the last two characters
                    //responseAsString = responseAsString.Substring(0, responseAsString.Length - 2);

                    bool libraryParse = await Parsers.ParseAnimeLibrary(responseAsString, status);
                    //await libraryParse;

                    Debug.WriteLine("GetLibrary(): Exiting");

                    if (!calledFromStreamline)
                    {
                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.GetLibrary);
                        APICompletedEventHandler(args.UserState, args);
                    }

                    return responseAsString;
                }
                else
                {
                    // Do Stuff
                    Debug.WriteLine("GetLibrary(): Response Failed");

                    if (!calledFromStreamline)
                    {
                        args = new APICompletedEventArgs(APIResponse.Failed, APIType.GetLibrary);
                        APICompletedEventHandler(args.UserState, args);
                    }

                    return "";
                }
            }
            public static async Task<APICompletedEventArgs> AnimeFavourites(string username, bool fireEvent = true)
            {
                // Create the arguments for future usage
                APICompletedEventArgs args;

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///
                Debug.WriteLine("GetFavourites(): Entering: ");

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Helpers.CreateHummingbirdUrl("users/" + username + "/favorite_anime", HummingbirdAPILevel.Version1, true)); //"https://hbrd-v1.p.mashape.com/users/" + username + "/favorite_anime");

                // Add our custom headers
                //requestMessage.Headers.Add("Content-Type", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    // Just as an example I'm turning the response into a string here
                    string responseAsString = await response.Content.ReadAsStringAsync();

                    //Debug.WriteLine(responseAsString);

                    List<Anime> animeList = Parsers.ParseFavouritesResult(responseAsString);

                    foreach (Anime a in animeList)
                    {
                        Debug.WriteLine("GetFavourites(): " + a.RomanjiTitle);
                    }

                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.GetFavourites, animeList);

                    if (fireEvent)
                    {
                        APICompletedEventHandler(animeList, args);
                    }
                    return args;
                }

                args = new APICompletedEventArgs(APIResponse.Failed, APIType.GetFavourites);
                if (fireEvent)
                {
                    APICompletedEventHandler(args.UserState, args);
                }
                return args;
            }
        }
        public static class Post
        {
            public static async Task Login(string userMail, string password)
            {
                APICompletedEventArgs args;
                if (userMail == "" || password == "")
                {
                    args = new APICompletedEventArgs(APIResponse.InfoNotEntered, APIType.Login);
                    APICompletedEventHandler(args.UserState, args);
                    return;
                }
                if (userMail.Contains("@"))
                {
                    args = new APICompletedEventArgs(APIResponse.NotSupported, APIType.Login);
                    APICompletedEventHandler(args.UserState, args);
                    return;
                }

                bool testAccount = false;
                if (userMail == "killerrindev" && password == "testtest1") { testAccount = true; }

                Debug.WriteLine("Entering: Post.Login()");


                if (testAccount)
                {
                    Debug.WriteLine("Loading up the Test Account");

                    int networkSimulationInt = 1000;
                    for (int i = 0; i < networkSimulationInt; i++) { } //Simulate server activity
                    Debug.WriteLine("Network simulated");

                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Login Authorized"));
                    Debug.WriteLine("Feedback Sent");

                    Consts.LoggedInUser = await User.Load(true);
                    Debug.WriteLine("User Loaded from Solution");

                    for (int i = 0; i < networkSimulationInt; i++) { } //Simulate server activity
                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Grabbing User Info"));
                    Debug.WriteLine("Network Simulated Grabbing of User info");

                    for (int i = 0; i < networkSimulationInt; i++) { } //Simulate server activity
                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Extra User Info Succeeded"));
                    Debug.WriteLine("Network Simulated Grabbing of Extra User Info");
                    
                    FeedbackEventHandler(null, new APIFeedbackEventArgs("Login Successfull"));
                    Debug.WriteLine("Test user sucessfully loaded");
                    Debug.WriteLine("Exiting: Post.PostLogin()");

                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.Login);
                    APICompletedEventHandler(args.UserState, args);
                    Debug.WriteLine("Post.Login(): Exiting");

                    return;
                }
                else
                {
                    // Create a client
                    HttpClient httpClient = new HttpClient();
                    Debug.WriteLine("httpClient Created");

                    // Add a new Request Message
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Helpers.CreateHummingbirdUrl("users/authenticate/", HummingbirdAPILevel.Version1, true)); // "https://hbrd-v1.p.mashape.com/users/authenticate"); //
                    Debug.WriteLine("requestMessage Created");

                    // Add our custom headers
                    requestMessage.Headers.Add("accept", "application/json");
                    //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);
                    Debug.WriteLine("Headers Applied");

                    bool emailLogin;

                    // Determine Username or Email
                    if (userMail.Contains("@"))
                    {
                        Debug.WriteLine("User Logging in with Email");

                        emailLogin = true;
                        requestMessage.Content = new FormUrlEncodedContent(new[]
                                                {
                                                    new KeyValuePair<string,string>("email", userMail),
                                                    new KeyValuePair<string,string>("password", password)
                                                });

                    }
                    else
                    {
                        Debug.WriteLine("User Logging in with Username");

                        emailLogin = false;
                        requestMessage.Content = new FormUrlEncodedContent(new[]
                                                {
                                                    new KeyValuePair<string,string>("username", userMail),
                                                    new KeyValuePair<string,string>("password", password)
                                                });
                    }
                    Debug.WriteLine("Content applied");

                    // Send the request to the server
                    Debug.WriteLine("Sending Request ...");
                    HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                    Debug.WriteLine("Response Recieved");

                    //Debug.WriteLine(response.RequestMessage);
                    //Debug.WriteLine("\n");
                    //string responseAsStrings = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseAsStrings);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Response was successful");


                        FeedbackEventHandler(null, new APIFeedbackEventArgs("Login Authorized"));

                        // Grab the string and grab the content
                        Debug.WriteLine("Beginning Content Parse");
                        string responseAsString = await response.Content.ReadAsStringAsync();//.Result;
                        Debug.WriteLine("Response has been read to a string");

                        //Parse the responseAsString to remove ""'s
                        string _authToken = "";
                        char[] txtarr = responseAsString.ToCharArray();
                        foreach (char c in txtarr)
                        {
                            switch (c)
                            {
                                case '"':
                                    break;
                                default:
                                    _authToken += c;
                                    break;
                            }
                        }
                        Debug.WriteLine("AuthToken Parsed");

                        // Grab the Username and/or Email and then Login to the user
                        string userName = "";
                        string email = "";
                        if (emailLogin)
                        {
                            Debug.WriteLine("Beginning Email Login Parse");

                            // Set the EmailAddress to the user
                            email = userMail;

                            // Send a request to get username
                            userName = "me"; //userName = await GetUsernameFromServer(_authToken);
                        }
                        else
                        {
                            Debug.WriteLine("Beginning Username Login Parse");

                            // Set the username to the user
                            userName = userMail;
                        }


                        Consts.LoggedInUser.Login(userName, _authToken, password, email);
                        Debug.WriteLine("User Info Stored");

                        APICompletedEventArgs userEventArgs = await Get.Streamlining.AllUserInfo(userName, false);
                        switch (userEventArgs.Result)
                        {
                            case APIResponse.Successful:
                                Debug.WriteLine("AllUserInfo(): UserInfo Recieved Successfully!");
                                Consts.LoggedInUser.UserInfo = (userEventArgs.ResultObject as Data_Structures.API_Classes.UserInfo);
                                break;
                            default:
                                Debug.WriteLine("AllUserInfo(): UserInfo Failed");
                                break;
                        }


                        Debug.WriteLine("Exiting: Post.PostLogin()");
                        FeedbackEventHandler(null, new APIFeedbackEventArgs("Login Successfull"));


                        args = new APICompletedEventArgs(APIResponse.Successful, APIType.Login);
                        APICompletedEventHandler(args.UserState, args);

                        return;
                    }
                    else
                    {
                        // Do Stuff
                        if (!Consts.IsConnectedToInternet())
                        {
                            Debug.WriteLine("Network Error: PostLogin()");

                            args = new APICompletedEventArgs(APIResponse.NetworkError, APIType.Login);
                            APICompletedEventHandler(args.UserState, args);
                            return;
                        }
                        else if (await response.Content.ReadAsStringAsync() == "{\"error\":\"Invalid credentials\"}")
                        {
                            Debug.WriteLine("Invalid Login Credidentials: PostLogin()");

                            args = new APICompletedEventArgs(APIResponse.InvalidCredentials, APIType.Login);
                            APICompletedEventHandler(args.UserState, args);
                            return;
                        }
                        else
                        {
                            Debug.WriteLine("Error connecting to server: PostLogin()");

                            args = new APICompletedEventArgs(APIResponse.ServerError, APIType.Login);
                            APICompletedEventHandler(args.UserState, args);
                            return;
                        }
                    }
                }
            }
            public static async Task StatusUpdate(string _text)
            {
                Debug.WriteLine("PostStatusUpdate(): Entering");

                APICompletedEventArgs args;

                /// ------------------------------------------------ ///
                /// Double Check if _anime string is API Compliant.  ///
                /// ------------------------------------------------ ///
                string text = _text; // Helpers.ConvertToAPIConpliantString(_text, '+');

                /// --------------------------------------------------- ///
                /// Once _text string is API Compliant, begin the POST  ///
                /// --------------------------------------------------- ///

                //Debug.WriteLine("Creating Client");
                // Create a client
                HttpClient httpClient = new HttpClient();

                //Debug.WriteLine("Making Custom Message");
                // Add a new Request Message                                                                               Consts.settings.userName
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Helpers.CreateHummingbirdUrl("users/" + Consts.LoggedInUser.Username + "/comment.json", HummingbirdAPILevel.None, true)); //"http://hummingbird.me/users/" + Consts.LoggedInUser.Username + "/comment.json"); //"http://httpbin.org/post");

                //Debug.WriteLine("Setting Headers");
                // Add our custom headers
                requestMessage.Headers.Add("accept", "application/json");
                //requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                //Debug.WriteLine("Setting Content");
                // Add our Content
                requestMessage.Content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>("auth_token", Consts.LoggedInUser.AuthToken),//Consts.settings.auth_token),
                    new KeyValuePair<string,string>("comment", text),
                });

                //Debug.WriteLine("Sending Message");
                // Send the request to the server
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

                //Debug.WriteLine("Parsing the Response");
                if (response.IsSuccessStatusCode)
                {
                    // Grab the string and grab the content
                    string responseAsString = await response.Content.ReadAsStringAsync();//.Result;

                    DateTime dT = DateTime.UtcNow;
                    Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject temp = new Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject
                    {
                        storyImage = Consts.LoggedInUser.AvatarURL,//Consts.settings.userAvatar,
                        header = Consts.LoggedInUser.Username,//Consts.settings.userName,
                        content = _text,
                        timeStamp = dT, //dT.Date.Year + "-" + dT.Date.Month + "-" + dT.Date.Day + " at " +
                                                  //dT.TimeOfDay.Hours + ":" + dT.TimeOfDay.Minutes + ":" + dT.TimeOfDay.Seconds
                    };

                    Consts.LoggedInUser.activityFeed.Insert(0, temp);

                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.PostActivityFeed);
                    APICompletedEventHandler(args.UserState, args);
                    return;
                }

                args = new APICompletedEventArgs(APIResponse.Failed, APIType.PostActivityFeed);
                APICompletedEventHandler(args.UserState, args);
                return;
            }

            public static async Task<APICompletedEventArgs> LibraryUpdate(LibraryObject libraryObject, bool updateRating, bool fireEvent = true)
            {
                Debug.WriteLine("PostLibraryUpdate(LibraryObject libraryObject): Entering");
                APICompletedEventArgs args;

                if (libraryObject.Notes == null) { libraryObject.Notes = ""; }
                bool incrimentEpisodes = false;

                Debug.WriteLine("Posting: ");
                Debug.WriteLine(libraryObject.Anime.ServiceID);
                Debug.WriteLine(libraryObject.Status.ToString());
                Debug.WriteLine(libraryObject.Private.ToString());
                Debug.WriteLine(libraryObject.Rating);
                Debug.WriteLine(Convert.ToInt32(libraryObject.RewatchedTimes));
                Debug.WriteLine(libraryObject.Notes.ToString());
                Debug.WriteLine(Convert.ToInt32(libraryObject.EpisodesWatched));
                Debug.WriteLine(incrimentEpisodes);


                args = await LibraryUpdate(libraryObject.Anime.ServiceID,
                                           APIConverter.LibrarySelectionToString(libraryObject.Status),
                                           libraryObject.Private.ToString(),
                                           libraryObject.Rating.ToString(),
                                           libraryObject.RewatchedTimes,
                                           libraryObject.Notes,
                                           libraryObject.EpisodesWatched,
                                           incrimentEpisodes,
                                           updateRating,
                                           false);

                if (args.Result == APIResponse.Successful)
                {
                    //args = new APICompletedEventArgs(APIResponse.Successful, APIType.PostLibraryUpdate);

                    //AnimePage.dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    //{
                    if (fireEvent) { APICompletedEventHandler(args.UserState, args); }
                    //});
                }
                else
                {
                    //args = new APICompletedEventArgs(APIResponse.Failed, APIType.PostLibraryUpdate);
                    if (fireEvent) { APICompletedEventHandler(args.UserState, args); }
                }
                return args;
            }
            public static async Task<APICompletedEventArgs> LibraryUpdate(string _slug, string status, string privacy, string rating, int rewatchedTimes, string notes, int episodesWatched, bool incrimentEpisodes, bool updateRating, bool fireEventWhenComplete = true)
            {
                Debug.WriteLine("PostLibraryUpdate(): Entering");
                APICompletedEventArgs args;

                /// ------------------------------------------------ ///
                /// Double Check if _anime string is API Compliant.  ///
                /// ------------------------------------------------ ///
                string anime = Helpers.ConvertToAPIConpliantString(_slug, ' ', '-');

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///
                /// 

                try
                {
                    // Create a client
                    HttpClient httpClient = new HttpClient();

                    // Add a new Request Message
                    
                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Helpers.CreateHummingbirdUrl("libraries/" + anime, HummingbirdAPILevel.Version1, true)); //string uri = "https://hbrd-v1.p.mashape.com/libraries/" + anime;

                    // Add our custom headers
                    requestMessage.Headers.Add("accept", "application/json"); //"accept"
                    requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                    LibrarySelection libSel = Consts.LoggedInUser.animeLibrary.FindWhereExistsInLibrary(anime);
                    LibraryObject libObj = Consts.LoggedInUser.animeLibrary.GetObjectInLibrary(LibrarySelection.APISupported, anime);

                    Debug.WriteLine("Current Rating: " + libObj.Rating);
                    Debug.WriteLine("Changed Value: " + rating);


                    if (!updateRating) //rating == (Consts.LoggedInUser.animeLibrary.GetObjectInLibrary(libSel, anime)).rating.value)
                    {
                        Debug.WriteLine("Rating is the same, Dont update rating");
                        requestMessage.Content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string,string>("auth_token", Consts.LoggedInUser.AuthToken),//Consts.settings.auth_token),
                                new KeyValuePair<string,string>("status", status),
                                new KeyValuePair<string,string>("privacy", privacy),
                                new KeyValuePair<string,string>("rewatched_times", rewatchedTimes.ToString()),
                                new KeyValuePair<string,string>("notes", notes),
                                new KeyValuePair<string,string>("episodes_watched", episodesWatched.ToString()),
                                new KeyValuePair<string,string>("increment_episodes", (incrimentEpisodes.ToString()).ToLower())
                            });
                    }
                    else
                    {
                        Debug.WriteLine("Rating isn't the same, Update Rating");
                        requestMessage.Content = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string,string>("auth_token", Consts.LoggedInUser.AuthToken),//Consts.settings.auth_token),
                                new KeyValuePair<string,string>("status", status),
                                new KeyValuePair<string,string>("privacy", privacy),
                                new KeyValuePair<string,string>("rating", rating), // none = None Selected, 0-2 = Unhappy, 3 = Neutral, 4-5 = Happy
                                new KeyValuePair<string,string>("rewatched_times", rewatchedTimes.ToString()),
                                new KeyValuePair<string,string>("notes", notes),
                                new KeyValuePair<string,string>("episodes_watched", episodesWatched.ToString()),
                                new KeyValuePair<string,string>("increment_episodes", (incrimentEpisodes.ToString()).ToLower())
                            });
                    }

                    // Set Timeout
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 2, 0);

                    // Send the request to the server
                    Debug.WriteLine("Sending Server Request");

                    // await the response if we care about a reply.
                    Task<HttpResponseMessage> response = httpClient.SendAsync(requestMessage);
                    // ...thankfully, we dont care.

                    if (true)//response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Response Succeded");
                        //string responseAsString = await response.Content.ReadAsStringAsync();//.Result;
                        //Debug.WriteLine(responseAsString);
                        Debug.WriteLine("PostLibraryUpdate(): Exiting Succeeded");

                        if (fireEventWhenComplete)
                        {
                            args = new APICompletedEventArgs(APIResponse.Successful, APIType.PostLibraryUpdate);
                            APICompletedEventHandler(args.UserState, args);
                        }
                        else { args = new APICompletedEventArgs(); }
                        return args;
                    }

                    Debug.WriteLine("PostLibraryUpdate(): Exiting Failed");
                    if (fireEventWhenComplete)
                    {
                        args = new APICompletedEventArgs(APIResponse.Failed, APIType.PostLibraryUpdate);
                        APICompletedEventHandler(args.UserState, args);
                    }
                    else { args = new APICompletedEventArgs(); }
                    return args;
                }
                catch (Exception)
                {
                    Debug.WriteLine("PostLibraryUpdate(): Exiting Error");
                    if (fireEventWhenComplete)
                    {
                        args = new APICompletedEventArgs(APIResponse.Failed, APIType.PostLibraryUpdate);
                        APICompletedEventHandler(args.UserState, args);
                    }
                    else { args = new APICompletedEventArgs(); }
                    return args;
                }
            }
            public static async Task<APICompletedEventArgs> LibraryRemove(string _anime, bool fireOffEvent = true)
            {
                APICompletedEventArgs args;

                /// ------------------------------------------------ ///
                /// Double Check if _anime string is API Compliant.  ///
                /// ------------------------------------------------ ///
                string anime = Helpers.ConvertToAPIConpliantString(_anime, ' ', '-');

                /// --------------------------------------------------- ///
                /// Once _anime string is API Compliant, begin the GET  ///
                /// --------------------------------------------------- ///

                Debug.WriteLine("LibraryRemove(): Entering");

                // Create a client
                HttpClient httpClient = new HttpClient();

                // Add a new Request Message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Helpers.CreateHummingbirdUrl("libraries/" + anime + "/remove", HummingbirdAPILevel.Version1, true)); //"https://hbrd-v1.p.mashape.com/libraries/" + anime + "/remove");

                // Add our custom headers
                requestMessage.Headers.Add("accept", "application/json"); //"accept"
                requestMessage.Headers.Add("X-Mashape-Authorization", Consts.appData.MashapeKey);

                // Add our Content
                requestMessage.Content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string,string>("auth_token", Consts.LoggedInUser.AuthToken),//Consts.settings.auth_token),
                    });

                // Send the request to the server
                // add await to await response
                Task<HttpResponseMessage> responseTask = httpClient.SendAsync(requestMessage);

                if (true) //response.IsSuccessStatusCode)
                {
                    // Just as an example I'm turning the response into a string here
                    //string responseAsString = await response.Content.ReadAsStringAsync();//.Result;

                    //Debug.WriteLine(responseAsString);

                    Debug.WriteLine("LibraryRemove(): Success!");
                    args = new APICompletedEventArgs(APIResponse.Successful, APIType.PostLibraryRemove);
                    if (fireOffEvent)
                    {
                        APICompletedEventHandler(args.UserState, args);
                    }
                    return args;
                }

                Debug.WriteLine("LibraryRemove(): Failed");
                args = new APICompletedEventArgs(APIResponse.Failed, APIType.PostLibraryRemove);
                if (fireOffEvent)
                {
                    APICompletedEventHandler(args.UserState, args);
                }
                return args;
            }
        }
    }

}
