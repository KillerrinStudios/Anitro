using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using Anitro.Data_Structures;
using Anitro.Data_Structures.API_Classes;


namespace Anitro.Data_Structures.Structures
{
    public class AnimePageParameter
    {
        public enum ComingFrom
        {
            Tile,
            Protocol,
            Voice,
            ActivityFeed,
            Search,
            Recent,
            LibraryPage,
            Favourites
        }

        public string slug;
        public LibraryObject libraryObject;
        public ComingFrom comingFrom;

        public AnimePageParameter(LibraryObject libObj, ComingFrom _comingFrom)
        {
            slug = libraryObject.Anime.ServiceID;
            libraryObject = libObj;
            comingFrom = _comingFrom;
        }
        public AnimePageParameter(string _slug, ComingFrom _comingFrom)
        {
            slug = _slug;
            libraryObject = null;
            comingFrom = _comingFrom;
        }

        public override string ToString()
        {
            return slug + " | " + comingFrom.ToString();
        }
    }
}
