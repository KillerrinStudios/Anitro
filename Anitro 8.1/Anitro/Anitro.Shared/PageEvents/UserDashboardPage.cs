using Anitro.APIs;
using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Anitro.Data_Structures;
using Anitro.APIs.Events;
using Windows.UI.Xaml.Media.Imaging;

#if WINDOWS_PHONE_APP
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Advertising.Mobile.Common;
#else
using Microsoft.Advertising.WinRT.UI;
//using Microsoft.Advertising.WinRT.Common;
#endif

namespace Anitro
{
    public partial class UserDashboardPage
    {
        private void BindUserInfo()
        {
            try
            {
                cover_Image.Source = new BitmapImage(new Uri(pageParameter.user.UserInfo.cover_image, UriKind.Absolute));
                avatar_Image.Source = new BitmapImage(new Uri(pageParameter.user.UserInfo.avatar, UriKind.Absolute));
            }
            catch (Exception) { }

            try
            {
                username_TextBlock.Text = pageParameter.user.Username;
                userID_TextBlock.Text = "UserID: " + Consts.LoggedInUser.UserInfo.id.ToString();
            }
            catch (Exception) { }
            
            bool hidewifu = false;
            try
            {
                if (String.IsNullOrEmpty(pageParameter.user.UserInfo.waifu))
                {
                    hidewifu = true;
                }
                else
                {
                    // Waifu
                    try {
                        waifuHusbando_TextBlock.Text = pageParameter.user.UserInfo.waifu_or_husbando;
                    }
                    catch (Exception) { }

                    waifu_Image.Source = new BitmapImage(pageParameter.user.UserInfo.GetWaifuPictureURI());
                    waifuName_TextBlock.Text = pageParameter.user.UserInfo.waifu;

                    // Waifu Show
                    // See if the Show exists within the library
                    if (pageParameter.user.animeLibrary.DoesExistInLibrary(LibrarySelection.All, pageParameter.user.UserInfo.waifu_slug)) {
                        var anime = pageParameter.user.animeLibrary.GetObjectInLibrary(LibrarySelection.All, pageParameter.user.UserInfo.waifu_slug);

                        waifuShow_Image.Source = new BitmapImage(anime.Anime.CoverImageUrl);
                        waifuShowName_TextBlock.Text = anime.Anime.RomanjiTitle;

                        // Fix the slug
                        pageParameter.user.UserInfo.waifu_slug = anime.Anime.ServiceID;
                    }
                    else {
                        // Brute Force Search for Shows
                        ObservableCollection<Anitro.Data_Structures.API_Classes.Anime> search = pageParameter.user.animeLibrary.Search(pageParameter.user.UserInfo.waifu_slug);
                        if (search.Count > 0) {
                            waifuShowName_TextBlock.Text = search[0].RomanjiTitle;
                            waifuShow_Image.Source = new BitmapImage(search[0].CoverImageUrl);

                            // Fix the slug
                            pageParameter.user.UserInfo.waifu_slug = search[0].ServiceID;
                        }
                        else {
                            waifuShowName_TextBlock.Text = Helpers.ConvertToAPIConpliantString(pageParameter.user.UserInfo.waifu_slug, '-', ' ');
                        }
                    }
                }
            }
            catch (Exception) { hidewifu = true; }

            // Hide it to remove placeholder data
            if (hidewifu) {
                // ToDo: Fix the crash which happens when the app loads in snapped mode
                // Fix: Solve for Null Refrence Error of Controls
                try {
                    waifuHusbando_TextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    waifu_Image.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    waifuName_TextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    waifuShow_Image.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    waifuShowName_TextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
                catch (Exception) { }
            }

            // ToDo: Fix the crash which happens when the app loads in snapped mode
            // Fix: Solve for Null Refrence Error of Controls
            try {
                total_AnimeWatched_TextBlock.Text = pageParameter.user.UserInfo.anime_watched.ToString();
            }
            catch (Exception) { }

            // Grab the genre to make it easier
            List<Anitro.Data_Structures.API_Classes.TopGenres> genre = pageParameter.user.UserInfo.top_genres;

            // Set the Genre Values
            #region Bind Genre Values
            // ToDo: Fix the crash which happens when the app loads in snapped mode
            // Fix: Solve for Null Refrence Error of Controls
            try {
                genre1_TextBlock.Text = genre[0].genre.ToString();
                num_Genre1_TextBlock.Text = genre[0].num.ToString();
            }
            catch (Exception) {
                //num_Genre1_TextBlock.Text = " ";
            }

            try {
                genre2_TextBlock.Text = genre[1].genre.ToString(); ;
                num_Genre2_TextBlock.Text = genre[1].num.ToString();
            }
            catch (Exception) {
                //num_Genre2_TextBlock.Text = " ";
            }

            try {
                genre3_TextBlock.Text = genre[2].genre.ToString();
                num_Genre3_TextBlock.Text = genre[2].num.ToString();
            }
            catch (Exception) {
                //num_Genre3_TextBlock.Text = " ";
            }

            try {
                genre4_TextBlock.Text = genre[3].genre.ToString();
                num_Genre4_TextBlock.Text = genre[3].num.ToString();
            }
            catch (Exception) {
                //num_Genre4_TextBlock.Text = " ";
            }

            try {
                genre5_TextBlock.Text = genre[4].genre.ToString();
                num_Genre5_TextBlock.Text = genre[4].num.ToString();
            }
            catch (Exception) {
                //num_Genre5_TextBlock.Text = " ";
            }
            #endregion
        }

        #region Control Events
        private void DashboardHub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var section = DashboardHub.SectionsInView[0];
            var tag = section.Tag.ToString();

            switch (tag)
            {
                case "0":
                    refreshUserInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
                case "1":
                    refreshUserInfo.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case "2":
                    refreshUserInfo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
            }
        }
        private void ActivityFeed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!activityFeedLoaded || !libraryLoaded || Consts.forceLibrarySave) return;

            try
            {
                if (((ListBox)sender).SelectedItem == null) { return; }
                Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject selected = ((ListBox)sender).SelectedItem as Anitro.Data_Structures.API_Classes.Hummingbird.V1.ActivityFeedObject;

                ((ListBox)sender).SelectedItem = null;

                if (!string.IsNullOrEmpty(selected.slug) && selected.header != pageParameter.user.Username)//Consts.settings.userName)
                {
#if WINDOWS_PHONE_APP
                    // Remove the Event Handler for a safe transition
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                    APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    AnimePageParameter sendParameter = new AnimePageParameter(selected.slug, AnimePageParameter.ComingFrom.ActivityFeed);
                    Frame.Navigate(typeof(AnimePage), sendParameter);
                }

            }
            catch (Exception) { }
        }
        private void AnimeLibrary_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            //!activityFeedLoaded || 

            // Remove the Event Handler for a safe transition
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Consts.UpdateLoggedInUser(pageParameter.user);
            LibraryPageParameter sendParam = new LibraryPageParameter(pageParameter.user, LibraryType.Anime);
            Frame.Navigate(typeof(LibraryPage), sendParam);
        }
        private void WaifuShow_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || !userInfoLoaded || refreshingUserInfo || Consts.forceLibrarySave) return;
            if (string.IsNullOrEmpty(pageParameter.user.UserInfo.waifu_slug)) return;

#if WINDOWS_PHONE_APP
            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            Consts.UpdateLoggedInUser(pageParameter.user);
            AnimePageParameter sendParameter = new AnimePageParameter(pageParameter.user.UserInfo.waifu_slug, AnimePageParameter.ComingFrom.ActivityFeed);
            Frame.Navigate(typeof(AnimePage), sendParameter);
        }

        private void StatusPostEnterCheck(object sender, KeyRoutedEventArgs e)
        {
            if (!activityFeedLoaded || !libraryLoaded || Consts.forceLibrarySave) return;

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("Enter Pressed, Submitting Status");
                TextBox txtBox = (sender as TextBox);

                // Return early if it is empty
                if (txtBox.Text.Length == 0) return;

                this.Focus(FocusState.Programmatic);

                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
                if (Consts.IsConnectedToInternet())
                {
                    try
                    {
                        APIv1.APICompletedEventHandler += ActivityFeedUpdated;

                        this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            APIv1.Post.StatusUpdate(txtBox.Text);
                        });
                    }
                    catch (Exception) { XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false); }
                }
                else
                {
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                }
            }
        }
        #endregion

        #region Loaded Events
        #region Header Template
        private void Cover_Image_Loaded(object sender, RoutedEventArgs e)
        {
            cover_Image = (sender as Image);
        }

        private void Avatar_Image_Loaded(object sender, RoutedEventArgs e)
        {
            avatar_Image = (sender as Image);
        }

        private void Username_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            username_TextBlock = (sender as TextBlock);
            username_TextBlock.Text = pageParameter.user.Username;
        }

        private void UserID_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            userID_TextBlock = (sender as TextBlock);
        }
        #endregion

        #region General
        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdControl);
            XamlControlHelper.AnitroAdControlSettings(adControl);
        }
        #endregion

        #region Statistics
        private void WaifuShow_Button_Loaded(object sender, RoutedEventArgs e)
        {
            waifuShow_Button = (sender as Button);
        }

        private void WaifuShowName_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            waifuShowName_TextBlock = (sender as TextBlock);
        }

        private void WaifuShow_Image_Loaded(object sender, RoutedEventArgs e)
        {
            waifuShow_Image = (sender as Image);

            waifu_Image.Source = new BitmapImage(pageParameter.user.UserInfo.GetWaifuPictureURI());
        }

        private void WaifuName_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            waifuName_TextBlock = (sender as TextBlock);

            if (!string.IsNullOrWhiteSpace(pageParameter.user.UserInfo.waifu)) {
                waifuName_TextBlock.Text = pageParameter.user.UserInfo.waifu;
            }
        }

        private void Waifu_Image_Loaded(object sender, RoutedEventArgs e)
        {
            waifu_Image = (sender as Image);
        }

        private void WaifuHusbando_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            waifuHusbando_TextBlock = (sender as TextBlock);

            if (!string.IsNullOrWhiteSpace(pageParameter.user.UserInfo.waifu_or_husbando)) {
                try {
                    waifuHusbando_TextBlock.Text = pageParameter.user.UserInfo.waifu_or_husbando;
                }
                catch (Exception) { }
            }
        }

        #region Genres
        private void Total_AnimeWatched_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            total_AnimeWatched_TextBlock = (sender as TextBlock);
        }

        #region Total Watched
        private void Num_Genre1_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            num_Genre1_TextBlock = (sender as TextBlock);
        }

        private void Num_Genre2_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            num_Genre2_TextBlock = (sender as TextBlock);
        }

        private void Num_Genre3_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            num_Genre3_TextBlock = (sender as TextBlock);
        }

        private void Num_Genre4_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            num_Genre4_TextBlock = (sender as TextBlock);
        }

        private void Num_Genre5_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            num_Genre5_TextBlock = (sender as TextBlock);
        }
        #endregion

        #region Names
        private void Genre1_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            genre1_TextBlock = (sender as TextBlock);
        }


        private void Genre2_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            genre2_TextBlock = (sender as TextBlock);
        }

        private void Genre3_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            genre3_TextBlock = (sender as TextBlock);
        }

        private void Genre4_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            genre4_TextBlock = (sender as TextBlock);
        }

        private void Genre5_TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            genre5_TextBlock = (sender as TextBlock);
        }
        #endregion
        #endregion

        private void GenrePieChart_Loaded(object sender, RoutedEventArgs e)
        {
            ///http://stackoverflow.com/questions/24155021/how-to-do-chart-on-windows-phone-universal-app
            //genreChart = (sender as Chart);

            //List<NameValueItem> items = new List<NameValueItem>();
            //items.Add(new NameValueItem { Name = "Test1", Value = Consts.random.Next(10, 100) });
            //items.Add(new NameValueItem { Name = "Test2", Value = Consts.random.Next(10, 100) });
            //items.Add(new NameValueItem { Name = "Test3", Value = Consts.random.Next(10, 100) });
            //items.Add(new NameValueItem { Name = "Test4", Value = Consts.random.Next(10, 100) });
            //items.Add(new NameValueItem { Name = "Test5", Value = Consts.random.Next(10, 100) });

            //((PieSeries)genreChart.Series[0]).ItemsSource = items;
        }
        #endregion

        #region Activity Feed
        private void ActivityFeedLoaded(object sender, RoutedEventArgs e)
        {
            activityFeedBox = (sender as ListBox);
            activityFeedBox.ItemsSource = pageParameter.user.activityFeed;
        }
        private void ActivityTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            activityTextBox = (sender as TextBox);

            if (!pageParameter.user.IsLoggedIn)
            {
                activityTextBox.IsEnabled = false;
            }
        }
        #endregion
        #endregion

        private async void LoadLibrary()
        {
            // This is why its not auto-regenerating
            if (pageParameter.user.animeLibrary.IsEveryLibraryEmpty(LibrarySelection.All))
            {
                if (Consts.IsConnectedToInternet())
                {
                    Debug.WriteLine("LoadLibrary(): Loading Library");

                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
                    try
                    {
                        pageParameter.user.animeLibrary.LibraryLoadedEventHandler += LibraryLoaded;

                        this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            pageParameter.user.animeLibrary.LoadFromStorageOrServer(pageParameter.user, LibraryType.Anime, Consts.justSignedIn);
                        });
                    }
                    catch (Exception)
                    {
                        pageParameter.user.animeLibrary.LibraryLoadedEventHandler -= LibraryLoaded;
                        XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                        libraryLoaded = true;
                        LoadActivityFeed();
                    }
                }
                else
                {
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    libraryLoaded = true;
                    activityFeedLoaded = true;
                    userInfoLoaded = true;

                    // Since the network is out, Just jump to binding the user info
                    BindUserInfo();
                }
            }
            else
            {
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                libraryLoaded = true;
                LoadActivityFeed();
            }
        }
        private async void LoadActivityFeed(bool forceRefresh = false)
        {
            // Check if the feed is already loaded
            if (pageParameter.user.activityFeed.Count > 0)
            {
                if (!forceRefresh)
                {

                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    activityFeedLoaded = true;

                    LoadUserInfo();
                    return;
                }
            }

            Debug.WriteLine("LoadActivityFeed(): Loading Library");

            if (Consts.IsConnectedToInternet())
            {
                try
                {
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

                    APIv1.APICompletedEventHandler += ActivityFeedUpdated;
                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        APIv1.Get.StatusFeed(pageParameter.user);
                    });
                }
                catch (Exception)
                {
                    APIv1.APICompletedEventHandler -= ActivityFeedUpdated;

                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                    activityFeedLoaded = true;
                    LoadUserInfo();
                }
            }
            else
            {
                activityFeedLoaded = true;
                userInfoLoaded = true;

                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                BindUserInfo();
            }
        }
        private async void LoadUserInfo(bool forceRefresh = false)
        {
            if (refreshingUserInfo) { return; }
            if (pageParameter.user.IsLoggedIn)
            {
                if (userInfoLoaded)
                {
                    if (!forceRefresh)
                    {
                        BindUserInfo();
                        return;
                    }
                }
            }

            if (Consts.IsConnectedToInternet())
            {
                Debug.WriteLine("LoadUserInfo(): Beginning");

                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
                try
                {
                    refreshingUserInfo = true;

                    APIv1.APICompletedEventHandler += RefreshingUserInfo_Completed;

                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        APIv1.Get.Streamlining.AllUserInfo(pageParameter.user.Username);
                    });
                }
                catch (Exception)
                {
                    APIv1.APICompletedEventHandler -= RefreshingUserInfo_Completed;
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

                    refreshingUserInfo = false;
                    userInfoLoaded = true;
                    BindUserInfo();
                }
            }
            else
            {
                userInfoLoaded = true;
                BindUserInfo();
            }
        }

        #region Custom Events
        private void LibraryLoaded(object sender, Anitro.APIs.Events.LibraryLoadedEventArgs e)
        {
            if (e.Type != Data_Structures.APIType.LoadLibrary) return;
            Debug.WriteLine("LibraryLoaded(): MainPage Loaded");

            switch (e.Result)
            {
                case APIResponse.Successful:
                    Debug.WriteLine("LibraryLoaded(): Successful!");
                    pageParameter.user = new User((e.ResultObject as User));
                    break;

                case APIResponse.Failed:
                case APIResponse.NetworkError:
                    Debug.WriteLine("LibraryLoaded(): Failed");
                    break;
            }

            //for (int i = 0; i < 9; i++)
            //{
            //    bool s = false;
            //    if (i == 8) s = true;
            //    Consts.LoggedInUser.animeLibrary.AddToRecent(Consts.LoggedInUser.animeLibrary.CurrentlyWatching[i], s);
            //}

            // Remove the Event Handler
            pageParameter.user.animeLibrary.LibraryLoadedEventHandler -= LibraryLoaded;
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
            libraryLoaded = true;

            LoadActivityFeed();
        }
        private void ActivityFeedUpdated(object sender, Anitro.APIs.Events.APICompletedEventArgs e)
        {
            switch (e.Type)
            {
                case Anitro.Data_Structures.APIType.GetActivityFeed:
                case Anitro.Data_Structures.APIType.PostActivityFeed:
                    break;
                default:
                    Debug.WriteLine("ActivityFeedUpdated(): Not GetActivityFeed or PostActvityFeed");
                    return;
            }

            bool currentlyOnPage = true;
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

            switch (e.Result)
            {
                case Anitro.Data_Structures.APIResponse.Successful:
                    Debug.WriteLine("ActivityFeedUpdated(): Status-Feed Updated Successfully!");

                    try
                    {
                        pageParameter.user.activityFeed = (e.ResultObject as User).activityFeed;
                        pageParameter.user.AvatarURL = (e.ResultObject as User).AvatarURL;
                        activityFeedBox.ItemsSource = pageParameter.user.activityFeed;
                    }
                    catch (Exception) { currentlyOnPage = false; }
                    break;
                case Anitro.Data_Structures.APIResponse.Failed:
                    Debug.WriteLine("ActivityFeedUpdated(): Status Feed Failed");
                    break;
            }

            // Remove the Event Handler
            APIv1.APICompletedEventHandler -= ActivityFeedUpdated;

            // Since we know we are currently on another page while this event is being called
            // we will exit before the second check to stop the application from
            // temporarily freezing as it grabs the stack
            if (!currentlyOnPage) return;

            try
            {
                // Turn off the progress bar
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

                // Reset the box
                activityFeedLoaded = true;
                activityTextBox.Text = "";
            }
            catch (Exception) { }

            LoadUserInfo();
        }
        private async void RefreshingUserInfo_Completed(object sender, APICompletedEventArgs e)
        {
            if (e.Type != APIType.UserInfo) { return; }

            Debug.WriteLine("Getting UserInfo");

            switch (e.Result)
            {
                case APIResponse.Successful:
                    Debug.WriteLine("Login(): UserInfo Recieved Successfully!");
                    pageParameter.user.UserInfo = (e.ResultObject as Data_Structures.API_Classes.UserInfo);

                    if (pageParameter.user.IsLoggedIn)
                    {
                        Consts.UpdateLoggedInUser(pageParameter.user);
                        await Consts.LoggedInUser.Save();
                    }
                    break;
                default:
                    Debug.WriteLine("Login(): UserInfo Failed");
                    break;
            }

            Debug.WriteLine("Exiting: Post.PostLogin()");

            // Remove the Event Handler
            APIv1.APICompletedEventHandler -= RefreshingUserInfo_Completed;

            // Shut off the Progress Bar
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

            // Allow us to leave
            refreshingUserInfo = false;
            userInfoLoaded = true;

            // Reset the bindings for the content
            BindUserInfo();
        }
        private void APIv1_FeedbackEventHandler(object sender, APIs.Events.APIFeedbackEventArgs e)
        {

        }
        #endregion

        private void AdControl_ErrorOccured(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.Error + " ErrorCode: " + e.ErrorCode.ToString());
        }
    }
}
