using Anitro.APIs;
using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.Structures;
using BugSense;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static PageType ComingFromPage = PageType.None;
        public static bool RecentlyLoggedIn = false;
        public static bool RecentlyLoggedOut = false;

        public MainPage()
        {
            Debug.WriteLine("MainPage: Begin Constructor");
            this.InitializeComponent();

            Debug.WriteLine("MainPage: End Constructor");
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MainPage: Begin Loaded");
            try
            {
                if (Consts.LoggedInUser.IsLoggedIn)
                {
                    if (Consts.forceLibrarySave)
                    {
                        Debug.WriteLine("Forcing Save");
                        await Consts.LoggedInUser.Save();
                        Consts.forceLibrarySave = false;
                    }
                }
                else
                {
                }
            }
            catch (Exception) { }

            if (ComingFromPage == PageType.UserDashboardPage)
            {
                Debug.WriteLine("Coming From: UserDashboardPage");
                if (RecentlyLoggedOut)
                {
                    LoggedOutView();
                }
                else Application.Current.Exit();
            }
            else if (ComingFromPage == PageType.LoggedOutPage)
            {
                Debug.WriteLine("Coming From: LoggedOutPage");
                if (RecentlyLoggedIn)
                {
                    LoggedInView();
                }
                else Application.Current.Exit();
            }
            else if (ComingFromPage == PageType.None) { }
            else { }

            Debug.WriteLine("MainPage: End Loaded");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("MainPage: Begin OnNavigatedTo");
            base.OnNavigatedTo(e);

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            if (ComingFromPage == PageType.None)
            {
                Consts.AppSettings = await Anitro.Data_Structures.Settings.Load();

                // Login and proceed to app launch
                if (!Consts.LoggedInUser.IsLoggedIn)
                    Consts.LoggedInUser = await Anitro.Data_Structures.User.Load();

                await InAppPurchaseHelper.CheckProductInformation();

                // Check for Args
                //-- Test Args
                //string fPam = "type=" + "Anime" + "&" +
                //               "args=" + "steins-gate" + "&" +
                //               "status=" + "uriAssociation";

                // If there is a parameter then continue the navigation to the appropriate page.
                if (!Consts.openedFromProtocolOrTile && !String.IsNullOrWhiteSpace(e.Parameter.ToString()))
                {
                    XamlControlHelper.SetDebugString(debugTextBlock, "Parameter Found");

                    AnitroLaunchArgs launchArgs = (e.Parameter as AnitroLaunchArgs);
                    if (launchArgs == null) { launchArgs = new AnitroLaunchArgs(e.Parameter.ToString()); }

                    XamlControlHelper.SetDebugString(debugTextBlock, "Launch Args Parsed");

                    if (DebugTools.DebugMode) {
                        if (launchArgs != null) {
                            Debug.WriteLine("Application was activated from a Secondary Tile or from URI Association with the following Activation Arguments : " + launchArgs.ToString());
                        }
                    }

                    // Navigate
                    OpenedFromTileOrProtocol(launchArgs);
                }
                // Lastly, If nothing else, Just open normally.
                else
                {
                    // Install the Voice Commands
                    VoiceCommandHelper.InstallVoiceCommands();

                    // Proceed to launch
                    Debug.WriteLine("Setting Hub View");
                    if (Consts.LoggedInUser.IsLoggedIn)
                    {
                        LoggedInView();
                    }
                    else
                    {
                        LoggedOutView();
                    }
                }
            }
            else if (ComingFromPage == PageType.UserDashboardPage) { }
            else if (ComingFromPage == PageType.LoggedOutPage) { }

            Debug.WriteLine("MainPage: End OnNavigatedTo");
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Consts.isApplicationClosing = true;

            bool loop = true;
            while (loop) { 
                if (APIs.StorageTools.isSavingComplete &&
                    !Consts.forceLibrarySave)
                {
                    loop = false; 
                }
            }

            GoBack();
        }

        private void GoBack()
        {
            // De-subscribe from event
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            // Exit the application
            Application.Current.Exit();
        }
    }
}
