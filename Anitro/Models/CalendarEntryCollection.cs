using AnimeTrackingServiceWrapper.Universal_Service_Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class CalendarEntryCollection : ModelBase
    {
        #region Properties
        private ObservableCollection<CalendarEntry> m_unfiltered = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Unfiltered
        {
            get { return m_unfiltered; }
            set
            {
                m_unfiltered = value;
                m_unfiltered.CollectionChanged += M_unfiltered_CollectionChanged;

                RaisePropertyChanged(nameof(Unfiltered));
                FilterCalendaryEntries();
            }
        }

        private void M_unfiltered_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FilterCalendaryEntries();
        }

        #region Day Collections
        private ObservableCollection<CalendarEntry> m_sunday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Sunday
        {
            get { return m_sunday; }
            protected set
            {
                m_sunday = value;
                RaisePropertyChanged(nameof(Sunday));
            }
        }

        private ObservableCollection<CalendarEntry> m_monday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Monday
        {
            get { return m_monday; }
            protected set
            {
                m_monday = value;
                RaisePropertyChanged(nameof(Monday));
            }
        }

        private ObservableCollection<CalendarEntry> m_tuesday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Tuesday
        {
            get { return m_tuesday; }
            protected set
            {
                m_tuesday = value;
                RaisePropertyChanged(nameof(Tuesday));
            }
        }

        private ObservableCollection<CalendarEntry> m_wednesday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Wednesday
        {
            get { return m_wednesday; }
            protected set
            {
                m_wednesday = value;
                RaisePropertyChanged(nameof(Wednesday));
            }
        }

        private ObservableCollection<CalendarEntry> m_thursday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Thursday
        {
            get { return m_thursday; }
            protected set
            {
                m_thursday = value;
                RaisePropertyChanged(nameof(Thursday));
            }
        }

        private ObservableCollection<CalendarEntry> m_friday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Friday
        {
            get { return m_friday; }
            protected set
            {
                m_friday = value;
                RaisePropertyChanged(nameof(Friday));
            }
        }

        private ObservableCollection<CalendarEntry> m_saturday = new ObservableCollection<CalendarEntry>();
        public ObservableCollection<CalendarEntry> Saturday
        {
            get { return m_saturday; }
            protected set
            {
                m_saturday = value;
                RaisePropertyChanged(nameof(Saturday));
            }
        }
        #endregion
        #endregion

        public CalendarEntryCollection()
        {
            Unfiltered.CollectionChanged += M_unfiltered_CollectionChanged;
        }
        public CalendarEntryCollection(IList<CalendarEntry> list)
        {
            Unfiltered = new ObservableCollection<CalendarEntry>(list);
            Unfiltered.CollectionChanged += M_unfiltered_CollectionChanged;
        }

        public void FilterCalendaryEntries()
        {
            Sunday.Clear();
            Monday.Clear();
            Tuesday.Clear();
            Wednesday.Clear();
            Thursday.Clear();
            Friday.Clear();
            Saturday.Clear();

            foreach (var entry in Unfiltered)
            {
                switch (entry.Date.DayOfWeek)
                {
                    case DayOfWeek.Sunday:      Sunday.Add(entry); break;
                    case DayOfWeek.Monday:      Monday.Add(entry);break;
                    case DayOfWeek.Tuesday:     Tuesday.Add(entry);break;
                    case DayOfWeek.Wednesday:   Wednesday.Add(entry);break;
                    case DayOfWeek.Thursday:    Thursday.Add(entry);break;
                    case DayOfWeek.Friday:      Friday.Add(entry);break;
                    case DayOfWeek.Saturday:    Saturday.Add(entry);break;
                    default: break;
                }
            }
        }

        public void Add(CalendarEntry entry)
        {
            Unfiltered.Add(entry);
            FilterCalendaryEntries();
        }

        public void Clear()
        {
            Unfiltered.Clear();
            FilterCalendaryEntries();
        }
    }
}
