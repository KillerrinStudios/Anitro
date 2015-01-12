using System;
using System.Collections.Generic;
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

using Windows.ApplicationModel.Email;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : Page
    {
        private AnitroApplicationData appData;

        public About()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            LoadAppData();
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete)
                {
                    loop = false;
                }
            }

            e.Handled = true;
            GoBack();
        }

        private void GoBack()
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            Frame.GoBack();
        }

        void LoadAppData()
        {
            appData = Consts.appData;

            // App Data
            Name.Text = appData.Name;
            Version.Text = appData.Version;
            Description.Text = appData.Description;

            // Contact
            KillerrinTwitter.NavigateUri = new Uri(appData.Twitter);

            EmailFeedback.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SupportEmail.Visibility = Windows.UI.Xaml.Visibility.Visible;


            // Links
            KillerrinWebsite.NavigateUri = new Uri(appData.Website);
            OtherWebsite.NavigateUri = new Uri(appData.OtherWebsite);
        }

        private async void EmailFeedbackButton(object sender, TappedRoutedEventArgs e)
        {
            //predefine Recipient
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = appData.FeedbackUrl
            };

            //generate mail object
            EmailMessage mail = new EmailMessage();
            mail.Subject = appData.FeedbackSubject;
            mail.Body = "";

            //add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            //open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }

        private async void EmailSupportButton(object sender, TappedRoutedEventArgs e)
        {
            //predefine Recipient
            EmailRecipient sendTo = new EmailRecipient()
            {
                Address = appData.SupportUrl
            };

            //generate mail object
            EmailMessage mail = new EmailMessage();
            mail.Subject = appData.SupportSubject;
            mail.Body = "";

            //add recipients to the mail object
            mail.To.Add(sendTo);
            //mail.Bcc.Add(sendTo);
            //mail.CC.Add(sendTo);

            //open the share contract with Mail only:
            await EmailManager.ShowComposeNewEmailAsync(mail);
        }
    }
}
