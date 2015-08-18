using AnimeTrackingServiceWrapper.UniversalServiceModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Interfaces
{
    public class SearchLibraryFilter<T> : IFilter<T> where T : LibraryObject
    {
        private string m_name = "Search";
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name == value) return;
                m_name = value;
            }
        }

        private object m_parameter = new object();
        public object Parameter
        {
            get { return m_parameter; }
            set
            {
                if (m_parameter == value) return;
                m_parameter = value;
            }
        }

        public ObservableCollection<T> Filter(ObservableCollection<T> collection)
        {
            if (Parameter == null)
                return collection;
            if (!(Parameter is string))
                return collection;

            ObservableCollection<T> filteredItems = new ObservableCollection<T>();
            string parameter = Parameter as string;
            if (string.IsNullOrWhiteSpace(parameter))
                return collection;

            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (CheckQuery(collection[i], parameter))
                    filteredItems.Add(collection[i]);
            }

            return filteredItems;
        }

        public bool CheckQuery(T item, string parameter)
        {
            if (item.Anime.EnglishTitle.Contains(parameter))
                return true;
            if (item.Anime.RomanjiTitle.Contains(parameter))
                return true;
            if (item.Anime.KanjiTitle.Contains(parameter))
                return true;

            if (item.Anime.ID.ID.Contains(parameter))
                return true;
            if (item.Anime.ID2.ID.Contains(parameter))
                return true;

            foreach (var id in item.Anime.AlternateIDs)
            {
                if (id.ID.Contains(parameter))
                    return true;
            }

            return false;
        }
    }
}
