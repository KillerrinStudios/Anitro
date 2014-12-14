using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Anitro.Data_Structures;
using Anitro.APIs;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Structures;
using Anitro.APIs.Hummingbird;
using Microsoft.Advertising.WinRT.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Anitro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private SearchPageParameter pageParameter;

        AdControl adControl;

        private bool isSearching = false;

        // By making it static, search results will save between pages
        private static ObservableCollection<Anime> searchResults = new ObservableCollection<Anime>();

        public SearchPage()
        {
            this.InitializeComponent();
        }

        async void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Consts.LoggedInUser.IsLoggedIn)
            {
                if (Consts.forceLibrarySave)
                {
                    Debug.WriteLine("Forcing Save");
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

            // Set Listbox to Search Results
            gridView_Search.ItemsSource = searchResults;

            // Subscribe to the back button event
            

            // Check if the URI Association was handled already, if so we can leave
            if (Consts.uriAssociationHandled) return;
            if (e.Parameter != null)
            {
                // Set the handle to true
                Consts.uriAssociationHandled = true;

                // Save the Parameters
                pageParameter = e.Parameter as SearchPageParameter;
                SetAndSearchAnime(pageParameter);
            }
            
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !Consts.forceLibrarySave && 
                    !isSearching)
                {
                    loop = false;
                }
            }

            

            GoBack();
        }

        public void GoBack()
        {
            
            Frame.GoBack();
        }

        private void ChangeProgressBar(bool isEnabled)
        {
            ApplicationProgressBar.IsActive = isEnabled;
        }

        private void SetAndSearchAnime(SearchPageParameter _pageParameter)
        {
            Debug.WriteLine("Launching from URISceme. Searching: " + _pageParameter.searchTerm);
            searchBox.QueryText = _pageParameter.searchTerm;
            Search(_pageParameter.searchTerm);
        }

        private void Search(string _text)
        {
            APIResponse result;

            if (!Consts.IsConnectedToInternet()) { 
                result = APIResponse.NetworkError;
                SearchOffline(_text);
            }
            else
            {
                ChangeProgressBar(true);
                isSearching = true;

                //Frame.Focus(FocusState.Programmatic);

                APIv1.APICompletedEventHandler += SearchCompleted;

                this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    APIv1.Get.SearchAnime(_text);
                });
            }
        }

        private void SearchOffline(string _text)
        {
            ObservableCollection<Anime> localSearch = Consts.LoggedInUser.animeLibrary.Search(_text);

            searchResults = localSearch;
            gridView_Search.ItemsSource = searchResults;
        }

        private void SearchCompleted(object sender, APIs.Events.APICompletedEventArgs e)
        {
            if (e.Type != APIType.Search) { return; }

            switch (e.Result)
            {
                case Anitro.Data_Structures.APIResponse.Successful:
                    Debug.WriteLine("Anime Searched Successfully");
                    List<Anime> animeList = (sender as List<Anime>);

                    searchResults = new ObservableCollection<Anime>();
                    foreach (Anime a in animeList)
                    {
                        searchResults.Add(a);
                    }

                    gridView_Search.ItemsSource = searchResults;
                    break;
                case Anitro.Data_Structures.APIResponse.Failed:
                    Debug.WriteLine("Anime Search Failed");
                    break;
            }

            // Remove the Event Handler
            APIv1.APICompletedEventHandler -= SearchCompleted;

            ChangeProgressBar(false);
            isSearching = false;
        }

        private void SearchBox_Clicked(object sender, ItemClickEventArgs e)
        {
            if (Consts.forceLibrarySave || isSearching) return;
            Anime selected = e.ClickedItem as Anime;

            ((GridView)sender).SelectedItem = null;

            if (!string.IsNullOrEmpty(selected.slug))
            {
                AnimePageParameter pageParam = new AnimePageParameter(selected.slug, AnimePageParameter.ComingFrom.Search);
                Frame.Navigate(typeof(AnimePage), pageParam);
            }
        }


        #region SearchBox
        private void searchBoxEnterEvent(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

            }
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            gridView_Search.Focus(FocusState.Programmatic);
            Search(searchBox.QueryText);
        }

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

                searchResults = localSearch;
                gridView_Search.ItemsSource = searchResults;
            }
        }
        #endregion

        #region Required due to stupid errors
        private void listBox_Tap(object sender, TappedRoutedEventArgs e)
        {

        }
        #endregion

        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdControl);
            XamlControlHelper.AnitroAdControlSettings(adControl);
        }

        private void AdControl_ErrorOccured(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.Error + " ErrorCode: " + e.ErrorCode.ToString());
        }
    }
}
