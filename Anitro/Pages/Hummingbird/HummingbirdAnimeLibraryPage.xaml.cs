using AnimeTrackingServiceWrapper.Converters;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
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
    public sealed partial class HummingbirdAnimeLibraryPage : Page
    {
        public HummingbirdAnimeLibraryViewModel ViewModel { get { return (HummingbirdAnimeLibraryViewModel)DataContext; } }

        public HummingbirdAnimeLibraryPage()
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
            ViewModel.User = e.Parameter as Models.HummingbirdUser;
            ViewModel.OnNavigatedTo();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        private void librarySearchFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.User.AnimeLibrary.LibraryCollection.SearchFilter.SearchTerm = ((TextBox)sender).Text;
            ViewModel.User.AnimeLibrary.LibraryCollection.ApplyFilters();
        }

        private void librarySectionFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem comboBoxItem = (ComboBoxItem)comboBox.SelectedItem;
            ViewModel.User.AnimeLibrary.LibraryCollection.LibrarySelectionFilter.LibrarySelection = LibrarySectionConverter.StringToLibrarySection((string)comboBoxItem.Content);
            ViewModel.User.AnimeLibrary.LibraryCollection.ApplyFilters();
        }

        private void LibraryListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.LibraryItemClicked((LibraryObject)e.ClickedItem);
        }

        private void FavouritesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.FavouriteItemClicked((AnimeObject)e.ClickedItem);
        }

        private void RecentListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.RecentItemClicked((AnimeObject)e.ClickedItem);
        }
    }
}
