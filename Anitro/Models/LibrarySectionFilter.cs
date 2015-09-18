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
    public class LibrarySectionFilter: ModelBase
    {
        private LibrarySection m_librarySelection = LibrarySection.All;
        public LibrarySection LibrarySelection
        {
            get { return m_librarySelection; }
            set
            {
                //if (m_parameter == value) return;
                m_librarySelection = value;
                RaisePropertyChanged(nameof(LibrarySelection));
            }
        }

        public LibrarySectionFilter() { }
        public LibrarySectionFilter(LibrarySection section)
        {
            LibrarySelection = section;
        }

        public ObservableCollection<LibraryObject> Filter(ObservableCollection<LibraryObject> collection)
        {
            // Check for All or None
            if (LibrarySelection == LibrarySection.All)
                return collection;
            else if (LibrarySelection == LibrarySection.None)
                return new ObservableCollection<LibraryObject>();

            // Filter the Collection
            ObservableCollection<LibraryObject> filteredCollection = new ObservableCollection<LibraryObject>();
            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].Section == LibrarySection.None) continue;
                if (collection[i].Section == LibrarySelection)
                    filteredCollection.Add(collection[i]);
            }

            return filteredCollection;
        }
    }
}
