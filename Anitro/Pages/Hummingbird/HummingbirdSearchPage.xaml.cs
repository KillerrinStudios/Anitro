using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Models;
using Anitro.Models.Page_Parameters;
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
    public sealed partial class HummingbirdSearchPage : Page
    {
        public HummingbirdSearchViewModel ViewModel { get { return (HummingbirdSearchViewModel)DataContext; } }

        public HummingbirdSearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SearchParameter parameter = (SearchParameter)e.Parameter;
            ViewModel.User = parameter.User as HummingbirdUser;
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
            AnimeObject anime = (AnimeObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(anime);
        }

        private void AnimeLibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LibraryObject libraryObject = (LibraryObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(libraryObject.Anime);
        }

        private void MangaLibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LibraryObject libraryObject = (LibraryObject)e.ClickedItem;
            ViewModel.NavigateAnimeDetails(libraryObject.Anime);
        }
        #endregion


    }
}
