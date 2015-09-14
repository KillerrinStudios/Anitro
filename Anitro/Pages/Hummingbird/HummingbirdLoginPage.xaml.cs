using Anitro.ViewModels;
using Anitro.ViewModels.Hummingbird;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Anitro.Pages.Hummingbird
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HummingbirdLoginPage : Page
    {
        public HummingbirdLoginViewModel ViewModel { get { return (HummingbirdLoginViewModel)DataContext; } }

        public HummingbirdLoginPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Loaded();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                    PasswordBox.Focus(FocusState.Programmatic);
                else if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
                    UsernameTextBox.Focus(FocusState.Programmatic);
                else
                    ViewModel.Login();
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
