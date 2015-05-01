using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.Structures
{
    public class LibraryPageParameter
    {
        public User user;
        public LibraryType libraryType;

        public LibraryPageParameter(User _user, LibraryType _libraryType)
        {
            user = _user;
            libraryType = _libraryType;
        }
    }
}
