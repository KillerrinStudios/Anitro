using Anitro.APIs;
using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using Microsoft.AdMediator;
using Microsoft.AdMediator.WindowsPhone81;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoggedOutPage : Page
    {
        AdMediatorControl adMediator;

        public LoggedOutPage()
        {
            this.InitializeComponent();
        }

        void LoggedOutPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Consts.LoggedInUser.IsLoggedIn)
                {
                    GoBack();
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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
            // Describe from event
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            MainPage.ComingFromPage = PageType.LoggedOutPage;
            Frame.GoBack();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Frame.Navigate(typeof(LoginPage));
        }

        #region AppBar
        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Frame.Navigate(typeof(SearchPage));
        }
        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Frame.Navigate(typeof(SettingsPage));
        }
        private void About_Clicked(object sender, RoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Frame.Navigate(typeof(About));
        }

        private async void Review_Clicked(object sender, RoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            await Consts.appData.LaunchReview();
        }
        #endregion

        #region Ad Mediator
        private void AdMediator_Loaded(object sender, RoutedEventArgs e)
        {
            adMediator = (sender as AdMediatorControl);
            XamlControlHelper.AnitroAdMediatorSettings(adMediator);
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
