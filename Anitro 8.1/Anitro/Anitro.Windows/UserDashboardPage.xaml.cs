using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.Structures;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class UserDashboardPage : Page
    {
        private UserDashboardPageParameter pageParameter;

        // Header Template
        private Image cover_Image;
        private Image avatar_Image;
        private TextBlock username_TextBlock;
        private TextBlock userID_TextBlock;
        private SearchBox searchBox;
        private ProgressRing ApplicationProgressBar;

        // General
        private AdControl adControl;

        // Stats
        private TextBlock waifuHusbando_TextBlock;
        private Image waifu_Image;
        private TextBlock waifuName_TextBlock;
        private Button waifuShow_Button;
        private Image waifuShow_Image;
        private TextBlock waifuShowName_TextBlock;

        private TextBlock total_AnimeWatched_TextBlock;
        private TextBlock genre1_TextBlock;
        private TextBlock genre2_TextBlock;
        private TextBlock genre3_TextBlock;
        private TextBlock genre4_TextBlock;
        private TextBlock genre5_TextBlock;
        private TextBlock num_Genre1_TextBlock;
        private TextBlock num_Genre2_TextBlock;
        private TextBlock num_Genre3_TextBlock;
        private TextBlock num_Genre4_TextBlock;
        private TextBlock num_Genre5_TextBlock;

        // Social
        private ListBox activityFeedBox;
        private TextBox activityTextBox;

        // Loading State Management
        bool libraryLoaded = false;
        bool activityFeedLoaded = false;

        static bool userInfoLoaded = false;
        bool refreshingUserInfo = false;

        public UserDashboardPage()
        {
            this.InitializeComponent();
        }

        private async void UserDashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("UserDashboardPage: Begin Loaded");

            if (MainPage.RecentlyLoggedOut ||
               (MainPage.ComingFromPage == PageType.SettingsPage && MainPage.RecentlyLoggedIn))
            {
                Debug.WriteLine("UserDashboardPage: End Loaded");

                GoBack();
                return;
            }
            else if (pageParameter.user.IsLoggedIn)
            {
                if (Consts.forceLibrarySave)
                {
                    Debug.WriteLine("Forcing Save");

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    await Consts.LoggedInUser.Save();

                    Consts.forceLibrarySave = false;
                }
            }
            else { }

            Debug.WriteLine("UserDashboardPage: End Loaded");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("UserDashboardPage: Begin OnNavigatedTo");
            base.OnNavigatedTo(e);

            if (MainPage.RecentlyLoggedOut ||
               (MainPage.ComingFromPage == PageType.SettingsPage && MainPage.RecentlyLoggedIn))
            {
                Debug.WriteLine("UserDashboardPage: Early Leave");
                return;
            }

            // Subscribe to the back button event as to not close the page
            APIv1.FeedbackEventHandler += APIv1_FeedbackEventHandler;

            // Save the parameters
            pageParameter = e.Parameter as UserDashboardPageParameter;

            if (pageParameter.user.IsLoggedIn)
            {
                // Because data could have been changed within another area of the app further ahead, we will do a check which if positive,
                // will rewrite the parameter user with our logged in user
                pageParameter.user = Consts.LoggedInUser;
            }
            else { }

            // Finally, Load the Library
            LoadLibrary();

            Debug.WriteLine("UserDashboardPage: End OnNavigatedTo");
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
            Debug.WriteLine("UserDashboardPage: GoBack()");

            // Describe from event
            APIv1.FeedbackEventHandler -= APIv1_FeedbackEventHandler;

            // Exit the application
            if (MainPage.ComingFromPage == PageType.SettingsPage) MainPage.ComingFromPage = PageType.LoggedOutPage;
            else MainPage.ComingFromPage = PageType.UserDashboardPage;

            // Exit the application
            Application.Current.Exit();
        }

        #region Navigation Events
        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            Consts.UpdateLoggedInUser(pageParameter.user);

            var newFlyout = new OptionsSettingsPane();
            newFlyout.ShowIndependent();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            Consts.UpdateLoggedInUser(pageParameter.user);

            var newFlyout = new AboutSettingsPane();
            newFlyout.ShowIndependent();
        }

        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(SearchPage));
        }
        #endregion

        #region SearchBox
        private void SearchBar_Loaded(object sender, RoutedEventArgs e)
        {
            searchBox = (sender as SearchBox);
        }

        private void SearchBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            if (searchBox == null) return;

            string queryText = args.QueryText;
            if (!string.IsNullOrEmpty(queryText))
            {
                ObservableCollection<Anime> localSearch = Consts.LoggedInUser.animeLibrary.Search(queryText);
                Windows.ApplicationModel.Search.SearchSuggestionCollection suggestionCollection = args.Request.SearchSuggestionCollection;

                foreach (Anime a in localSearch)
                {
                    suggestionCollection.AppendQuerySuggestion(a.RomanjiTitle);
                }
            }
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;
            if (searchBox == null) return;

            Consts.UpdateLoggedInUser(pageParameter.user);
            SearchPageParameter searchPageParam = new SearchPageParameter(searchBox.QueryText, SearchFilter.Everything);
            Frame.Navigate(typeof(SearchPage), searchPageParam);
        }
        #endregion

        private void UserInfo_Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            if (refreshingUserInfo || !libraryLoaded || Consts.forceLibrarySave) { return; }

            LoadUserInfo(true);
        }

        private async void Review_Clicked(object sender, RoutedEventArgs e)
        {
            if (!libraryLoaded || Consts.forceLibrarySave) return;

            await Consts.LaunchReview();
        }

        private void ApplicationProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationProgressBar = (sender as ProgressRing);
        }
    }
}
