using AnimeTrackingServiceWrapper.UniversalServiceModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Interfaces
{
    public class LibrarySectionFilter<T> : IFilter<T> where T : LibraryObject
    {
        private string m_name = "Library Section";
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

        public LibrarySectionFilter(LibrarySection section)
        {
            Parameter = section;
        }

        public ObservableCollection<T> Filter(ObservableCollection<T> collection)
        {
            if (Parameter == null ||
                !(Parameter is LibrarySection))
                return collection;

            // Check for All or None
            LibrarySection librarySection = (LibrarySection)Parameter;
            if (librarySection == LibrarySection.All)
                return collection;
            else if (librarySection == LibrarySection.None)
                return new ObservableCollection<T>();

            // Filter the Collection
            for (int i = collection.Count - 1; i >= 0 ; i--)
            {
                if (collection[i].Section != librarySection)
                {
                    collection.RemoveAt(i);
                }
            }

            return collection;
        }
    }
}
