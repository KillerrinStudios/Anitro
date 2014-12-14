using Anitro.APIs;
using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class SettingsPage : Page
    {
        bool loggedIn = false;

        // Used to determine when we are loging out so we can freeze and potential page navigation
        bool loggingOut = false;

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Set Text
            if (Consts.LoggedInUser.IsLoggedIn) {

                LoggedInSet();
            }
            else
            {
                LoggedOutSet();
            }

            SetUnlockView();
            if (DebugTools.DebugMode)
            {
            }

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void LoggedInSet()
        {
            Debug.WriteLine("LoggedInSet()");

            loggedIn = true;
            logoutUsernameText.Text = Consts.LoggedInUser.Username;
            logoutButton.Content = "logout";
        }
        private void LoggedOutSet()
        {
            Debug.WriteLine("LoggedOutSet()");

            loggedIn = false;
            logoutText.Text = "you are currently logged out";
            logoutUsernameText.Text = "";
            logoutButton.Content = "login";
        }
        private void SetUnlockView()
        {
            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                Debug.WriteLine("Product Owned, Setting View");

                unlockAnitro_Button.IsEnabled = false;
                purchaseThankYouMessage.Visibility = Windows.UI.Xaml.Visibility.Visible;
                purchasedText.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !loggingOut)
                {
                    loop = false;
                }
            }

            e.Handled = true;

            GoBack();
        }

        private async void GoBack()
        {
            Debug.WriteLine("SettingsPage: GoBack()");

            if (MainPage.RecentlyLoggedIn)
            {
                MainPage.ComingFromPage = PageType.SettingsPage;
                await CloseApplicationMessageBox();
            }
            else if (MainPage.RecentlyLoggedOut)
            {
                await CloseApplicationMessageBox();
            }

            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            Frame.GoBack();
        }

        private async Task CloseApplicationMessageBox()
        {
            Debug.WriteLine("CloseApplicationMessageBox(): User has Logged Out");

            var messageDialog = new MessageDialog("Anitro will now close to apply account changes. Please re-open to login with your new account");
            messageDialog.Commands.Add(new UICommand("Ok", delegate(IUICommand command)
            {
                Application.Current.Exit();
            }));

            messageDialog.DefaultCommandIndex = 0; // Set the command that will be invoked by default
            messageDialog.CancelCommandIndex = 0; // Set the command to be invoked when escape is pressed

            await messageDialog.ShowAsync();
        }

        private async void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Consts.LoggedInUser.IsLoggedIn) {
                loggingOut = true;

                Debug.WriteLine("Logout Button Pressed");

                await StorageTools.DeleteFile(StorageTools.StorageConsts.UserFile, Windows.Storage.StorageDeleteOption.PermanentDelete);
                Consts.LoggedInUser = new Anitro.Data_Structures.User();

                LoggedOutSet();
                MainPage.RecentlyLoggedOut = true;
                MainPage.RecentlyLoggedIn = false;

                loggingOut = false;
            }
            else
            {
                Debug.WriteLine("Login Button Pressed");
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
                Frame.Navigate(typeof(LoginPage));
            }
        }

        private async void openAnitroCompanionButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Launching Anitro Lockscreen");
            var uri = Consts.CreateAnitroCompanionUri("");

            Debug.WriteLine("Launching Uri");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri, Consts.AnitroCompanionLauncherOptions);
        }

        private async void UnlockAnitro_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked) return;
            Debug.WriteLine("UnlockAnitro_Button_Tapped()");

            InAppPurchaseHelper.PurchaseAnitroUnlock();

            SetUnlockView();
        }
    }
}
