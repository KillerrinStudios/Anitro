using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.ViewModels;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
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
        private ObservableCollection<LibraryObject> m_unfilteredCollection = new ObservableCollection<LibraryObject>();
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

        private ObservableCollection<LibraryObject> m_filteredCollection = new ObservableCollection<LibraryObject>();
        [JsonIgnore]
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
        private LibrarySectionFilter m_librarySectionFilter = new LibrarySectionFilter(LibrarySection.All);
        [JsonIgnore]
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

        private SearchLibraryFilter m_searchLibraryFilter = new SearchLibraryFilter("");
        [JsonIgnore]
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

        #region Filters
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

        private void Filters_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void M_unfilteredCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        //public RelayCommand ApplyFilterCommand { get { return new RelayCommand(() => { ApplyFilters(); }); } }
        public void ApplyFilters()
        {
            //Debug.WriteLine("Applying Filters");
            ObservableCollection<LibraryObject> filtered = new ObservableCollection<LibraryObject>(UnfilteredCollection);

            if (LibrarySelectionFilter != null)
                filtered = LibrarySelectionFilter.Filter(filtered);
            if (SearchFilter != null)
                filtered = SearchFilter.Filter(filtered);

            FilteredCollection = filtered;

            // Calculate Charts
            Progress<bool> chartProgress = new Progress<bool>();
            chartProgress.ProgressChanged += ChartProgress_ProgressChanged;
            CalculateTotalGenres(chartProgress);
        }

        #endregion

        [JsonIgnore]
        public int UnfilteredCount { get { return UnfilteredCollection.Count; } }
        [JsonIgnore]
        public int FilteredCount { get { return FilteredCollection.Count; } }

        #region Chart Helpers
        [JsonIgnore]
        private List<StatisticsChartModel> m_libraryGenres = new List<StatisticsChartModel>();
        [JsonIgnore]
        public List<StatisticsChartModel> LibraryGenres
        {
            get { return m_libraryGenres; }
            set
            {
                if (m_libraryGenres == value) return;
                m_libraryGenres = value;
                RaisePropertyChanged(nameof(LibraryGenres));
            }
        }
        public async Task CalculateTotalGenres(IProgress<bool> progress)
        {
            m_libraryGenres.Clear();
            foreach (var lO in UnfilteredCollection)
            {
                foreach (var genre in lO.Anime.Genres)
                {
                    bool counted = false;
                    foreach (var genreCount in m_libraryGenres)
                    {
                        if (genreCount.Name == genre.ToString())
                        {
                            genreCount.Amount++;
                            counted = true;
                            break;
                        }
                    }

                    if (!counted)
                        m_libraryGenres.Add(new StatisticsChartModel(genre.ToString(), 1));
                }
            }

            m_libraryGenres = m_libraryGenres.OrderByDescending(o => o.Amount).ToList();

            int max = 10;
            if (m_libraryGenres.Count > max)
            {
                m_libraryGenres.RemoveRange(max - 1, m_libraryGenres.Count - max);
            }

            //// Remove all with 0 Amount
            //for (int i = m_libraryGenres.Count - 1; i > 0; i--)
            //{
            //    if (m_libraryGenres[i].Amount == 0) m_libraryGenres.RemoveAt(i);
            //}

            progress.Report(true);
        }

        private void ChartProgress_ProgressChanged(object sender, bool e)
        {
            RaisePropertyChanged(nameof(LibraryGenres));
        }
        #endregion
    }
}
