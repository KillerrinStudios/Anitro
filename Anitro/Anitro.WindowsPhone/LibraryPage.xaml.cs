using Anitro.Data_Structures;
using Anitro.Data_Structures.Enumerators;
using Anitro.Data_Structures.API_Classes;
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
    public sealed partial class LibraryPage : Page
    {
        public LibraryPageParameter pageParameter;
        bool pageRefreshing = false;

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

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            pageParameter = e.Parameter as LibraryPageParameter;

            // Set the specific text
            switch (pageParameter.libraryType)
            {
                case LibraryType.Anime:
                    libraryPivot.Title = pageParameter.user.Username + "'s Anime Library";
                    currently_PivotItem.Header = "Currently Watching";
                    planTo_PivotItem.Header = "Plan To Watch";
                    break;
                case LibraryType.Manga:
                    libraryPivot.Title = pageParameter.user.Username + "'s Manga Library";
                    currently_PivotItem.Header = "Currently Reading";
                    planTo_PivotItem.Header = "Plan To Read";
                    break;
            }

            if (pageParameter.user.IsLoggedIn)
            {
                // Because data could have been changed within another area of the app further ahead, we will do a check which if positive,
                // will rewrite the parameter user with our logged in user
                pageParameter.user = Consts.LoggedInUser;
            }
            else { 
            //recent_PivotItem.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            // Bind the page
            BindListBoxes();
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
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

            e.Handled = true;
            GoBack();
        }

        private void GoBack()
        {
            Consts.UpdateLoggedInUser(pageParameter.user);

            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            Frame.GoBack();
        }

        #region AppBar Events
        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing) return;

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(SearchPage));
        }
        private void About_Clicked(object sender, RoutedEventArgs e)
        {
            if (pageRefreshing) return;

            // Remove the Event Handler for a safe transition
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;

            Consts.UpdateLoggedInUser(pageParameter.user);
            Frame.Navigate(typeof(About));
        }
        #endregion
    }
}
