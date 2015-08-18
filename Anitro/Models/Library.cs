using AnimeTrackingServiceWrapper.Helpers;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class Library : ModelBase
    {
        FilteredObservableCollection<LibraryObject> m_libraryCollection = new FilteredObservableCollection<LibraryObject>();
        public FilteredObservableCollection<LibraryObject> LibraryCollection
        {
            get { return m_libraryCollection; }
            set
            {
                if (m_libraryCollection == value) return;
                m_libraryCollection = value;
                RaisePropertyChanged(nameof(LibraryCollection));
            }
        }

        ObservableCollection<AnimeObject> m_recent = new ObservableCollection<AnimeObject>();
        public ObservableCollection<AnimeObject> Recent
        {
            get { return m_recent; }
            set
            {
                if (m_recent == value) return;
                m_recent = value;
                RaisePropertyChanged(nameof(Recent));
            }
        }

        ObservableCollection<AnimeObject> m_favourites = new ObservableCollection<AnimeObject>();
        public ObservableCollection<AnimeObject> Favourites
        {
            get { return m_favourites; }
            set
            {
                if (m_favourites == value) return;
                m_favourites = value;
                RaisePropertyChanged(nameof(Favourites));
            }
        }

        public Library()
        {
            LibraryCollection.CurrentFirstFilter = new LibrarySectionFilter<LibraryObject>(LibrarySection.All);
            LibraryCollection.FirstFilterSet.Add(LibraryCollection.CurrentFirstFilter);

            LibraryCollection.CurrentSecondFilter = new SearchLibraryFilter<LibraryObject>();
            LibraryCollection.SecondFilterSet.Add(LibraryCollection.CurrentSecondFilter);
        }
    }
}
