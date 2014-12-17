using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures
{
    public enum LibrarySelection
    {
        // Program Specific
        None,
        All,
        APISupported,

        // Anime
        CurrentlyWatching,
        PlanToWatch,

        // Manga
        CurrentlyReading,
        PlanToRead,

        // Universal
        Completed,
        OnHold,
        Dropped,

        // Extra
        Favourites,
        Recent,
        Search,
    }
}
