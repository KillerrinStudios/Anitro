using System;
using System.Collections.Generic;
using System.Text;
using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.API_Classes
{
    public class LibraryObject
    {
        #region Properties
        public int RewatchedTimes { get; set; } 
        public int EpisodesWatched { get; set; }
        public string EpisodesWatchedString
        {
            get
            {
                //if (EpisodesWatched == 0) return "?";
                return EpisodesWatched.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { EpisodesWatched = 0; }
                else if (value.Contains("?")) { EpisodesWatched = 0; }
                else {
                    try { EpisodesWatched = Convert.ToInt32(value); }
                    catch (Exception) { EpisodesWatched = 0; }
                }
            }
        }

        public PrivacySettings Private { get; set; }
        public bool Rewatching { get; set; }

        public double Rating { get; set; }

        public string Notes { get; set; }

        public DateTime LastWatched { get; set; }
        public LibrarySelection Status { get; set; }

        public Anime Anime { get; set; }
        #endregion

        #region Helper Methods
        public void IncrimentEpisodesWatched(int ammount = 1)
        {
            int watchedCount = EpisodesWatched;
            int epCount = Anime.EpisodeCount;

            int newWatchedCount;
            if ((watchedCount + ammount) >= epCount) newWatchedCount = epCount;
            else newWatchedCount = watchedCount + ammount;

            EpisodesWatched = newWatchedCount;
        }
        public void DecrementEpisodesWatched(int ammount = 1)
        {
            int watchedCount = EpisodesWatched;
            int epCount = Anime.EpisodeCount;

            int newWatchedCount;
            if ((watchedCount - ammount) <= 0) newWatchedCount = 0;
            else newWatchedCount = watchedCount - ammount;

            EpisodesWatched = newWatchedCount;
        }

        public void IncrimentRewatchedCount(int ammount = 1)
        {
            RewatchedTimes += ammount;
        }
        public void DecrimentRewatchedCount(int ammount = 1)
        {
            RewatchedTimes -= ammount;
        }
        #endregion

        #region Constructors
        public LibraryObject(Anitro.Data_Structures.API_Classes.Hummingbird.V1.LibraryObject o)
        {
            try {
                EpisodesWatched = Convert.ToInt32(o.episodes_watched); ;
            }
            catch (Exception) { EpisodesWatched = 0; }

            string animeLastWatched = o.last_watched.Substring(0, o.last_watched.Length - 1);
            LastWatched = DateTime.Parse(animeLastWatched);

            try {
                RewatchedTimes = Convert.ToInt32(o.rewatched_times);
            }
            catch (Exception) { RewatchedTimes = 0; }

            if (o.notes == null) { Notes = ""; }
            else { Notes = o.notes.ToString(); }

            Status = APIConverter.StringToLibrarySelection(o.status);

            try {
                switch (o.@private) {
                    case true:      Private = PrivacySettings.Private;      break;
                    case false:     Private = PrivacySettings.Public;       break;
                }
            }
            catch (Exception) { }

            if (o.rewatching == null) Rewatching = false;
            else Rewatching = (bool)o.rewatching;

            Anime = new Anime(o.anime);
            Rating = o.rating.valueAsDouble;
        }

        public LibraryObject(Anitro.Data_Structures.API_Classes.Hummingbird.V1.Anime a)
        {
            EpisodesWatched = 0;
            LastWatched = new DateTime();
            RewatchedTimes = 0;
            Notes = "";
            Status = LibrarySelection.None;
            Private = PrivacySettings.Public;
            Rewatching = false;
            Anime = new Anime(a);
            Rating = 0.0;
        }

        public LibraryObject(API_Classes.Anime a)
        {
            EpisodesWatched = 0;
            LastWatched = new DateTime();
            RewatchedTimes = 0;
            Notes = "";
            Status = LibrarySelection.None;
            Private = PrivacySettings.Public;
            Rewatching = false;
            Anime = a;
            Rating = 0.0;
        }

        public LibraryObject()
        {
            EpisodesWatched = 0;
            LastWatched = new DateTime();
            RewatchedTimes = 0;
            Notes = "";
            Status = LibrarySelection.None;
            Private = PrivacySettings.Public;
            Rewatching = false;
            Anime = new Anime();
            Rating = 0.0;
        }
        #endregion
    }
}
