using Anitro.Data_Structures.Structures;
using BugSense;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Anitro
{
    public partial class MainPage
    {
        private void OpenedFromTileOrProtocol(AnitroLaunchArgs args)
        {
            Consts.openedFromProtocolOrTile = true;

            XamlControlHelper.SetDebugString(debugTextBlock, args.ToString());

#if WINDOWS_PHONE_APP
            // Remove the Back Button Event, because we are out of here!
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif

            // BLAST OFF!
            switch (args.type)
            {
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.Anime:
                    XamlControlHelper.SetDebugString(debugTextBlock, "Parameter identified as Anime");

                    AnimePageParameter animePageParam = new AnimePageParameter(args.args, AnimePageParameter.ComingFrom.Tile);
                    Debug.WriteLine("OpenedFromTileOrProtocol(): Anime: " + animePageParam.ToString());

                    XamlControlHelper.SetDebugString(debugTextBlock, animePageParam.ToString());

                    Frame.Navigate(typeof(AnimePage), animePageParam);
                    break;
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.Search:
                    XamlControlHelper.SetDebugString(debugTextBlock, "Parameter identified as Search");

                    SearchPageParameter searchPageParam = new SearchPageParameter(args.args);
                    Debug.WriteLine("OpenedFromTileOrProtocol(): Search: " + searchPageParam.ToString());

                    XamlControlHelper.SetDebugString(debugTextBlock, searchPageParam.ToString());

                    try
                    {
                        Frame.Navigate(typeof(SearchPage), searchPageParam);
                    }
                    catch (Exception ex) { XamlControlHelper.SetDebugString(debugTextBlock, DebugTools.PrintOutException("", ex)); }
                    break;

                case Data_Structures.Enumerators.AnitroLaunchType.Failed:
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.Manga:
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.Page:
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.Url:
                case Anitro.Data_Structures.Enumerators.AnitroLaunchType.None:
                default:
#if WINDOWS_PHONE_APP
                    // Well, we failed. Add it back
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif
                    if (Consts.LoggedInUser.IsLoggedIn)
                    {
                        LoggedInView();
                    }
                    else
                    {
                        LoggedOutView();
                    }

                    //if (!DebugTools.DebugMode) {
                    //    Application.Current.Exit();
                    //}
                    break;
            }
        }

        private void LoggedInView()
        {
            Debug.WriteLine("LoggedInView");
            RecentlyLoggedIn = false;

#if WINDOWS_PHONE_APP
            // Remove the Back Button
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif

            try
            {
                // Set the BugSense Username for tracking
                BugSenseHandler.Instance.UserIdentifier = Consts.LoggedInUser.Username;

                UserDashboardPageParameter sendParam = new UserDashboardPageParameter(Consts.LoggedInUser);
                Frame.Navigate(typeof(UserDashboardPage), sendParam);
            }
            catch (Exception) { }
        }

        private void LoggedOutView()
        {
            Debug.WriteLine("LoggedOutView");
            RecentlyLoggedOut = false;

#if WINDOWS_PHONE_APP
            // Remove the Back Button
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif

            Frame.Navigate(typeof(LoggedOutPage));
        }
    }
}
