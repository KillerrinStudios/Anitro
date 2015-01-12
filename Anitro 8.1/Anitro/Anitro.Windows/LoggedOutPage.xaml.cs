using Microsoft.Advertising.WinRT.UI;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoggedOutPage : Page
    {
        private AdControl adControl;

        public LoggedOutPage()
        {
            this.InitializeComponent();
        }

        private void LoggedOutPage_Loaded(object sender, RoutedEventArgs e)
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoginSettingsPane.LoggedInEventHandler += LoginSettingsPane_LoggedInEventHandler;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
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

            GoBack();
        }

        public void GoBack()
        {
            LoginSettingsPane.LoggedInEventHandler -= LoginSettingsPane_LoggedInEventHandler;

            Frame.GoBack();
        }

        private void LoginSettingsPane_LoggedInEventHandler(object sender, APIs.Events.LoggedInEventArgs e)
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var newFlyout = new LoginSettingsPane();
            newFlyout.ShowIndependent();
        }

        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            var newFlyout = new OptionsSettingsPane();
            newFlyout.ShowIndependent();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var newFlyout = new AboutSettingsPane();
            newFlyout.ShowIndependent();
        }

        private async void Review_Clicked(object sender, RoutedEventArgs e)
        {
            await Consts.appData.LaunchReview();
        }

        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;

            Frame.Navigate(typeof(SearchPage));
        }

        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdControl);
            XamlControlHelper.AnitroAdControlSettings(adControl);
        }

        private void AdControl_ErrorOccured(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.Error + " ErrorCode: " + e.ErrorCode.ToString());
        }
    }
}
