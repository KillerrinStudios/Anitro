using AnimeTrackingServiceWrapper.Helpers;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
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
        FilteredLibraryObservableCollection m_libraryCollection = new FilteredLibraryObservableCollection();
        public FilteredLibraryObservableCollection LibraryCollection
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
        }


        public bool IsInLibrary(AnimeObject anime)
        {
            foreach (var item in LibraryCollection.UnfilteredCollection)
            {
                if (anime.ID.ID == item.Anime.ID.ID)
                    return true;
            }
            return false;
        }

        public LibraryObject FindInLibrary(AnimeObject anime)
        {
            foreach (var item in LibraryCollection.UnfilteredCollection)
            {
                if (anime.ID.ID == item.Anime.ID.ID)
                    return item;
            }
            return null;
        }
    }
}
