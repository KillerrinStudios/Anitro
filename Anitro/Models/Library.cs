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

        #region Add/Remove
        public void AddToLibrary(LibraryObject libraryObject)
        {
            LibraryCollection.UnfilteredCollection.Insert(0, libraryObject);
        }
        public void RemoveFromLibrary(ServiceID id)
        {
            for(int i = LibraryCollection.UnfilteredCount - 1; i > 0; i--)
            {
                if (LibraryCollection.UnfilteredCollection[i].Anime.ID == id)
                    LibraryCollection.UnfilteredCollection.RemoveAt(i);
            }
        }

        public void AddToRecent(AnimeObject anime)
        {
            bool notInRecent = true;
            foreach (var recentItem in Recent)
            {
                if (recentItem.ID.ID == anime.ID.ID)
                {
                    Recent.Remove(recentItem);
                    Recent.Insert(0, recentItem);

                    notInRecent = false;
                    break;
                }
            }

            if (notInRecent)
                Recent.Insert(0, anime);

            while (Recent.Count > 6)
            {
                Recent.RemoveAt(Recent.Count - 1);
            }
        }
        #endregion

        #region Search
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
        #endregion

        public LibraryObject SelectRandomTitle()
        {
            int count = LibraryCollection.UnfilteredCollection.Count;
            if (count == 0) return null;

            Random random = new Random();
            return LibraryCollection.UnfilteredCollection[random.Next(count) - 1];
        }
    }
}
