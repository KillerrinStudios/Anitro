using Anitro.APIs;
using Anitro.APIs.Events;
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
// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Anitro
{
    public sealed partial class OptionsSettingsPane : SettingsFlyout
    {
        public static event LoggedInEventHandler LoggedOutEventHandler; 

        bool loggedIn = false;

        // Used to determine when we are loging out so we can freeze and potential page navigation
        bool loggingOut = false;

        public OptionsSettingsPane()
        {
            this.InitializeComponent();

            // Set Text
            if (Consts.LoggedInUser.IsLoggedIn) { LoggedInSet(); }
            else { LoggedOutSet(); }

        }

        #region Events
        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            var newFlyout = new AboutSettingsPane();
            newFlyout.ShowIndependent();
        }

        private async void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Consts.LoggedInUser.IsLoggedIn)
            {
                await StorageTools.DeleteFile(StorageTools.StorageConsts.UserFile, Windows.Storage.StorageDeleteOption.PermanentDelete);
                Consts.LoggedInUser = new Data_Structures.User();

                if (LoggedOutEventHandler != null)
                {
                    LoggedOutEventHandler(null, new LoggedInEventArgs(Data_Structures.APIResponse.Successful, Data_Structures.APIType.Login));
                }

                LoggedOutSet();
                MainPage.RecentlyLoggedOut = true;
                MainPage.RecentlyLoggedIn = false;

                await CloseApplicationMessageBox();

                try
                {
                    (Window.Current.Content as Frame).GoBack();
                }
                catch (Exception) { }
            }
            else
            {
                var newFlyout = new LoginSettingsPane();
                newFlyout.ShowIndependent();
            }
        }
        #endregion

        private void LoggedInSet()
        {
            loggedIn = true;
            logoutText.Text = "You are currently signed into";
            logoutUsernameText.Text = Consts.LoggedInUser.Username;
            logoutButton.Content = "logout";
        }
        private void LoggedOutSet()
        {
            loggedIn = false;
            logoutText.Text = "You are currently logged out";
            logoutUsernameText.Text = "";
            logoutButton.Content = "login";
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
    }
}
