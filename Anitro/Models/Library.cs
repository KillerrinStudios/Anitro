using AnimeTrackingServiceWrapper.Helpers;
using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Newtonsoft.Json;
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
        private FilteredLibraryObservableCollection m_libraryCollection = new FilteredLibraryObservableCollection();
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

        private ObservableCollection<AnimeObject> m_recent = new ObservableCollection<AnimeObject>();
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

        private ObservableCollection<AnimeObject> m_favourites = new ObservableCollection<AnimeObject>();
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

        public void AddToRecent(AnimeObject anime)
        {
            if (Recent.Contains(anime) == false)
            {
                Recent.Insert(0, anime);
            }

            while (Recent.Count > 6)
            {
                Recent.RemoveAt(Recent.Count - 1);
            }
        }

        public bool IsInLibrary(ServiceID id)
        {
            foreach (var item in LibraryCollection.UnfilteredCollection)
            {
                if (id.ID == item.Anime.ID.ID)
                    return true;
            }
            return false;
        }

        public bool IsFavourited(AnimeObject anime)
        {
            foreach (var favourite in Favourites)
            {
                if (favourite.DoesNameFitSearch(anime.ID.ID))
                    return true;
                if (favourite.DoesNameFitSearch(anime.RomanjiTitle))
                    return true;
            }
            return false;
        }

        public LibraryObject FindInLibrary(ServiceID id)
        {
            foreach (var item in LibraryCollection.UnfilteredCollection)
            {
                if (id.ID == item.Anime.ID.ID)
                    return item;
            }
            return null;
        }

        public LibraryObject SelectRandomTitle()
        {
            int count = LibraryCollection.UnfilteredCollection.Count;
            if (count == 0) return null;

            Random random = new Random();
            return LibraryCollection.UnfilteredCollection[random.Next(count) - 1];
        }
    }
}
