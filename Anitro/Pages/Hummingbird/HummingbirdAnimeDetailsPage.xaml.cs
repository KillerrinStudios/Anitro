using AnimeTrackingServiceWrapper.Converters;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
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
    public sealed partial class HummingbirdAnimeDetailsPage : Page
    {
        public HummingbirdAnimeDetailsViewModel ViewModel { get { return (HummingbirdAnimeDetailsViewModel)DataContext; } }

        public HummingbirdAnimeDetailsPage()
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

            HummingbirdAnimeDetailsParameter parameter = e.Parameter as HummingbirdAnimeDetailsParameter;
            ViewModel.User = parameter.User;
            ViewModel.LibraryObject = parameter.LibraryObject;

            ViewModel.OnNavigatedTo();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.OnNavigatedFrom();
            base.OnNavigatedFrom(e);
        }

        private void librarySectionComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;

            switch (ViewModel.LibraryObject.Section)
            {
                case LibrarySection.CurrentlyWatching: comboBox.SelectedIndex = 1; break;
                case LibrarySection.PlanToWatch: comboBox.SelectedIndex = 2; break;
                case LibrarySection.Completed: comboBox.SelectedIndex = 3; break;
                case LibrarySection.OnHold: comboBox.SelectedIndex = 4; break;
                case LibrarySection.Dropped: comboBox.SelectedIndex = 5; break;
                case LibrarySection.None:
                default: comboBox.SelectedIndex = 0; break;
            }
        }

        private void librarySectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem comboBoxItem = (ComboBoxItem)comboBox.SelectedItem;
            ViewModel.LibraryObject.Section = LibrarySectionConverter.StringToLibrarySection((string)comboBoxItem.Content);
        }
    }
}
