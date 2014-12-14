using Anitro.APIs;
using Anitro.Data_Structures;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Structures;
using Anitro.APIs.Events;
using Anitro.Data_Structures.Enumerators;
using Anitro.APIs.Hummingbird;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.StartScreen;
using Windows.UI.Popups;
using System.Threading.Tasks;

#if WINDOWS_PHONE_APP
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Advertising.Mobile.Common;
#else
using Microsoft.Advertising.WinRT.UI;
//using Microsoft.Advertising.WinRT.Common;
#endif

namespace Anitro
{
    public partial class AnimePage
    {
        private void AnimePage_Loaded(object sender, RoutedEventArgs e)
        {
            me = this;
        }

        private void AdControl_Loaded(object sender, RoutedEventArgs e)
        {
            adControl = (sender as AdControl);
            XamlControlHelper.AnitroAdControlSettings(adControl);
        }
        private void AdControl_ErrorOccured(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine("AdControl error (" + ((AdControl)sender).Name + "): " + e.Error + " ErrorCode: " + e.ErrorCode.ToString());
        }

        #region Message Box
        private void NotConnected_CommandInvokedHandler(IUICommand command)
        {
            Application.Current.Exit();
        }
        private async Task NotConnectedToInternetMessageBox()
        {
            Debug.WriteLine("GetAnimeInfo(): Not connected to the internet");

            var messageDialog = new MessageDialog("No internet connection has been found.");
            messageDialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(NotConnected_CommandInvokedHandler)));

            messageDialog.DefaultCommandIndex = 0; // Set the command that will be invoked by default
            messageDialog.CancelCommandIndex = 0; // Set the command to be invoked when escape is pressed

            await messageDialog.ShowAsync();
        }
        #endregion
        
        #region AppBar
        private void Favourite_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            favouriteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            unfavouriteButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void UnFavourite_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            favouriteButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            unfavouriteButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void Pin_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            try
            {
#if WINDOWS_PHONE_APP
                bool result = await SecondaryTileHelper.CreateTile(libraryObject.anime.slug,
                                                                   libraryObject.anime.title,
                                                                   new AnitroLaunchArgs(AnitroLaunchType.Anime, libraryObject.anime.slug),
                                                                   libraryObject.anime.cover_image_uri,
                                                                   TileSize.Default);
#else
                bool result = await SecondaryTileHelper.CreateTile(sender,
                                                                   libraryObject.anime.slug,
                                                                   libraryObject.anime.title,
                                                                   new AnitroLaunchArgs(AnitroLaunchType.Anime, libraryObject.anime.slug),
                                                                   libraryObject.anime.cover_image_uri,
                                                                   TileSize.Default);
#endif

                if (result)
                {
                    pinButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    unpinButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            catch (Exception) { }
        }
        private async void UnPin_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            bool result = await SecondaryTileHelper.RemoveTile(libraryObject.anime.slug, SecondaryTileHelper.GetElementRect((FrameworkElement)sender));

            if (result)
            {
                pinButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                unpinButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void SetAsLockscreen_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            Debug.WriteLine("Launching Anitro Lockscreen");
            var uri = Consts.CreateAnitroCompanionUri(libraryObject.anime.slug);

            Debug.WriteLine("Launching Uri");
            var success = await Windows.System.Launcher.LaunchUriAsync(uri, Consts.AnitroCompanionLauncherOptions);
        }

        private async void Save_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded || !objectChanged || !Consts.LoggedInUser.IsLoggedIn) return;

            Debug.WriteLine("Posting Update");

            if (animeRemoved) { RemoveFromLibrary(); AnimeRemoved(false); }
            else UpdateLibrary();

            await Consts.LoggedInUser.Save(); //Consts.forceLibrarySave = true;
            AnimeUpdated(false);
        }

        private async void OpenIE_Clicked(object sender, RoutedEventArgs e)
        {
            if (!contentLoaded) return;

            var uri = new Uri(libraryObject.anime.url, UriKind.Absolute);

            var options = new Windows.System.LauncherOptions();
            options.TreatAsUntrusted = false;

            var success = await Windows.System.Launcher.LaunchUriAsync(uri, options);
        }
        #endregion

        #region Helper Functions
        private void AnimeUpdated(bool isEnabled)
        {
            switch (isEnabled)
            {
                case true:
                    saveButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    objectChanged = true;
                    break;
                case false:
                    saveButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    objectChanged = false;
                    break;
            }
        }
        private void AnimeRemoved(bool isEnabled)
        {
            switch (isEnabled)
            {
                case true:
                    saveButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    animeRemoved = true;
                    break;
                case false:
                    saveButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    animeRemoved = false;
                    break;
            }
        }
        #endregion

        #region Update Library
        private async void UpdateLibrary()
        {
            if (!contentLoaded) { return; }//false; }
            if (!Consts.LoggedInUser.IsLoggedIn) { return; }// false; }
            if (!objectChanged) { return; }

            Debug.WriteLine("UpdateLibrary(): Entering");
            if (libraryObject.status == "" || libraryObject.status == "favourites" || libraryObject.status == "search" || libraryObject.status == "none" || libraryObject.status == "recent") { Debug.WriteLine("UpdateLibrary(): Can't update library"); }
            else
            {
                Consts.LoggedInUser.animeLibrary.savingOrUpdatingLibrary = true;

                try
                {
                    if (Consts.IsConnectedToInternet())
                    {
                        XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

                        Debug.WriteLine("UpdateLibrary(): updating library on server...");

                        bool updateRating = (originalRatingText != libraryObject.rating.value);

                        await APIv1.Post.LibraryUpdate(libraryObject, updateRating, false);//.anime.slug,
                        //libraryObject.status,
                        //libraryObject.@private.ToString(),
                        //libraryObject.rating.value,
                        //Convert.ToInt32(libraryObject.rewatched_times),
                        //libraryObject.notes.ToString(),
                        //Convert.ToInt32(libraryObject.episodes_watched),
                        //false,
                        //false);
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("AnimePage:UpdateLibrary(): update failed");
                }


                // Even if it excepts, 9/10 it has already posted, so lets save and leave
                Debug.WriteLine("Updating Local Storage");
                Consts.LoggedInUser.animeLibrary.UpdateLibrary(Data_Structures.Library.GetLibrarySelectionFromStatus(libraryObject), libraryObject, false);
                //Consts.LoggedInUser.Save();

                // Re-allow people to exit again
                Consts.LoggedInUser.animeLibrary.savingOrUpdatingLibrary = false;
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
            }
            return; // false;
        }
        private async void RemoveFromLibrary()
        {
            if (!contentLoaded) { return; }//false; }
            if (!Consts.LoggedInUser.IsLoggedIn) { return; }// false; }
            if (!objectChanged) { return; }
            if (!animeRemoved) { return; }

            Debug.WriteLine("Removing " + libraryObject.anime.slug + " from Library");
            Consts.LoggedInUser.animeLibrary.savingOrUpdatingLibrary = true;

            try
            {
                if (Consts.IsConnectedToInternet())
                {
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

                    await APIv1.Post.LibraryRemove(libraryObject.anime.slug, false);
                }
            }
            catch (Exception) { }

            // Pre-emptively remove
            LibrarySelection remSel = Consts.LoggedInUser.animeLibrary.FindWhereExistsInLibrary(libraryObject.anime.slug);

            Debug.WriteLine("LibrarySelection: " + remSel.ToString());

            if (remSel == LibrarySelection.None || remSel == LibrarySelection.Favourites) { }
            else
            {
                bool result = Consts.LoggedInUser.animeLibrary.RemoveFromLibrary(remSel, libraryObject, false);

                Debug.WriteLine(result);

                foreach (LibraryObject lO in Consts.LoggedInUser.animeLibrary.OnHold)
                {
                    Debug.WriteLine(lO.anime.slug);
                }
            }

            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);

            // Re-allow people to exit again
            Consts.LoggedInUser.animeLibrary.savingOrUpdatingLibrary = false;
        }
        #endregion

        #region Control Events
        private async void LibraryPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("LibraryPicker_SelectionChanged(): Entering");

            if (!contentLoaded) { return; }
            if (!Consts.LoggedInUser.IsLoggedIn) { return; }
            if (LibraryPicker.SelectedItem == null) { return; }

            try
            {
                string previousStatus = libraryObject.status;

                switch (LibraryPicker.SelectedIndex)
                {
                    case 0: //<sys:String>None</sys:String>
                        libraryObject.status = "";
                        break;
                    case 1: //<sys:String>Currently Watching</sys:String>
                        libraryObject.status = "currently-watching";
                        break;
                    case 2: //<sys:String>Plan To Watch</sys:String>
                        libraryObject.status = "plan-to-watch";
                        break;
                    case 3: //<sys:String>Completed</sys:String>
                        libraryObject.status = "completed";
                        break;
                    case 4: //<sys:String>On Hold</sys:String>
                        libraryObject.status = "on-hold";
                        break;
                    case 5: //<sys:String>Dropped</sys:String>
                        libraryObject.status = "dropped";
                        break;
                    default:
                        break;
                }

                Debug.WriteLine("LibraryPicker_SelectionChanged(): Status set");

                if (Consts.IsConnectedToInternet())
                {
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

                    if (LibraryPicker.SelectedIndex != 0)
                    {
                        AnimeRemoved(false);
                        AnimeUpdated(true);
                        Consts.LoggedInUser.animeLibrary.SwitchLibraries(Data_Structures.Library.GetLibrarySelectionFromStatus(libraryObject), libraryObject);
                        await Consts.LoggedInUser.Save();
                    }
                    else if (LibraryPicker.SelectedIndex == 0)
                    {
                        if (!Consts.LoggedInUser.animeLibrary.DoesExistInLibrary(Consts.LoggedInUser.animeLibrary.FindWhereExistsInLibrary(libraryObject.anime.slug),
                                                                                libraryObject.anime.slug))
                        {
                            return;
                        }

                        AnimeUpdated(true);
                        AnimeRemoved(true);
                    }
                    else { savingAndUpdatingAnime = false; }
                }
            }
            catch (Exception) { savingAndUpdatingAnime = false; }

            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
        }

        private void libraryPrivate_Toggled(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);
            libraryObject.@private = libraryPrivate.IsOn;
            //UpdateLibrary();
        }

        private void libraryRewatching_Toggled(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);
#if WINDOWS_PHONE_APP
            libraryObject.rewatching = libraryRewatching.IsOn;
#else
            libraryObject.rewatching = libraryRewatching_Switch.IsOn;
#endif
            //UpdateLibrary();
        }

        private void incrementRewatchedTimesBy1(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);

            int rewatchedTimes = Convert.ToInt32(libraryObject.rewatched_times);

            libraryObject.rewatched_times = Convert.ToString(++rewatchedTimes);
            libraryRewatchedTimes.Text = libraryObject.rewatched_times.ToString();
            //UpdateLibrary();
        }

        private void decrimentRewatchedTimesBy1(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);

            int rewatchedTimes = Convert.ToInt32(libraryObject.rewatched_times);

            if (rewatchedTimes <= 0) { return; }
            libraryObject.rewatched_times = Convert.ToString(--rewatchedTimes);
            libraryRewatchedTimes.Text = libraryObject.rewatched_times.ToString();
            //UpdateLibrary();
        }

        private void incrementEpisodeWatchedBy1(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);

            int wCount = Convert.ToInt32(libraryObject.episodes_watched);
            int epCount;

            string epCountAsStr = libraryObject.anime.episode_count;
            if (string.IsNullOrEmpty(epCountAsStr) || epCountAsStr.Contains("?") || epCountAsStr.StartsWith("0")) epCount = 0;
            else { epCount = Convert.ToInt32(libraryObject.anime.episode_count); }

            bool skipLibChange = false;
            if (epCount == 0) { skipLibChange = true; }

            if (wCount >= epCount && skipLibChange == false)
            {
                LibraryPicker.SelectedIndex = 3;
                return;
            }
            else
            {
                libraryObject.episodes_watched = Convert.ToString(++wCount);
                if (epCount == 0) { libraryEpisodesWatched.Text = wCount + "/" + "?"; }
                else { libraryEpisodesWatched.Text = wCount + "/" + epCount; }
                //UpdateLibrary();

                if (wCount >= epCount && skipLibChange == false) { LibraryPicker.SelectedIndex = 3; }
                else if (LibraryPicker.SelectedIndex == 0) { LibraryPicker.SelectedIndex = 1; }
            }
        }

        private void decrimentEpisodeWatchedBy1(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);


            int wCount = Convert.ToInt32(libraryObject.episodes_watched);
            int epCount;

            if (libraryObject.anime.episode_count.Contains("?")) epCount = 0;
            else { epCount = Convert.ToInt32(libraryObject.anime.episode_count); }

            if (wCount <= 0) { return; }
            if (wCount >= epCount) { LibraryPicker.SelectedIndex = 1; }


            libraryObject.episodes_watched = Convert.ToString(--wCount);
            if (epCount == 0) { libraryEpisodesWatched.Text = wCount + "/" + "?"; }
            else { libraryEpisodesWatched.Text = wCount + "/" + epCount; }
            //UpdateLibrary();
        }

        private void notesEnterEvent(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("Enter Pressed");
                Frame.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        private void notesLostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Notes Lost Focus");

            libraryObject.notes = notesTextBox.Text;
            if (notesTextBox.Text == "") { libraryObject.notes_present = false; }
            else { libraryObject.notes_present = true; AnimeUpdated(true); }

            //UpdateLibrary();
        }

        private void incrementRatingByHalf(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);
            //libraryRating.Value += 0.5;

            double rating = libraryObject.rating.valueAsDouble;
            rating += 0.5;

            if (rating > 5) rating = 5;
            libraryObject.rating.valueAsDouble = rating;

            libraryRating.Text = libraryObject.rating.value + "/5";
        }

        private void decrimentRatingByHalf(object sender, RoutedEventArgs e)
        {
            AnimeUpdated(true);
            //libraryRating.Value -= 0.5;

            double rating = libraryObject.rating.valueAsDouble;
            rating -= 0.5;

            if (rating < 0) rating = 0;
            libraryObject.rating.valueAsDouble = rating;

            libraryRating.Text = libraryObject.rating.value + "/5";
        }
        #endregion
    }
}
