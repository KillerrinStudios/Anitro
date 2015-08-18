using Anitro.Interfaces;
using Anitro.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class FilteredObservableCollection<T> : ModelBase
    {
        ObservableCollection<T> m_unfilteredCollection = new ObservableCollection<T>();
        public ObservableCollection<T> UnfilteredCollection
        {
            get { return m_unfilteredCollection; }
            set
            {
                if (m_unfilteredCollection == value) return;
                m_unfilteredCollection = value;
                RaisePropertyChanged(nameof(UnfilteredCollection));

                ApplyFilters();
            }
        }

        ObservableCollection<T> m_filteredCollection = new ObservableCollection<T>();
        public ObservableCollection<T> FilteredCollection
        {
            get { return m_filteredCollection; }
            protected set
            {
                if (m_filteredCollection == value) return;
                m_filteredCollection = value;
                RaisePropertyChanged(nameof(FilteredCollection));
            }
        }

        #region First Filter
        IFilter<T> m_currentFirstFilter = new NoFilter<T>();
        public IFilter<T> CurrentFirstFilter
        {
            get { return m_currentFirstFilter; }
            set
            {
                if (m_currentFirstFilter == value) return;
                m_currentFirstFilter = value;
                RaisePropertyChanged(nameof(CurrentFirstFilter));

                ApplyFilters();
            }
        }

        ObservableCollection<IFilter<T>> m_firstFilterSet = new ObservableCollection<IFilter<T>>();
        public ObservableCollection<IFilter<T>> FirstFilterSet
        {
            get { return m_firstFilterSet; }
            set
            {
                if (m_firstFilterSet == value) return;
                m_firstFilterSet = value;
                RaisePropertyChanged(nameof(FirstFilterSet));
            }
        }
        #endregion

        #region Second Filter
        IFilter<T> m_currentSecondFilter = new NoFilter<T>();
        public IFilter<T> CurrentSecondFilter
        {
            get { return m_currentSecondFilter; }
            set
            {
                if (m_currentSecondFilter == value) return;
                m_currentSecondFilter = value;
                RaisePropertyChanged(nameof(CurrentSecondFilter));

                ApplyFilters();
            }
        }

        ObservableCollection<IFilter<T>> m_secondFilterSet = new ObservableCollection<IFilter<T>>();
        public ObservableCollection<IFilter<T>> SecondFilterSet
        {
            get { return m_secondFilterSet; }
            set
            {
                if (m_secondFilterSet == value) return;
                m_secondFilterSet = value;
                RaisePropertyChanged(nameof(SecondFilterSet));
            }
        }
        #endregion

        public FilteredObservableCollection()
        {
            AddDefaultFilters();
        }
        public FilteredObservableCollection(IList<T> list)
        {
            foreach (var i in list)
            {
                UnfilteredCollection.Add(i);
            }

            AddDefaultFilters();
        }

        private void AddDefaultFilters()
        {
            FirstFilterSet.Add(CurrentFirstFilter);
            SecondFilterSet.Add(CurrentSecondFilter);

            ApplyFilters();
        }


        public RelayCommand ApplyFilterCommand { get { return new RelayCommand(() => { ApplyFilters(); }); } }
        public void ApplyFilters()
        {
            ObservableCollection<T> filtered = new ObservableCollection<T>(UnfilteredCollection);

            filtered = CurrentFirstFilter.Filter(filtered);
            filtered = CurrentSecondFilter.Filter(filtered);

            //FilteredCollection.Clear();
            FilteredCollection = filtered;
        }
    }
}
