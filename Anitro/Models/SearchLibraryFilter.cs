using AnimeTrackingServiceWrapper.UniversalServiceModels;
using Anitro.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class SearchLibraryFilter : ModelBase
    {
        private string m_searchTerm = "";
        public string SearchTerm
        {
            get { return m_searchTerm; }
            set
            {
                //if (m_parameter == value) return;
                m_searchTerm = value;
                RaisePropertyChanged(nameof(SearchTerm));
            }
        }

        public SearchLibraryFilter() { }
        public SearchLibraryFilter(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public ObservableCollection<LibraryObject> Filter(ObservableCollection<LibraryObject> collection)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return collection;

            ObservableCollection<LibraryObject> filteredItems = new ObservableCollection<LibraryObject>();

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Anime.DoesNameFitSearch(SearchTerm))
                    filteredItems.Add(collection[i]);
            }

            return filteredItems;
        }
    }
}
