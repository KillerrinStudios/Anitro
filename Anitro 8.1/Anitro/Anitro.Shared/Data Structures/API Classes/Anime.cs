using System;
using System.Collections.Generic;
using System.Text;
using Anitro.Data_Structures.Enumerators;
using System.Diagnostics;

namespace Anitro.Data_Structures.API_Classes
{
    public class Anime
    {
        #region Properties
        public string ServiceID { get; set; }
        public string ServiceID2 { get; set; }

        public AiringStatus AiringStatus { get; set; }

        public Uri WebUrl { get; set; }
        public string WebUrlString
        {
            get { return WebUrl.OriginalString; }
            set
            {
                if (!string.IsNullOrEmpty(value)) {
                    WebUrl = new Uri(value, UriKind.Absolute);
                }
            }
        }

        public string RomanjiTitle { get; set; }
        public string EnglishTitle { get; set; }
        public string KanjiTitle { get; set; }

        public int EpisodeCount { get; set; }
        public string EpisodeCountString
        {
            get
            {
                if (EpisodeCount == 0) return "?";
                return EpisodeCount.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) { EpisodeCount = 0; }
                else if (value.Contains("?")) { EpisodeCount = 0; }
                else {
                    try { EpisodeCount = Convert.ToInt32(value); }
                    catch (Exception) { EpisodeCount = 0; }
                }
            }
        }

        private Uri m_coverImageUrl;
        public Uri CoverImageUrl 
        {
            get { return m_coverImageUrl; }
            set 
            {
                if (value != null)
                {
                    m_coverImageUrl = value;
                }
            }
        }

        public string CoverImageUrlString
        {
            get { return m_coverImageUrl.OriginalString; }
            set
            {
                if (!string.IsNullOrEmpty(value)) {
                    m_coverImageUrl = new Uri(value, UriKind.Absolute);
                }
            }
        }

        public string Synopsis { get; set; }
        public MediaType MediaType { get; set; }

        public int FavouriteRank { get; set; }
        public int FavouriteID { get; set; }

        public List<MediaGenre> Genres { get; set; }
        #endregion

        #region Constructors
        public Anime()
        {
            MediaType = Enumerators.MediaType.None;
            AiringStatus = Enumerators.AiringStatus.None;
            Genres = new List<MediaGenre>();
        }
        public Anime(string romanjiTitle)
        {
            RomanjiTitle = romanjiTitle;
            EnglishTitle = romanjiTitle;

            MediaType = Enumerators.MediaType.None;
            AiringStatus = Enumerators.AiringStatus.None;
            Genres = new List<MediaGenre>();
        }

        public Anime(Anitro.Data_Structures.API_Classes.Hummingbird.V1.Anime o)
        {
            try {
                RomanjiTitle = o.title;
            }
            catch (Exception) { RomanjiTitle = ""; }

            if (string.IsNullOrWhiteSpace(o.alternate_title)) {
                EnglishTitle = "";
            }
            else {
                try {
                    EnglishTitle = o.alternate_title;
                }
                catch (Exception) { EnglishTitle = ""; }
            }

            try { 
                CoverImageUrl = o.cover_image_uri;
            }
            catch (Exception) { CoverImageUrl = new Uri(""); }

            try { 
                EpisodeCountString = o.episode_count;
            }
            catch (Exception) { EpisodeCountString = "0"; }

            try { 
                FavouriteID = o.fav_id;
            }
            catch (Exception) { FavouriteID = 0; }

            try { 
                FavouriteRank = o.fav_rank;
            }
            catch (Exception) { FavouriteRank = 0; }

            Debug.WriteLine("Beginning Genre Parse");
            Genres = new List<MediaGenre>();

            try {
                if (o.genres == null) { Debug.WriteLine("No Genres"); }
                else {
                    Debug.WriteLine("Genres Available");
                    foreach (var g in o.genres) {
                        AddMediaGenre(APIConverter.StringToMediaGenre(g.name));
                    }
                }
            }
            catch (Exception) { }


            try { 
                MediaType = APIConverter.StringToMediaType(o.show_type);
            }
            catch (Exception) { MediaType = Enumerators.MediaType.Unknown; }

            try { 
                ServiceID = o.slug;
            }
            catch (Exception) { ServiceID = ""; }

            try {
                ServiceID2 = o.slug;
            }
            catch (Exception) { }

            try { 
                AiringStatus = APIConverter.StringToAiringStatus(o.status);
            }
            catch (Exception) {
                Debug.WriteLine("Airing Status Exception");
                AiringStatus = Enumerators.AiringStatus.Unknown; 
            }
            
            try { 
                Synopsis = o.synopsis;
            }
            catch (Exception) { Synopsis = ""; }

            try {
                WebUrlString = o.url;
            }
            catch (Exception) { WebUrlString = ""; }
        }
        #endregion

        #region Helper Methods
        public void AddMediaGenre(MediaGenre genre) {
            if (genre == MediaGenre.Unknown) return;
            if (genre == MediaGenre.None) return;
            
            Genres.Add(genre);
        }
        #endregion
    }
}
