using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using Anitro.ViewModels.Hummingbird;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Anitro.Pages.Hummingbird
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HummingbirdDashboardPage : Page
    {
        public HummingbirdDashboardViewModel ViewModel { get { return (HummingbirdDashboardViewModel)DataContext; } }

        public HummingbirdDashboardPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.User = e.Parameter as Models.HummingbirdUser;
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        private void SocialListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AActivityFeedItem item = ((ListBox)sender).SelectedItem as AActivityFeedItem;
            if (item is ActivityFeedMediaUpdate)
            {
                ActivityFeedMediaUpdate mediaUpdateItem = ((ActivityFeedMediaUpdate)item);
                ViewModel.GetAnimeAndNavigate(mediaUpdateItem.MediaID);
            }
        }

        private void socialFeedTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ViewModel.PostToActivityFeed(textBox.Text);
            }
        }
    }
}
