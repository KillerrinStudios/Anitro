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
using Anitro.Data_Structures.Enumerators;
using Anitro.APIs.Hummingbird;
using Microsoft.Advertising.Mobile.UI;

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

        TextBox searchBox;
        ListBox searchResult_ListBox;
        ComboBox searchFilter_ComboBox;

        private bool isSearching = false;
        private bool searchFilterLoaded = false;

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

            // Subscribe to the back button event
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            try
            {
                // Check if the URI Association was handled already, if so we can leave
                if (Consts.uriAssociationHandled) return;
                if (e.Parameter != null)
                {
                    // Set the handle to true
                    Consts.uriAssociationHandled = true;

                    // Save the Parameters
                    pageParameter = (e.Parameter as SearchPageParameter);
                }
            }
            catch (Exception) { }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
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

            e.Handled = true;

            GoBack();
        }

        private void GoBack()
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            Frame.GoBack();
        }

        private void SetAndSearchAnime(SearchPageParameter _pageParameter)
        {
            Debug.WriteLine("Launching from URISceme. Searching: " + _pageParameter.searchTerm);

            try
            {
                searchBox.Text = _pageParameter.searchTerm;
            }
            catch (Exception ex) { XamlControlHelper.SetDebugString(debugTextBlock, DebugTools.PrintOutException("", ex)); }
                
            Search(_pageParameter.searchTerm);
        }

        private void searchBoxEnterEvent(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("Enter Pressed");

                XamlControlHelper.LoseFocusOnTextBox(searchBox);
                Search(searchBox.Text);
            }
            else
            {
                SearchOffline(searchBox.Text);
            }
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
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
                isSearching = true;

                APIv1.APICompletedEventHandler += SearchCompleted;

                this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    APIv1.Get.SearchAnime(_text);
                });
            }
        }

        private void SearchOffline(string _text)
        {
            try
            {
                ObservableCollection<Anime> localSearch = Consts.LoggedInUser.animeLibrary.Search(_text);

                searchResults = localSearch;
                searchResult_ListBox.ItemsSource = searchResults;
            }
            catch (Exception) { }
        }

        private void SearchCompleted(object sender, APIs.Events.APICompletedEventArgs e)
        {
            if (e.Type != APIType.Search) { return; }

            // Ensure the focus doesn't change
            try
            {
                mainHub.ScrollToSection(search_HubSection);
            }
            catch (Exception) { }

            switch (e.Result)
            {
                case Anitro.Data_Structures.APIResponse.Successful:
                    Debug.WriteLine("Anime Searched Successfully");
                    try
                    {
                        List<Anime> animeList = (sender as List<Anime>);

                        searchResults = new ObservableCollection<Anime>();
                        foreach (Anime a in animeList)
                        {
                            searchResults.Add(a);
                        }

                        searchResult_ListBox.ItemsSource = searchResults;
                    }
                    catch (Exception) { }
                    break;
                case Anitro.Data_Structures.APIResponse.Failed:
                    Debug.WriteLine("Anime Search Failed");
                    break;
            }

            // Remove the Event Handler
            APIv1.APICompletedEventHandler -= SearchCompleted;

            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
            isSearching = false;
        }

        private void listBox_Tap(object sender, TappedRoutedEventArgs e)
        {
            if (Consts.forceLibrarySave || isSearching) return;

            try
            {
                if (((ListBox)sender).SelectedItem == null) { return; }

                Anime _anime = ((ListBox)sender).SelectedItem as Anime;

                ((ListBox)sender).SelectedItem = null;

                // Remove the Event Handler for a safe transition
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

                AnimePageParameter pageParameter = new AnimePageParameter(_anime.ServiceID, AnimePageParameter.ComingFrom.Search);
                Frame.Navigate(typeof(AnimePage), pageParameter);
            }
            catch (Exception) { }
        }


        private void OnSpeechActionButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            if (VoiceCommandHelper.cortanaRecognizerState == CortanaRecognizerState.Listening) return;

            Debug.WriteLine("OnSpeechActionButtonTapped(): Entering");

            VoiceCommandHelper.CortanaVoiceRecognitionResult += VoiceCommandHelper_CortanaVoiceRecognitionResult;
            VoiceCommandHelper.PlayCortanaListeningEarcon(player);

            // Select a random title from the library

            string defaultExampleText = "just speak the english or japanese title.";
            string _exampletxt;

            try
            {
                if (Consts.LoggedInUser.IsLoggedIn)
                {
                    _exampletxt = Consts.LoggedInUser.animeLibrary.SelectRandomTitleFromLibrary(LibrarySelection.All);
                    _exampletxt = defaultExampleText + " ex: '" + _exampletxt + "'";
                }
                else { _exampletxt = defaultExampleText; }
            }
            catch (Exception) { _exampletxt = defaultExampleText; }


            VoiceCommandHelper.StartListening(_exampletxt, false, false);
        }

        void VoiceCommandHelper_CortanaVoiceRecognitionResult(object sender, VoiceRecognitionResultEventArgs e)
        {
            if (e.Result != APIResponse.Successful) {
                VoiceCommandHelper.CortanaVoiceRecognitionResult -= VoiceCommandHelper_CortanaVoiceRecognitionResult;
                return;
            }

            if (e.SpeechResult.Text != "") {
                string speechTxtResult;
                if (e.SpeechResult.Text.ToLower().StartsWith("search for"))
                {
                    speechTxtResult = e.SpeechResult.Text.Substring(11);
                }
                else speechTxtResult = e.SpeechResult.Text;

                Debug.WriteLine("Speech Query: " + speechTxtResult);
                searchBox.Text = "";// speechTxtResult;

                VoiceCommandHelper.CortanaFeedback("Searching for, " + speechTxtResult, player);
                Search(speechTxtResult);
            }

            VoiceCommandHelper.CortanaVoiceRecognitionResult -= VoiceCommandHelper_CortanaVoiceRecognitionResult;
        }

        private async void SearchFilterComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            // Early return to keep from saving every load
            if (!searchFilterLoaded) { searchFilterLoaded = true; return; }

            switch (searchFilter_ComboBox.SelectedIndex)
            {
                case 0:
                    await Consts.AppSettings.SetSearchFilter(SearchFilter.Everything);
                    break;
                case 1:
                    await Consts.AppSettings.SetSearchFilter(SearchFilter.Anime);
                    break;
                case 2:
                    await Consts.AppSettings.SetSearchFilter(SearchFilter.Manga);
                    break;
                case 3:
                    await Consts.AppSettings.SetSearchFilter(SearchFilter.User);
                    break;
                default:
                    break;
            }
        }

        #region Loaded Events
        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdControl);
            XamlControlHelper.AnitroAdControlSettings(adControl);
        }

        private void ListBoxSearch_Loaded(object sender, RoutedEventArgs e)
        {
            searchResult_ListBox = (sender as ListBox);

            // Set Listbox to Search Results
            searchResult_ListBox.ItemsSource = searchResults;
        }


        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            searchBox = (sender as TextBox);

            // Call the page parameter since it is now safe
            try
            {
                if (pageParameter != null)
                {
                    SetAndSearchAnime(pageParameter);
                }
            }
            catch (Exception ex) { }
        }

        private void SearchFilterComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            searchFilter_ComboBox = (sender as ComboBox);

            SearchFilter filter;
            if (pageParameter != null) {
                filter = pageParameter.searchType;
            }
            else {
                filter = Consts.AppSettings.SearchFilter;
            }

            switch (filter)
            {
                case SearchFilter.Everything:
                    searchFilter_ComboBox.SelectedIndex = 0;
                    break;
                case SearchFilter.Anime:
                    searchFilter_ComboBox.SelectedIndex = 1;
                    break;
                case SearchFilter.Manga:
                    searchFilter_ComboBox.SelectedIndex = 2;
                    break;
                case SearchFilter.User:
                    searchFilter_ComboBox.SelectedIndex = 3;
                    break;
            }

        }
        #endregion

        private void MainHub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var section = mainHub.SectionsInView[0];
            var tag = section.Tag.ToString();

            switch (tag)
            {
                case "0":
                    break;
                case "1":
                    if (isSearching) mainHub.ScrollToSection(search_HubSection); 
                    break;
            }
        }

        private void AdControl_ErrorOccured(object sender, Microsoft.Advertising.Mobile.Common.AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.Error + " ErrorCode: " + e.ErrorCode.ToString());
        }
    }
}
