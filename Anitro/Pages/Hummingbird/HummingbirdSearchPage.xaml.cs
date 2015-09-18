using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Models;
using Anitro.Models.Page_Parameters;
using Anitro.ViewModels.Hummingbird;
using System;
using System.Collections.Generic;
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

namespace Anitro.Pages.Hummingbird
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HummingbirdSearchPage : Page
    {
        public HummingbirdSearchViewModel ViewModel { get { return (HummingbirdSearchViewModel)DataContext; } }

        public HummingbirdSearchPage()
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

            HummingbirdSearchParameter parameter = (HummingbirdSearchParameter)e.Parameter;
            ViewModel.SearchTerms = parameter.SearchTerms;
            ViewModel.Filter = parameter.Filter;
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        #region SearchBox
        private void searchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.UpdateAutoSuggestions();
            }
        }

        private void searchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AnimeObject animeObject = args.SelectedItem as AnimeObject;
            sender.Text = animeObject.RomanjiTitle;
        }

        private void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
                //ViewModel.SearchAnime();
                AnimeObject anime = (AnimeObject)args.ChosenSuggestion;
                ViewModel.NavigateAnimeDetails(anime);
            }
            else
            {
                // Use args.QueryText to determine what to do.
                ViewModel.SearchAnime();
            }
        }
        #endregion

        #region ListView Clicked
        private void OnlineLibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("OnlineLibraryListView_ItemClick");
            AnimeObject anime = (AnimeObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(anime);
        }

        private void AnimeLibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("AnimeLibraryListView_ItemClick");
            LibraryObject libraryObject = (LibraryObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(libraryObject.Anime);
        }

        private void MangaLibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("MangaLibraryListView_ItemClick");
            LibraryObject libraryObject = (LibraryObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(libraryObject.Anime);
        }
        #endregion

        #region AdMediator
        private void AdMediator_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AdMediator_AdSdkError(object sender, Microsoft.AdMediator.Core.Events.AdFailedEventArgs e)
        {
            Debug.WriteLine("AdSdkError by {0} ErrorCode: {1} ErrorDescription: {2} Error: {3}", e.Name, e.ErrorCode, e.ErrorDescription, e.Error);
        }

        private void AdMediator_AdSdkEvent(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdSdk event {0} by {1}", e.EventName, e.Name);
        }

        private void AdMediator_AdMediatorFilled(object sender, Microsoft.AdMediator.Core.Events.AdSdkEventArgs e)
        {
            Debug.WriteLine("AdFilled:" + e.Name);
        }

        private void AdMediator_AdMediatorError(object sender, Microsoft.AdMediator.Core.Events.AdMediatorFailedEventArgs e)
        {
            Debug.WriteLine("AdMediatorError:" + e.Error + " " + e.ErrorCode);
            // if (e.ErrorCode == AdMediatorErrorCode.NoAdAvailable)
            // AdMediator will not show an ad for this mediation cycle
        }
        #endregion

    }
}
