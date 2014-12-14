using System;
using System.Collections.Generic;
using System.Text;
using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.Structures
{
    public class SearchPageParameter
    {
        public string searchTerm;
        public SearchFilter searchType;

        public SearchPageParameter(string _searchTerm)
        {
            searchTerm = _searchTerm;
            searchType = SearchFilter.Everything;
        }

        public SearchPageParameter(string _searchTerm, SearchFilter _searchType)
        {
            searchTerm = _searchTerm;
            searchType = _searchType;
        }

        public override string ToString()
        {
            return searchTerm + " | " + searchType;
        }
    }
}
