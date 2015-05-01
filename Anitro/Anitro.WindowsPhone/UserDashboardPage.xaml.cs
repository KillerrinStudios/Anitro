using Anitro.APIs;
using Anitro.APIs.Events;
using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.Structures;
using Microsoft.AdMediator.WindowsPhone81;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Anitro
{
    public sealed partial class UserDashboardPage : Page
    {
        private UserDashboardPageParameter pageParameter;

        // Header Template
        private Image cover_Image;
        private Image avatar_Image;
        private TextBlock username_TextBlock;
        private TextBlock userID_TextBlock;

        // General
        private AdMediatorControl adControl;

        // Stats
        private TextBlock waifuHusbando_TextBlock;
        private Image waifu_Image;
        private TextBlock waifuName_TextBlock;
        private Button waifuShow_Button;
        private Image waifuShow_Image;
        private TextBlock waifuShowName_TextBlock;

        private TextBlock total_AnimeWatched_TextBlock;
        private TextBlock genre1_TextBlock;
        private TextBlock genre2_TextBlock;
        private TextBlock genre3_TextBlock;
        private TextBlock genre4_TextBlock;
        private TextBlock genre5_TextBlock;
        private TextBlock num_Genre1_TextBlock;
        private TextBlock num_Genre2_TextBlock;
        private TextBlock num_Genre3_TextBlock;
        private TextBlock num_Genre4_TextBlock;
        private TextBlock num_Genre5_TextBlock;

        // Social
        private ListBox activityFeedBox;
        private TextBox activityTextBox;


        // Loading State Management
        bool libraryLoaded = false;
        bool activityFeedLoaded = false;

        static bool userInfoLoaded = false;
        bool refreshingUserInfo = false;

        public UserDashboardPage()
        {
            this.InitializeComponent();
        }

        private async void UserDashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UserDashboardPage: Begin Loaded");

            if (MainPage.RecentlyLoggedOut ||
               (MainPage.ComingFromPage == PageType.SettingsPage && MainPage.RecentlyLoggedIn))
            {
                Debug.WriteLine("UserDashboardPage: End Loaded");

                GoBack();
                return;
            }
            else if (pageParameter.user.IsLoggedIn)
            {
                if (Consts.forceLibrarySave)
                {
                    Debug.WriteLine("Forcing Save");

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    await Consts.LoggedInUser.Save();

                    Consts.forceLibrarySave = false;
                }
            }
            else { }

            Debug.WriteLine("UserDashboardPage: End Loaded");
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("UserDashboardPage: Begin OnNavigatedTo");
            base.OnNavigatedTo(e);

            if (MainPage.RecentlyLoggedOut ||
               (MainPage.ComingFromPage == PageType.SettingsPage && MainPage.RecentlyLoggedIn)) 
            {
                Debug.WriteLine("UserDashboardPage: Early Leave"); 
                return; 
            }

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler += APIv1_FeedbackEventHandler;

            // Save the parameters
            pageParameter = e.Parameter as UserDashboardPageParameter;

            if (pageParameter.user.IsLoggedIn)
            {
                // Because data could have been changed within another area of the app further ahead, we will do a check which if positive,
                // will rewrite the parameter user with our logged in user
                pageParameter.user = Consts.LoggedInUser;
            }
            else { }

            // Finally, Load the Library
            LoadLibrary();

            Debug.WriteLine("UserDashboardPage: End OnNavigatedTo");
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Consts.isApplicationClosing = true;

            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !Consts.forceLibrarySave)
                {
                    loop = false;
                }
            }

            e.Handled = true;
            GoBack();
        }

        private void GoBack()
        {
            Debug.WriteLine("UserDashboardPage: GoBack()");

            // Describe from event
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            // Exit the application
            if (MainPage.ComingFromPage == PageType.SettingsPage) MainPage.ComingFromPage = PageType.LoggedOutPage;
            else MainPage.ComingFromPage = PageType.UserDashboardPage;

            Frame.GoBack(); //Application.Current.Exit();
        }

        #region AppBar
        private void UserInfo_Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            if (refreshingUserInfo || !libraryLoaded || Consts.forceLibrarySave) { return; }

            LoadUserInfo(true);
        }

        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(SearchPage));
        }
        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(SettingsPage));
        }
        private void About_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(About));
        }

        private async void Review_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            await Consts.appData.LaunchReview();
        }
        #endregion

        #region Ad Mediator
        private void AdMediator_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdMediatorControl);
            XamlControlHelper.AnitroAdMediatorSettings(sender);
        }

        private void AdMediator_AdSdkError(object sender, Microsoft.AdMediator.Core.Events.AdFailedEventArgs e)
        {
            Debug.WriteLine("AdSdkError by {0} ErrorCode: {1} ErrorDescription: {2} Error: {3}", e.Name, e.ErrorCode, e.ErrorDescription, e.Error);
        }

        private void AdMediator_AdSdkEvent(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdSdk event {0} by {1}", e.EventName, e.Name);
        }

        private void AdMediator_AdMediatorFilled(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdFilled:" + e.Name);
        }

        private void AdMediator_AdMediatorError(object sender, Microsoft.AdMediator.Core.Events.AdMediatorFailedEventArgs e)
        {
            Debug.WriteLine("AdMediatorError:" + e.Error + " " + e.ErrorCode);
            // if (e.ErrorCode == AdMediatorErrorCode.NoAdAvailable)
            // AdMediator will not show an ad for this mediation cycle
        }
        #endregion
    }
}
