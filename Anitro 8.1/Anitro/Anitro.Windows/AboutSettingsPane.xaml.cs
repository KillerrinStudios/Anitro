using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Anitro
{
    public sealed partial class AboutSettingsPane : SettingsFlyout
    {
        private AnitroApplicationData appData;
        public AboutSettingsPane()
        {
            this.InitializeComponent();

            LoadAppData();
        }

        private void SettingsFlyout_BackClick(object sender, BackClickEventArgs e)
        {
            // Go back to the main settings
            var mainSettings = new OptionsSettingsPane();
            mainSettings.Show();
        }

        void LoadAppData()
        {
            appData = Consts.appData;

            // App Data
            Name.Text = appData.Name;
            Version.Text = appData.Version;
            Description.Text = appData.Description;

            //// Contact
            KillerrinTwitter.NavigateUri = new Uri(appData.Twitter);

            EmailFeedback.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SupportEmail.Visibility = Windows.UI.Xaml.Visibility.Visible;

            //// Links
            KillerrinWebsite.NavigateUri = new Uri(appData.Website);
            OtherWebsite.NavigateUri = new Uri(appData.OtherWebsite);
        }

        private async void EmailFeedbackButton(object sender, TappedRoutedEventArgs e)
        {
            ////predefine Recipient
            await Launcher.LaunchUriAsync(new Uri("mailto:" + appData.FeedbackUrl + "?subject=" + appData.FeedbackSubject));
            //Launcher.LaunchUriAsync(new Uri("mailto:windows8devs@almostbeta.com?subject=Code Request&cc=kevin@almostbeta.com&bcc=admin@almostbeta.com&body=Hi!"));
        }

        private async void EmailSupportButton(object sender, TappedRoutedEventArgs e)
        {
            ////predefine Recipient
            await Launcher.LaunchUriAsync(new Uri("mailto:" + appData.SupportUrl + "?subject=" + appData.SupportSubject));
        }
    }
}
