using AnimeTrackingServiceWrapper;
using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.Universal_Service_Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Helpers;
using Anitro.Models;
using Anitro.Models.Enums;
using Anitro.Pages.Hummingbird;
using Anitro.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Anitro.ViewModels.Hummingbird
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HummingbirdCalendarViewModel : AnitroViewModelBase
    {
        private HummingbirdUser m_user = new HummingbirdUser();
        public HummingbirdUser User
        {
            get { return m_user; }
            set
            {
                if (m_user == value) return;
                m_user = value;
                RaisePropertyChanged(nameof(User));
            }
        }

        #region Headers
        private string m_sundayHeader = "Sunday";
        public string SundayHeader
        {
            get { return m_sundayHeader; }
            set
            {
                if (m_sundayHeader == value) return;
                m_sundayHeader = value;
                RaisePropertyChanged(nameof(SundayHeader));
            }
        }

        private string m_mondayHeader = "Monday";
        public string MondayHeader
        {
            get { return m_mondayHeader; }
            set
            {
                if (m_mondayHeader == value) return;
                m_mondayHeader = value;
                RaisePropertyChanged(nameof(MondayHeader));
            }
        }

        private string m_tuesdayHeader = "Tuesday";
        public string TuesdayHeader
        {
            get { return m_tuesdayHeader; }
            set
            {
                if (m_tuesdayHeader == value) return;
                m_tuesdayHeader = value;
                RaisePropertyChanged(nameof(TuesdayHeader));
            }
        }

        private string m_wednesdayHeader = "Wednesday";
        public string WednesdayHeader
        {
            get { return m_wednesdayHeader; }
            set
            {
                if (m_wednesdayHeader == value) return;
                m_wednesdayHeader = value;
                RaisePropertyChanged(nameof(WednesdayHeader));
            }
        }

        private string m_thursdayHeader = "Thursday";
        public string ThursdayHeader
        {
            get { return m_thursdayHeader; }
            set
            {
                if (m_thursdayHeader == value) return;
                m_thursdayHeader = value;
                RaisePropertyChanged(nameof(ThursdayHeader));
            }
        }

        private string m_fridayHeader = "Friday";
        public string FridayHeader
        {
            get { return m_fridayHeader; }
            set
            {
                if (m_fridayHeader == value) return;
                m_fridayHeader = value;
                RaisePropertyChanged(nameof(FridayHeader));
            }
        }

        private string m_saturdayHeader = "Saturday";
        public string SaturdayHeader
        {
            get { return m_saturdayHeader; }
            set
            {
                if (m_saturdayHeader == value) return;
                m_saturdayHeader = value;
                RaisePropertyChanged(nameof(SaturdayHeader));
            }
        }
        #endregion

        #region Expanders
        public bool IsSunday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Sunday; } }
        public bool IsMonday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Monday; } }
        public bool IsTuesday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Tuesday; } }
        public bool IsWednesday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Wednesday; } }
        public bool IsThursday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Thursday; } }
        public bool IsFriday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Friday; } }
        public bool IsSaturday { get { return DateTime.Now.DayOfWeek == DayOfWeek.Saturday; } }
        #endregion

        #region Dates
        public DateTime Sunday;
        public DateTime Monday;
        public DateTime Tuesday;
        public DateTime Wednesday;
        public DateTime Thursday;
        public DateTime Friday;
        public DateTime Saturday;
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public HummingbirdCalendarViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                User = new HummingbirdUser();
                User.UserInfo.Username = "Design Time";
                User.UserInfo.AvatarUrl = new System.Uri("https://static.hummingbird.me/users/avatars/000/007/415/thumb/TyrilCropped1.png?1401236074", System.UriKind.Absolute);
                User.HummingbirdUserInfo.cover_image = "https://static.hummingbird.me/users/cover_images/000/007/415/thumb/Zamma_resiz.jpg?1401237213";

                User.LoginInfo.Username = "Design Time";
                User.LoginInfo.AuthToken = "AuthToken";

                User.Calendar.Add(new CalendarEntry(DateTime.Now, "Entry 1"));
                User.Calendar.Add(new CalendarEntry(DateTime.Now, "Entry 2"));
                User.Calendar.Add(new CalendarEntry(DateTime.Now, "Entry 3"));
                User.Calendar.Add(new CalendarEntry(DateTime.Now, "Entry 4"));
            }
            else
            {
                // Code runs "for real"
            }
        }

        public override void Loaded()
        {

        }

        public override void OnNavigatedTo()
        {
            MainViewModel.Instance.CurrentNavigationLocation = NavigationLocation.Calendar;
            SetDate(DateTime.Now);
        }

        public override void OnNavigatedFrom()
        {

        }

        public override void ResetViewModel()
        {

        }


        #region Set Date Command
        public RelayCommand<DateTime> SetDateCommand
        {
            get
            {
                return new RelayCommand<DateTime>((date) =>
                {
                    SetDate(date);
                });
            }
        }
        public void SetDate(DateTime date)
        {
            Debug.WriteLine("Setting Date: " + date.ToString());
            DateTime startOfWeek = date.StartOfWeek(DayOfWeek.Sunday);
                                                                                                Sunday = startOfWeek;
            SundayHeader    += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Monday = startOfWeek;
            MondayHeader    += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Tuesday = startOfWeek;
            TuesdayHeader   += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Wednesday = startOfWeek;
            WednesdayHeader += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Thursday = startOfWeek;
            ThursdayHeader  += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Friday = startOfWeek;
            FridayHeader    += " " + startOfWeek.Day;   startOfWeek = startOfWeek.AddDays(1.0); Saturday = startOfWeek;
            SaturdayHeader  += " " + startOfWeek.Day;

            RefreshCalendar();
        }
        #endregion

        #region Refresh Calendar
        public RelayCommand RefreshCalendarCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RefreshCalendar();
                });
            }
        }

        public void RefreshCalendar()
        {
            Debug.WriteLine("Refreshing Calendar");
            Progress<APIProgressReport> m_refreshCalendarProgress = new Progress<APIProgressReport>();
            m_refreshCalendarProgress.ProgressChanged += M_refreshCalendarProgress_ProgressChanged;
            APIServiceCollection.Instance.HummingbirdV1API.CalendarAPI.GetCalendar(User.UserInfo.Username, m_refreshCalendarProgress);
        }

        private void M_refreshCalendarProgress_ProgressChanged(object sender, APIProgressReport e)
        {
            ProgressService.SetIndicatorAndShow(true, e.Percentage, e.StatusMessage, false);
            if (e.CurrentAPIResonse == APIResponse.Successful)
            {
                ProgressService.Reset();
                CalendarRootHummingbird_Undocumented raw = (CalendarRootHummingbird_Undocumented)e.Parameter.Raw;

                User.Calendar.Unfiltered.Clear();
                List<CalendarEntry> calendarEntries = (List<CalendarEntry>)e.Parameter.Converted;

                foreach (var entry in calendarEntries)
                {
                    //Debug.WriteLine(entry);

                    if (entry.Date >= Sunday && 
                        entry.Date <= Saturday)
                    {
                        User.Calendar.Add(entry);
                    }
                }
            }
            else if (APIResponseHelpers.IsAPIResponseFailed(e.CurrentAPIResonse))
            {
                ProgressService.Reset();
            }
        }
        #endregion

        #region Calendar Item Clicked Command
        public RelayCommand<object> CalendarItemClickedCommand
        {
            get
            {
                return new RelayCommand<object>((item) =>
                {
                    CalendarItemClicked(item);
                });
            }
        }

        public void CalendarItemClicked(object item)
        {
            Debug.WriteLine("Calendar Item Clicked");
            if (item == null) return;

            CalendarEntry entry = item as CalendarEntry;
            AnimeObject anime = GetAnimeFromCalendarEntry(entry);

            if (anime == null) return;
            ViewModelLocator.Instance.vm_HummingbirdAnimeLibraryViewModel.NavigateAnimeDetailsPage(anime);
        }

        public AnimeObject GetAnimeFromCalendarEntry(CalendarEntry entry)
        {
            if (entry == null) return null;

            foreach (var libraryObject in User.AnimeLibrary.LibraryCollection.UnfilteredCollection)
            {
                if (libraryObject.Anime.DoesNameFitSearch(entry.Title))
                    return libraryObject.Anime;
            }

            return null;
        }
        #endregion

    }
}