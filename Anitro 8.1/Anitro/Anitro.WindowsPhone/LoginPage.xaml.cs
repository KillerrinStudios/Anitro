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

using System.Diagnostics;
using Anitro.APIs;
using Anitro.Data_Structures;
using System.Threading.Tasks;
using Anitro.APIs.Events;
using Windows.UI.Popups;
using Anitro.APIs.Hummingbird;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public bool postingLogin = false;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler += APIv1_FeedbackEventHandler;

            if (DebugTools.DebugMode)
            {
                usernameBox.Text = DebugTools.testAccountUsername;
                passwordBox.Password = DebugTools.testAccountPassword;
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
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

            e.Handled = true;

            GoBack();
        }
        private void GoBack()
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Frame.GoBack();
        }


        private void LoginEnterEvent(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("Enter Pressed");
                Frame.Focus(Windows.UI.Xaml.FocusState.Programmatic);

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

                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        APIv1.Post.Login(usernameBox.Text, passwordBox.Password);
                    });

                    //await Task.Run(() => APIv1.Post.Login(usernameBox.Text, passwordBox.Text));
                    //Task responseTask = APIv1.Post.Login(usernameBox.Text, passwordBox.Text);

                }
                catch (Exception) { }
            }

            switch (response)
            {
                case APIResponse.InfoNotEntered:
                    Debug.WriteLine("Username of Password not Entered");
                    loginErrors.Text = "Please type in your username/password";
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    break;
                case APIResponse.NetworkError:
                    Debug.WriteLine("Network Error");
                    loginErrors.Text = "Error Connecting to internet";
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    break;
            }
        }

        void APIv1_FeedbackEventHandler(object sender, APIFeedbackEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.FeedbackMessage)) return;
            
            loginErrors.Text = e.FeedbackMessage;
        }

        async void LoginCompleted(object sender, APICompletedEventArgs e)
        {
            if (e.Type != APIType.Login) { return; }

            Debug.WriteLine("Login Completed");

            bool goBackToMainPage = false;

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

                GoBack();
            }
        }

        private async void facebookExplanation_Clicked(object sender, RoutedEventArgs e)
        {
            string bodyText1 = "As the Hummingbird API doesn't yet support signing in through Facebook, you will need to sign in using your Username and Password. ";
            bodyText1 += "\n\n If you have never signed in through your username and password before, you may need to follow these next steps.";

            string bodyText2 = "First go to the Account settings page on the Hummingbird website http://hummingbird.me/users/edit";
            bodyText2 += "\n\n From there, their is a field to set your password, so set that. Your username is whatever shows under \"Name\". Once that is set, come back here and sign in";

            MessageDialog md = new MessageDialog(bodyText1, "Facebook Support");
            bool? result = null;
            md.Commands.Add(
               new UICommand("Next", new UICommandInvokedHandler((cmd) => result = true)));

            await md.ShowAsync();

            if (result == true)
            {
                MessageDialog md2 = new MessageDialog(bodyText2, "How to sign in");
                bool? result2 = null;
                md.Commands.Add(
                   new UICommand("Finish!", new UICommandInvokedHandler((cmd) => result2 = true)));

                await md2.ShowAsync(); 
            }
        }
    }
}
