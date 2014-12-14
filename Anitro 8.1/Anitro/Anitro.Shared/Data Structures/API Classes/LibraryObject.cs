using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes
{
    public class LibraryObject
    {
        public string episodes_watched { get; set; } // int
        public string last_watched { get; set; }
        public string rewatched_times { get; set; } // int
        public object notes { get; set; } //string
        public object notes_present { get; set; } //bool
        public string status { get; set; }
        public string id { get; set; }
        public bool @private { get; set; }
        public object rewatching { get; set; } //bool
        public Anime anime { get; set; }
        public Rating rating { get; set; }

        public void IncrimentEpisodesWatched(int ammount = 1)
        {
            int watchedCount = Convert.ToInt32(episodes_watched);
            int epCount;

            string epCountAsStr = anime.episode_count;
            if (string.IsNullOrEmpty(epCountAsStr) || epCountAsStr.Contains("?") || epCountAsStr.StartsWith("0")) epCount = 0;
            else { epCount = Convert.ToInt32(anime.episode_count); }


            int newWatchedCount;
            if ((watchedCount + ammount) >= epCount) newWatchedCount = epCount;
            else newWatchedCount = watchedCount + ammount;

            episodes_watched = Convert.ToString(newWatchedCount);
        }
        public void DecrementEpisodesWatched(int ammount = 1)
        {
            int watchedCount = Convert.ToInt32(episodes_watched);
            int epCount;

            string epCountAsStr = anime.episode_count;
            if (string.IsNullOrEmpty(epCountAsStr) || epCountAsStr.Contains("?") || epCountAsStr.StartsWith("0")) epCount = 0;
            else { epCount = Convert.ToInt32(anime.episode_count); }


            int newWatchedCount;
            if ((watchedCount - ammount) <= 0) newWatchedCount = 0;
            else newWatchedCount = watchedCount - ammount;

            episodes_watched = Convert.ToString(newWatchedCount);
        }

        public void IncrimentRewatchedCount(int ammount = 1)
        {
            int rewatchedTimes = Convert.ToInt32(rewatched_times);
            rewatched_times = Convert.ToString(rewatchedTimes + ammount);
        }
        public void DecrimentRewatchedCount(int ammount = 1)
        {
            int rewatchedTimes = Convert.ToInt32(rewatched_times);

            if (rewatchedTimes <= 0) { return; }

            int newWatchedCount;
            if ((rewatchedTimes - ammount) <= 0) newWatchedCount = 0;
            else newWatchedCount = rewatchedTimes - ammount;

            rewatched_times = Convert.ToString(newWatchedCount);
        }

        public LibraryObject(Anime _anime)
        {
            episodes_watched = "0"; 
            last_watched = "";
            rewatched_times = "0";
            notes = "";
            notes_present = false;
            status = "";
            id = "";
            @private = false;
            rewatching = false;
            anime = _anime;
            rating = new Rating
            {
                type = "",
                value = "0.0",
                valueAsDouble = 0.0,
            };
        }
    }
}
