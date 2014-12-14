using Anitro.Data_Structures;
using Anitro.Data_Structures.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Anitro
{
    public partial class LibraryPage
    {
        #region Control Tapped
        private void Library_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Consts.forceLibrarySave || pageRefreshing) return;

            try
            {
                if (((ListBox)sender).SelectedItem == null) { return; }
                Anitro.Data_Structures.API_Classes.LibraryObject selected = ((ListBox)sender).SelectedItem as Anitro.Data_Structures.API_Classes.LibraryObject;

                ((ListBox)sender).SelectedItem = null;

                if (!string.IsNullOrEmpty(selected.anime.slug))//Consts.settings.userName)
                {
#if WINDOWS_PHONE_APP
                    // Remove the Event Handler for a safe transition
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    AnimePageParameter sendParameter = new AnimePageParameter(selected.anime.slug, AnimePageParameter.ComingFrom.LibraryPage);
                    Frame.Navigate(typeof(AnimePage), sendParameter);
                }
            }
            catch (Exception) { }
        }
        private void Favourites_GridView_Clicked(object sender, ItemClickEventArgs e)
        {
            if (Consts.forceLibrarySave || pageRefreshing) return;

            Anitro.Data_Structures.API_Classes.LibraryObject selected = e.ClickedItem as Anitro.Data_Structures.API_Classes.LibraryObject;

            ((GridView)sender).SelectedItem = null;

            if (!string.IsNullOrEmpty(selected.anime.slug))
            {
#if WINDOWS_PHONE_APP
                // Remove the Event Handler for a safe transition
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                Consts.UpdateLoggedInUser(pageParameter.user);
                AnimePageParameter sendParameter = new AnimePageParameter(selected.anime.slug, AnimePageParameter.ComingFrom.Favourites);
                Frame.Navigate(typeof(AnimePage), sendParameter);
            }
        }
        private void Recent_GridView_Clicked(object sender, ItemClickEventArgs e)
        {
            if (Consts.forceLibrarySave || pageRefreshing) return;

            Anitro.Data_Structures.API_Classes.LibraryObject selected = e.ClickedItem as Anitro.Data_Structures.API_Classes.LibraryObject;

            ((GridView)sender).SelectedItem = null;

            if (!string.IsNullOrEmpty(selected.anime.slug))
            {
#if WINDOWS_PHONE_APP
                // Remove the Event Handler for a safe transition
                Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                Consts.UpdateLoggedInUser(pageParameter.user);
                AnimePageParameter sendParameter = new AnimePageParameter(selected.anime.slug, AnimePageParameter.ComingFrom.Recent);
                Frame.Navigate(typeof(AnimePage), sendParameter);
            }
        }
        #endregion

        #region LoadLibrary
        private void BindListBoxes()
        {
            if (recent_GridView != null)
                recent_GridView.ItemsSource = pageParameter.user.animeLibrary.Recent;

            if (currentlyWatching_ListBox != null)
                currentlyWatching_ListBox.ItemsSource = pageParameter.user.animeLibrary.CurrentlyWatching;

            if (planToWatch_ListBox != null)
                planToWatch_ListBox.ItemsSource = pageParameter.user.animeLibrary.PlanToWatch;

            if (completed_ListBox != null)
                completed_ListBox.ItemsSource = pageParameter.user.animeLibrary.Completed;

            if (onHold_ListBox != null)
                onHold_ListBox.ItemsSource = pageParameter.user.animeLibrary.OnHold;

            if (dropped_ListBox != null)
                dropped_ListBox.ItemsSource = pageParameter.user.animeLibrary.Dropped;

            if (favourites_GridView != null)
                favourites_GridView.ItemsSource = pageParameter.user.animeLibrary.Favourites;
        }

        public void UpdateLibraryListOnScreen()
        {
            // Set to nothing to reset the list
            pageParameter.user.animeLibrary.ClearLibrary(Data_Structures.LibrarySelection.APISupported);

            // Reset the datasource to re-add items as they finish
            BindListBoxes();
        }

        private async void LoadLibrary()
        {
            if (Consts.IsConnectedToInternet())
            {
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);
                try
                {
                    pageParameter.user.animeLibrary.LibraryLoadedEventHandler += LibraryLoaded;

                    this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        pageParameter.user.animeLibrary.LoadFromStorageOrServer(pageParameter.user, pageParameter.libraryType, true);
                    });
                }
                catch (Exception) { }
            }
        }

        private void LibraryLoaded(object sender, Anitro.APIs.Events.LibraryLoadedEventArgs e)
        {
            if (e.Type != Data_Structures.APIType.LoadLibrary) return;
            Debug.WriteLine("LibraryLoaded(): AnimeLibraryPage Loaded");

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


            // Remove the Event Handler
            pageParameter.user.animeLibrary.LibraryLoadedEventHandler -= LibraryLoaded;
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

            pageRefreshing = false;
        }
        #endregion

        #region AppBar
        private void Refresh_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing) return;

            if (Consts.IsConnectedToInternet())
            {
                pageRefreshing = true;

                UpdateLibraryListOnScreen();
                LoadLibrary();
            }
        }
        private async void ClearRecent_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing || Consts.forceLibrarySave || !APIs.StorageTools.isSavingComplete) return;
            //!activityFeedLoaded || 

            if (pageParameter.user.IsLoggedIn)
            {
                pageParameter.user.animeLibrary.ClearLibrary(LibrarySelection.Recent);

                Consts.LoggedInUser = pageParameter.user;
                await Consts.LoggedInUser.Save();

                recent_GridView.ItemsSource = pageParameter.user.animeLibrary.Recent;
            }
        }
        #endregion
    }
}
