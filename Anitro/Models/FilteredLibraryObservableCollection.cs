using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.ViewModels;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class FilteredLibraryObservableCollection : ModelBase
    {

        #region Collection
        ObservableCollection<LibraryObject> m_unfilteredCollection = new ObservableCollection<LibraryObject>();
        public ObservableCollection<LibraryObject> UnfilteredCollection
        {
            get { return m_unfilteredCollection; }
            set
            {
                if (m_unfilteredCollection == value) return;

                m_unfilteredCollection = value;
                m_unfilteredCollection.CollectionChanged += M_unfilteredCollection_CollectionChanged;

                RaisePropertyChanged(nameof(UnfilteredCollection));

                ApplyFilters();
            }
        }

        ObservableCollection<LibraryObject> m_filteredCollection = new ObservableCollection<LibraryObject>();
        public ObservableCollection<LibraryObject> FilteredCollection
        {
            get { return m_filteredCollection; }
            protected set
            {
                if (m_filteredCollection == value) return;
                m_filteredCollection = value;
                RaisePropertyChanged(nameof(FilteredCollection));
            }
        }
        #endregion

        #region Filters
        LibrarySectionFilter m_librarySectionFilter = new LibrarySectionFilter(LibrarySection.All);
        public LibrarySectionFilter LibrarySelectionFilter
        {
            get { return m_librarySectionFilter; }
            set
            {
                if (m_librarySectionFilter == value) return;
                m_librarySectionFilter = value;
                RaisePropertyChanged(nameof(LibrarySelectionFilter));

                m_librarySectionFilter.PropertyChanged += Filters_PropertyChanged;
            }
        }

        SearchLibraryFilter m_searchLibraryFilter = new SearchLibraryFilter("");
        public SearchLibraryFilter SearchFilter
        {
            get { return m_searchLibraryFilter; }
            set
            {
                if (m_searchLibraryFilter == value) return;
                m_searchLibraryFilter = value;
                RaisePropertyChanged(nameof(SearchFilter));

                m_searchLibraryFilter.PropertyChanged += Filters_PropertyChanged;
            }
        }
        #endregion

        #region Constructors
        public FilteredLibraryObservableCollection()
        {
            AddDefaultFilters();
        }
        public FilteredLibraryObservableCollection(IList<LibraryObject> list)
        {
            foreach (var i in list)
            {
                UnfilteredCollection.Add(i);
            }

            AddDefaultFilters();
        }

        private void AddDefaultFilters()
        {
            ApplyFilters();

            // Apply the Property Changed to the defaults
            m_librarySectionFilter.PropertyChanged += Filters_PropertyChanged;
            m_searchLibraryFilter.PropertyChanged += Filters_PropertyChanged;

            // Take away, and give back the Collection Changed
            m_unfilteredCollection.CollectionChanged -= M_unfilteredCollection_CollectionChanged;
            m_unfilteredCollection.CollectionChanged += M_unfilteredCollection_CollectionChanged;
        }
        #endregion


        private void Filters_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void M_unfilteredCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        public RelayCommand ApplyFilterCommand { get { return new RelayCommand(() => { ApplyFilters(); }); } }
        public void ApplyFilters()
        {
            Debug.WriteLine("Applying Filters");
            ObservableCollection<LibraryObject> filtered = new ObservableCollection<LibraryObject>(UnfilteredCollection);

            if (LibrarySelectionFilter != null)
                filtered = LibrarySelectionFilter.Filter(filtered);
            if (SearchFilter != null)
                filtered = SearchFilter.Filter(filtered);

            FilteredCollection = filtered;
        }
    }
}
