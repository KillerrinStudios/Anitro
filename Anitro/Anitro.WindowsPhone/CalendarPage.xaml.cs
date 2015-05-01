using Anitro.APIs;
using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures.API_Classes;
using Anitro.Data_Structures.Structures;
using Anitro.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class CalendarPage : Page
    {
        CalendarPageViewModel pageParameter;
        public static bool alreadyLoaded = false;

        public CalendarPage()
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
            pageParameter = e.Parameter as CalendarPageViewModel;
            this.DataContext = pageParameter;

            // Subscribe to the back button event as to not close the page
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            // Bind the Correct Values to the Pivot Headers
            // Get the days in order
            List<DateTime> daysInOrderFromNow;
            daysInOrderFromNow = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                DateTime day = DateTime.Now.AddDays(i);
                daysInOrderFromNow.Add(day);
            }
            pageParameter.Today = DateTime.Now;
            DayOfWeek todayDayOfWeek = pageParameter.Today.DayOfWeek;

            switch (todayDayOfWeek)
            {
                case DayOfWeek.Sunday:       sundayPivotItem.Header =       "Today";    break;
                case DayOfWeek.Monday:       mondayPivotItem.Header =       "Today";    break;
                case DayOfWeek.Tuesday:      tuesdayPivotItem.Header =      "Today";    break;
                case DayOfWeek.Wednesday:    wednesdayPivotItem.Header =    "Today";    break;
                case DayOfWeek.Thursday:     thursdayPivotItem.Header =     "Today";    break;
                case DayOfWeek.Friday:       fridayPivotItem.Header =       "Today";    break;
                case DayOfWeek.Saturday:     saturdayPivotItem.Header =     "Today";    break;
            }
            calendarPivot.SelectedIndex = (int)todayDayOfWeek;

            // Double check to ensure noone gets in here that isn't suppose to be
            if (!InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                GoBack();
            }

            // Begin Loading the Calendar
            pageParameter.CalendarLoaded += pageParameter_CalendarLoaded;
            pageParameter.CalendarFailedToLoad += pageParameter_CalendarFailedToLoad;

            if (!alreadyLoaded)
                pageParameter.LoadCalendar(ApplicationProgressBar, this.Dispatcher);
            else
            {
                pageParameter_CalendarLoaded(pageParameter, null);
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            bool loop = true;
            while (loop)
            {
                if (APIs.StorageTools.isSavingComplete &&
                    !Consts.forceLibrarySave)
                {
                    loop = false;
                }
            }

            e.Handled = true;
            GoBack();
        }

        private void GoBack()
        {
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
            Consts.UpdateLoggedInUser(pageParameter.user);

            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            pageParameter.CalendarLoaded -= pageParameter_CalendarLoaded;
            pageParameter.CalendarFailedToLoad -= pageParameter_CalendarFailedToLoad;
            
            pageParameter.Dispose();
            alreadyLoaded = false;

            Frame.GoBack();
        }

        void pageParameter_CalendarLoaded(object sender, EventArgs e)
        {
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    alreadyLoaded = true;

                    listbox_sunday.ItemsSource = pageParameter.calendarDaySunday;
                    listbox_monday.ItemsSource = pageParameter.calendarDayMonday;
                    listbox_tuesday.ItemsSource = pageParameter.calendarDayTuesday;
                    listbox_wednesday.ItemsSource = pageParameter.calendarDayWednesday;
                    listbox_thursday.ItemsSource = pageParameter.calendarDayThursday;
                    listbox_friday.ItemsSource = pageParameter.calendarDayFriday;
                    listbox_saturday.ItemsSource = pageParameter.calendarDaySaturday;

                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                }
            );
        }

        void pageParameter_CalendarFailedToLoad(object sender, EventArgs e)
        {
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                GoBack();
            });
        }

        private void CalendarEntryListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Consts.forceLibrarySave) return;

            try
            {
                if (((ListBox)sender).SelectedItem == null) { return; }
                CalendarEntry selected = ((ListBox)sender).SelectedItem as CalendarEntry;

                ((ListBox)sender).SelectedItem = null;

                if (!string.IsNullOrEmpty(selected.Anime.ServiceID))//Consts.settings.userName)
                {
#if WINDOWS_PHONE_APP
                    // Remove the Event Handler for a safe transition
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
#endif
                    pageParameter.CalendarLoaded -= pageParameter_CalendarLoaded;
                    pageParameter.CalendarFailedToLoad -= pageParameter_CalendarFailedToLoad;
                    pageParameter.Dispose();

                    Consts.UpdateLoggedInUser(pageParameter.user);
                    AnimePageParameter sendParameter = new AnimePageParameter(selected.Anime.ServiceID, AnimePageParameter.ComingFrom.Calendar);
                    Frame.Navigate(typeof(AnimePage), sendParameter);
                }
            }
            catch (Exception) { }
        }
    }
}
