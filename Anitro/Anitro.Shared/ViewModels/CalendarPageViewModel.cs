using Anitro.APIs.Hummingbird;
using Anitro.Data_Structures;
using Anitro.Data_Structures.API_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Anitro.ViewModels
{
    public class CalendarPageViewModel : IDisposable
    {
        public User user;

        public DateTime Today;

        public event EventHandler CalendarLoaded;
        public event EventHandler CalendarFailedToLoad;

        public ObservableCollection<CalendarEntry> calendarDaySunday;
        public ObservableCollection<CalendarEntry> calendarDayMonday;
        public ObservableCollection<CalendarEntry> calendarDayTuesday;
        public ObservableCollection<CalendarEntry> calendarDayWednesday;
        public ObservableCollection<CalendarEntry> calendarDayThursday;
        public ObservableCollection<CalendarEntry> calendarDayFriday;
        public ObservableCollection<CalendarEntry> calendarDaySaturday;

        public CalendarPageViewModel(User _user)
        {
            user = _user;
            Today = DateTime.Now;

            // Create the Calendar Lists
            calendarDaySunday = new ObservableCollection<CalendarEntry>();
            calendarDayMonday = new ObservableCollection<CalendarEntry>();
            calendarDayTuesday = new ObservableCollection<CalendarEntry>();
            calendarDayWednesday = new ObservableCollection<CalendarEntry>();
            calendarDayThursday = new ObservableCollection<CalendarEntry>();
            calendarDayFriday = new ObservableCollection<CalendarEntry>();
            calendarDaySaturday = new ObservableCollection<CalendarEntry>();
        }
        
        public void Dispose()
        {
            APIUnofficial.APICompletedEventHandler -= APIUnofficial_APICompletedEventHandler;
        }


        #region Message Boxes
        private async Task NotConnectedMessageBox()
        {
            Debug.WriteLine("NotConnectedMessageBox(): User is not connected");

            var messageDialog = new MessageDialog("You are required to be online to view the Calendar");
            messageDialog.Commands.Add(new UICommand("Ok", delegate(IUICommand command)
            {
                if (CalendarFailedToLoad != null)
                    CalendarFailedToLoad(this, null);
            }));

            messageDialog.DefaultCommandIndex = 0; // Set the command that will be invoked by default
            messageDialog.CancelCommandIndex = 0; // Set the command to be invoked when escape is pressed

            await messageDialog.ShowAsync();
        }

        private async Task ErrorGettingCalendarMessageBox()
        {
            Debug.WriteLine("ErrorGettingCalendarMessageBox(): Error Getting Calendar");

            var messageDialog = new MessageDialog("There was an error getting the Calendar. Please try again later");
            messageDialog.Commands.Add(new UICommand("Ok", delegate(IUICommand command)
                {
                    if (CalendarFailedToLoad != null)
                        CalendarFailedToLoad(this, null);
                })
            );

            messageDialog.DefaultCommandIndex = 0; // Set the command that will be invoked by default
            messageDialog.CancelCommandIndex = 0; // Set the command to be invoked when escape is pressed

            await messageDialog.ShowAsync();
        }
        #endregion

        public async void LoadCalendar(object ApplicationProgressBar, CoreDispatcher dispatcher)
        {
            XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

            if (Consts.IsConnectedToInternet())
            {
                Debug.WriteLine("LoadCalendar(): Loading Calendar");
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, true);

                try
                {
                    APIUnofficial.APICompletedEventHandler += APIUnofficial_APICompletedEventHandler;
                    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        APIUnofficial.Get.Calendar(user.Username);
                    });
                }
                catch (Exception)
                {
                    APIUnofficial.APICompletedEventHandler -= APIUnofficial_APICompletedEventHandler;
                    XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                }
            }
            else
            {
                XamlControlHelper.ChangeProgressIndicator(ApplicationProgressBar, false);
                await NotConnectedMessageBox();
            }
        }

        async void APIUnofficial_APICompletedEventHandler(object sender, APIs.Events.APICompletedEventArgs e)
        {
            if (e.Type != Data_Structures.APIType.CalendarInfo) return;
            APIUnofficial.APICompletedEventHandler -= APIUnofficial_APICompletedEventHandler;

            if (e.Result == Data_Structures.APIResponse.Successful)
            {
                // Fix the data to point to the actual Anime in our library
                List<CalendarEntry> rawCalendaryEntries = e.ResultObject as List<CalendarEntry>;
                for (int i = 0; i < rawCalendaryEntries.Count; i++)
                {
                    string title = rawCalendaryEntries[i].Anime.RomanjiTitle;
                    ObservableCollection<Anime> anime = user.animeLibrary.Search(title);

                    if (anime.Count > 0)
                    {
                        rawCalendaryEntries[i].Anime = anime[0];
                    }
                    else { rawCalendaryEntries[i].Anime.ServiceID = Helpers.ConvertToAPIConpliantString(title); }
                }

                // Filter it to content in the last week
                DateTime sixDaysMore = Today.AddDays(6);
                DateTime sixDaysLess = Today.AddDays(-6);
                List<CalendarEntry> secondStage = new List<CalendarEntry>();
                for (int i = 0; i < rawCalendaryEntries.Count; i++)
                {
                    if (rawCalendaryEntries[i].StartDate >= sixDaysLess &&
                        rawCalendaryEntries[i].StartDate <= sixDaysMore)
                    {
                        secondStage.Add(rawCalendaryEntries[i]);
                    }
                }

                // Remove Duplicates
                List<CalendarEntry> thirdStage = new List<CalendarEntry>();
                for (int i = 0; i < secondStage.Count; i++)
                {
                    int nextIndex = i + 1;
                    if (nextIndex >= secondStage.Count)
                    {
                        thirdStage.Add(secondStage[i]);
                        continue;
                    }
                    if (secondStage[i].Anime.RomanjiTitle != secondStage[nextIndex].Anime.RomanjiTitle)
                    {
                        thirdStage.Add(secondStage[i]);
                    }

                }

                // Finally, Parse and Send to the final list
                foreach (var entry in thirdStage)
                {
                    switch (entry.StartDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday: calendarDaySunday.Add(entry); break;
                        case DayOfWeek.Monday: calendarDayMonday.Add(entry); break;
                        case DayOfWeek.Tuesday: calendarDayTuesday.Add(entry); break;
                        case DayOfWeek.Wednesday: calendarDayWednesday.Add(entry); break;
                        case DayOfWeek.Thursday: calendarDayThursday.Add(entry); break;
                        case DayOfWeek.Friday: calendarDayFriday.Add(entry); break;
                        case DayOfWeek.Saturday: calendarDaySaturday.Add(entry); break;
                        default: break;
                    }
                }

                if (CalendarLoaded != null)
                    CalendarLoaded(this, null);
            }
            else
            {
                await ErrorGettingCalendarMessageBox();
            }
        }
    }
}
