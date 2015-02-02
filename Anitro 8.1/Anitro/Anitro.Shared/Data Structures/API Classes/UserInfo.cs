using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.API_Classes
{
    public class TopGenres
    {
        public TopGenres(Hummingbird.V1.TopGenres g)
        {
            try {
                genre = APIConverter.StringToMediaGenre(g.genre.name);
                num = g.num;
            }
            catch (Exception) {
                genre = MediaGenre.None;
                num = 0;
            }
        }

        public TopGenres(Hummingbird.V2.TopGenreV2 g)
        {
            try
            {
                genre = APIConverter.StringToMediaGenre(g.genre.name);
                num = g.num;
            }
            catch (Exception)
            {
                genre = MediaGenre.None;
                num = 0;
            }
        }

        #region Documented
        public MediaGenre genre { get; set; }
        public double num { get; set; }
        #endregion
    }

    public class UserInfoFavorite
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int item_id { get; set; }
        public string item_type { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int fav_rank { get; set; }
    }

    public class UserInfo
    {
        #region Undocumented
        public int id { get; set; }
        //public int life_spent_on_anime { get; set; }
        public int anime_total { get; set; }

        private int m_animeWatched;
        public int anime_watched {
            get 
            { 
                if (m_animeWatched != 0)
                    return m_animeWatched;

                int total = 0;
                foreach (var i in top_genres)
                {
                    total += (int)i.num;
                }
                return total;
            }
            set { m_animeWatched = value; }        
        }
        public List<TopGenres> top_genres { get; set; }
        #endregion

        #region Documented
        public string name { get; set; }

        private string _waifu;
        public string waifu {
            get { return _waifu; }
            set
            {
                if (value != null)
                {
                    _waifu = value;
                }
            }
        }
        public string waifu_or_husbando { get; set; }
        public string waifu_slug { get; set; }

        private string _waifuCharID;

        public string waifu_char_id
        {
            get { return _waifuCharID; }
            set 
            {
                _waifuCharID = value; 
            }
        }

        public Uri GetWaifuPictureURI()
        {
            try {
                string beginningUrl = "http://static.hummingbird.me/characters/images/000/0";
                string firstTwo = "" + _waifuCharID[0] + _waifuCharID[1] + "/";
                string lastThree = "" + _waifuCharID[2] + _waifuCharID[3] + _waifuCharID[4] + '/';
                string afterCutID = "thumb_small/";
                string fullWaifuIDExtension = _waifuCharID + ".jpg?1375255551";

                string fullUrl = beginningUrl + firstTwo + lastThree + afterCutID + fullWaifuIDExtension;
                return new Uri(fullUrl, UriKind.Absolute);
            }
            catch (Exception) {
                return new Uri("http://www.hummingbird.me", UriKind.Absolute);
            }
        }

        public string location { get; set; }
        public string website { get; set; }
        public string avatar { get; set; }
        public string cover_image { get; set; }
        public object about { get; set; }
        public string bio { get; set; }
        public int karma { get; set; }
        public int life_spent_on_anime { get; set; }
        public bool show_adult_content { get; set; }
        public string title_language_preference { get; set; }
        public string last_library_update { get; set; }
        public bool online { get; set; }
        public bool following { get; set; }
        #endregion

        public UserInfo()
        {
            id = 0;
            anime_total = 0;
            anime_watched = 0;
            top_genres = new List<TopGenres>();

            name = "";
            waifu = "";
            waifu_slug = "";
            waifu_char_id = "";
            waifu_or_husbando = "";

            location = "";
            website = "";
            avatar = "";
            cover_image = "";
            about = "";
            bio = "";
            karma = 0;
            life_spent_on_anime = 0;
            show_adult_content = false;
            title_language_preference = "";
            last_library_update = "";
            online = true;
            following = false;
        }
        public UserInfo(Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfo userInfo)
        {
            top_genres = new List<TopGenres>();

            Debug.WriteLine("Waifu User Info");
            try {
                name = userInfo.name;
                waifu = userInfo.waifu;
                waifu_slug = userInfo.waifu_slug;
                waifu_char_id = userInfo.waifu_char_id;
                waifu_or_husbando = userInfo.waifu_or_husbando;
            }
            catch (Exception) { Debug.WriteLine("Exception"); }

            Debug.WriteLine("User Account Information");
            try {
                location = userInfo.location;
                website = userInfo.website;
                avatar = userInfo.avatar;
                cover_image = userInfo.cover_image;
                about = userInfo.about;
                bio = userInfo.bio;
            }
            catch (Exception) { Debug.WriteLine("Exception"); }

            Debug.WriteLine("Reputation, Stats and Social information");
            try {
                karma = userInfo.karma;
                show_adult_content = userInfo.show_adult_content;
                title_language_preference = userInfo.title_language_preference;
                last_library_update = userInfo.last_library_update;
                online = userInfo.online;
                following = userInfo.following;
            }
            catch (Exception) { Debug.WriteLine("Exception"); }

            Debug.WriteLine("Undocumented User Info");
            try {
                id = userInfo.id;
                life_spent_on_anime = userInfo.life_spent_on_anime;
                anime_total = userInfo.anime_total;
                anime_watched = userInfo.anime_watched;
            }
            catch (Exception) { Debug.WriteLine("Undocumented User Info: Exception"); };

            Debug.WriteLine("TopGenres");
            try {
                foreach (var g in userInfo.top_genres) {
                    top_genres.Add(new TopGenres(g));
                }
            }
            catch (Exception) { Debug.WriteLine("Exception"); }
        }

        public UserInfo(Hummingbird.V2.UserInfoV2 userInfo)
        {
            top_genres = new List<TopGenres>();

            Debug.WriteLine("Undocumented User Info");
            try
            {
                id = Convert.ToInt32(userInfo.id);
                life_spent_on_anime = userInfo.life_spent_on_anime;

                anime_total = userInfo.anime_watched;
                anime_watched = userInfo.anime_watched;
            }
            catch (Exception) { Debug.WriteLine("Undocumented User Info: Exception"); };

            Debug.WriteLine("TopGenres");
            try
            {
                foreach (var g in userInfo.top_genres)
                {
                    top_genres.Add(new TopGenres(g));
                }
            }
            catch (Exception) { Debug.WriteLine("Exception"); }
        }

        public void AddUnDocumentedToDocumented(UserInfo _undocumentedUserinfo)
        {
            Debug.WriteLine("AddUnDocumentedToDocumented: Entering");

            try {
                id = _undocumentedUserinfo.id;
                life_spent_on_anime = _undocumentedUserinfo.life_spent_on_anime;
                anime_total = _undocumentedUserinfo.anime_total;
                anime_watched = _undocumentedUserinfo.anime_watched;
                top_genres = _undocumentedUserinfo.top_genres;
            }
            catch (Exception) { Debug.WriteLine("AddUnDocumentedToDocumented: Exception"); }
            
            Debug.WriteLine("AddUnDocumentedToDocumented: Exiting");
        }
        public void AddUnDocumentedToDocumented(Anitro.Data_Structures.API_Classes.Hummingbird.V1.UserInfo _undocumentedUserinfo)
        {
            try {
                id = _undocumentedUserinfo.id;
                life_spent_on_anime = _undocumentedUserinfo.life_spent_on_anime;
                anime_total = _undocumentedUserinfo.anime_total;
                anime_watched = _undocumentedUserinfo.anime_watched;

                top_genres = new List<TopGenres>();
                foreach (var g in _undocumentedUserinfo.top_genres) {
                    top_genres.Add(new TopGenres(g));
                }
            }
            catch (Exception) { }
        }
    }

    public class UserInfoRootObject
    {
        public UserInfo user_info { get; set; }
    }
}
