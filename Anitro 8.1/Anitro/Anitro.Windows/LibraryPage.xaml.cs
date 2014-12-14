using Anitro.Data_Structures;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.Structures;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LibraryPage : Page
    {
        public LibraryPageParameter pageParameter;
        bool pageRefreshing = false;

        private GridView recent_GridView;
        private GridView favourites_GridView;
        private ListBox currentlyWatching_ListBox;
        private ListBox planToWatch_ListBox;
        private ListBox completed_ListBox;
        private ListBox onHold_ListBox;
        private ListBox dropped_ListBox;

        public LibraryPage()
        {
            this.InitializeComponent();
        }

        async void LibraryPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (pageParameter.user.IsLoggedIn)
            {
                if (Consts.forceLibrarySave)
                {
                    Debug.WriteLine("Forcing Save");

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    await Consts.LoggedInUser.Save();

                    Consts.forceLibrarySave = false;
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            pageParameter = e.Parameter as LibraryPageParameter;

            // Set the specific text
            switch (pageParameter.libraryType)
            {
                case LibraryType.Anime:
                    pageTitle.Text = pageParameter.user.Username + "'s Anime Library";
                    currently_HubSection.Header = "Currently Watching";
                    planTo_HubSection.Header = "Plan To Watch";
                    break;
                case LibraryType.Manga:
                    pageTitle.Text = pageParameter.user.Username + "'s Manga Library";
                    currently_HubSection.Header = "Currently Reading";
                    planTo_HubSection.Header = "Plan To Read";
                    break;
            }

            if (pageParameter.user.IsLoggedIn)
            {
                // Because data could have been changed within another area of the app further ahead, we will do a check which if positive,
                // will rewrite the parameter user with our logged in user
                pageParameter.user = Consts.LoggedInUser;
            }
            else
            {
                //recent_PivotItem.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !Consts.forceLibrarySave &&
                    !pageRefreshing)
                {
                    loop = false;
                }
            }

            
            GoBack();
        }

        public void GoBack()
        {
            Consts.UpdateLoggedInUser(pageParameter.user);

            Frame.GoBack();
        }

        #region SearchBox
        private void SearchBox_SuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            string queryText = args.QueryText;
            if (!string.IsNullOrEmpty(queryText))
            {
                ObservableCollection<Anime> localSearch = Consts.LoggedInUser.animeLibrary.Search(queryText);
                Windows.ApplicationModel.Search.SearchSuggestionCollection suggestionCollection = args.Request.SearchSuggestionCollection;

                foreach (Anime a in localSearch)
                {
                    suggestionCollection.AppendQuerySuggestion(a.title);
                }
            }
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            if (Consts.forceLibrarySave) return;

            SearchPageParameter searchPageParam = new SearchPageParameter(searchBox.QueryText, SearchFilter.Anime);
            Frame.Navigate(typeof(SearchPage), searchPageParam);
        }
        #endregion

        #region AppBar Events
        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing) return;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(SearchPage));
        }
        private void About_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing) return;

            Consts.UpdateLoggedInUser(pageParameter.user);
            var newFlyout = new AboutSettingsPane();
            newFlyout.ShowIndependent();
        }

        #endregion

        #region Loaded Events
        private void CurrentlyWatching_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            currentlyWatching_ListBox = (sender as ListBox);
            currentlyWatching_ListBox.ItemsSource = Consts.LoggedInUser.animeLibrary.CurrentlyWatching;
        }

        private void PlanToWatch_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            planToWatch_ListBox = (sender as ListBox);
            planToWatch_ListBox.ItemsSource = Consts.LoggedInUser.animeLibrary.PlanToWatch;
        }

        private void Completed_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            completed_ListBox = (sender as ListBox);
            completed_ListBox.ItemsSource = Consts.LoggedInUser.animeLibrary.Completed;
        }

        private void OnHold_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            onHold_ListBox = (sender as ListBox);
            onHold_ListBox.ItemsSource = Consts.LoggedInUser.animeLibrary.OnHold;
        }

        private void Dropped_ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            dropped_ListBox = (sender as ListBox);
            dropped_ListBox.ItemsSource = Consts.LoggedInUser.animeLibrary.Dropped;
        }

        private void Favourites_GridView_Loaded(object sender, RoutedEventArgs e)
        {
            favourites_GridView = (sender as GridView);
            favourites_GridView.ItemsSource = Consts.LoggedInUser.animeLibrary.Favourites;
        }
        private void Recent_GridView_Loaded(object sender, RoutedEventArgs e)
        {
            recent_GridView = (sender as GridView);
            recent_GridView.ItemsSource = Consts.LoggedInUser.animeLibrary.Recent;
        }
        #endregion
    }
}
