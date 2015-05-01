using Anitro.APIs;
using Anitro.APIs.Events;
using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures;
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

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Anitro
{
    public sealed partial class LoginSettingsPane : SettingsFlyout
    {
        public static event LoggedInEventHandler LoggedInEventHandler;
        public static string cachedUsername = "";

        public bool postingLogin = false;

        public LoginSettingsPane()
        {
            this.InitializeComponent();

            if (DebugTools.DebugMode)
            {
                usernameBox.Text = DebugTools.testAccountUsername;
                passwordBox.Password = DebugTools.testAccountPassword;
            }
            else
            {
                usernameBox.Text = cachedUsername;
            }
        }

        private void SettingsFlyout_BackClick(object sender, BackClickEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !postingLogin)
                {
                    loop = false;
                }
            }

            GoBack();
        }

        private void GoBack()
        {
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;
            cachedUsername = "";

            // Go back to the main settings
            var mainSettings = new OptionsSettingsPane();
            mainSettings.Show();
        }

        private void LoginEnterEvent(object sender, KeyRoutedEventArgs e)
        {
            cachedUsername = usernameBox.Text;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("Enter Pressed");
                //this.Focus(Windows.UI.Xaml.FocusState.Programmatic);

                Login();
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private async void Login()
        {
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
            Debug.WriteLine("Clicking Login Button");

            APIResponse response = APIResponse.None;

            if (usernameBox.Text == "" || passwordBox.Password == "") { response = APIResponse.InfoNotEntered; }
            else if (!Consts.IsConnectedToInternet()) { response = APIResponse.NetworkError; }
            else
            {
                try
                {
                    Debug.WriteLine("Getting Login Result");
                    loginErrors.Text = "Attempting to Login..";
                    postingLogin = true;

                    APIv1.APICompletedEventHandler += LoginCompleted;
                    APIv1.FeedbackEventHandler += APIv1_FeedbackEventHandler;

                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        cachedUsername = "";
                        APIv1.Post.Login(usernameBox.Text, passwordBox.Password);
                    });

                    //await Task.Run(() => HummingbirdAPI.V1API.Post.Login(usernameBox.Text, passwordBox.Text));
                    //Task responseTask = HummingbirdAPI.V1API.Post.Login(usernameBox.Text, passwordBox.Text);

                }
                catch (Exception) { }
            }

            switch (response)
            {
                case APIResponse.InfoNotEntered:
                    Debug.WriteLine("Username of Password not Entered");
                    loginErrors.Text = "Please type in your username/password";
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar,false);
                    break;
                case APIResponse.NetworkError:
                    Debug.WriteLine("Network Error");
                    loginErrors.Text = "Error Connecting to internet";
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    break;
            }
        }

        private void APIv1_FeedbackEventHandler(object sender, APIFeedbackEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(e.FeedbackMessage)) return;
                loginErrors.Text = e.FeedbackMessage;
            }
            catch (Exception) { }
        }

        async void LoginCompleted(object sender, APICompletedEventArgs e)
        {
            if (e.Type != APIType.Login) { return; }

            Debug.WriteLine("Login Completed");
            bool goBackToMainPage = false;

            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            try
            {
                switch (e.Result)
                {
                    case APIResponse.Successful:
                        Debug.WriteLine(Consts.LoggedInUser.AuthToken + " | " + Consts.LoggedInUser.Username);

                        // Reset the textboxes
                        usernameBox.Text = ""; //UsernameTB.Text = "username     ";
                        passwordBox.Password = ""; //PasswordTB.Text = "password     ";

                        Debug.WriteLine("Success!");
                        loginErrors.Text = "Success!";

                        goBackToMainPage = true;
                        break;
                    case APIResponse.NotSupported:
                        Debug.WriteLine("Email not Supported");
                        loginErrors.Text = "Logging in through Email is not supported yet. Please use your Username";
                        break;
                    case APIResponse.InfoNotEntered:
                        Debug.WriteLine("Username of Password not Entered");
                        loginErrors.Text = "Please type in your username/password";
                        break;

                    case APIResponse.InvalidCredentials:
                        Debug.WriteLine("Invalid Login Credidentials");
                        loginErrors.Text = "Invalid Login Credidentials";
                        break;
                    case APIResponse.ServerError:
                        Debug.WriteLine("Error connecting to hummingbird.me");
                        loginErrors.Text = "Error connecting to hummingbird.me";
                        break;
                    case APIResponse.NetworkError:
                        Debug.WriteLine("Network Error");
                        loginErrors.Text = "Error Connecting to internet";
                        break;
                    case APIResponse.UnknownError:
                    default:
                        Debug.WriteLine("An Unknown Error has Occured");
                        loginErrors.Text = "An Unknown Error has Occured";
                        break;
                }

                // Remove the Event Handler
                APIv1.APICompletedEventHandler -= LoginCompleted;

                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                postingLogin = false;
                Debug.WriteLine("Login Result Posted\n\n");

                if (goBackToMainPage)
                {
                    await Consts.LoggedInUser.Save();
                    MainPage.RecentlyLoggedIn = true;
                    MainPage.RecentlyLoggedOut = false;

                    if (LoggedInEventHandler != null)
                    {
                        LoggedInEventHandler(null, new LoggedInEventArgs(e.Result, APIType.Login));
                        LoggedInEventHandler = null;
                    }

                    GoBack();
                }
            }
            catch (Exception) { GoBack(); }
        }
    }
}
