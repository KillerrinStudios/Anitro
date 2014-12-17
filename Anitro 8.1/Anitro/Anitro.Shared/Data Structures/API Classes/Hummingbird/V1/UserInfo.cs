using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.API_Classes.Hummingbird.V1
{
    public class TopGenres
    {
        #region Documented
        public Genre genre { get; set; }
        public double num { get; set; }
        #endregion

        #region oldCode
        public List<MediaGenre> genresWithData = new List<MediaGenre>() { };

        public double GetGenreTotal(MediaGenre genre)
        {
            switch (genre)
            {
                case MediaGenre.Action:
                    return Action;
                case MediaGenre.Adventure:
                    return Adventure;
                case MediaGenre.AnimeInfluenced:
                    return AnimeInfluenced;

                case MediaGenre.Cars:
                    return Cars;
                case MediaGenre.Comedy:
                    return Comedy;

                case MediaGenre.Dementia:
                    return Dementia;
                case MediaGenre.Demons:
                    return Demons;
                case MediaGenre.Drama:
                    return Drama;

                case MediaGenre.Ecchi:
                    return Ecchi;

                case MediaGenre.Fantasy:
                    return Fantasy;

                case MediaGenre.Game:
                    return Game;

                case MediaGenre.Harem:
                    return Harem;
                case MediaGenre.Historical:
                    return Historical;
                case MediaGenre.Horror:
                    return Horror;

                case MediaGenre.Kids:
                    return Kids;

                case MediaGenre.Magic:
                    return Magic;
                case MediaGenre.MartialArts:
                    return MartialArts;
                case MediaGenre.Mecha:
                    return Mecha;
                case MediaGenre.Military:
                    return Military;
                case MediaGenre.Music:
                    return Music;
                case MediaGenre.Mystery:
                    return Mystery;
                case MediaGenre.Parody:
                    return Parody;
                case MediaGenre.Psychological:
                    return Psychological;

                case MediaGenre.Romance:
                    return Romance;

                case MediaGenre.Samurai:
                    return Samurai;
                case MediaGenre.School:
                    return School;
                case MediaGenre.SciFi:
                    return SciFi;
                case MediaGenre.ShoujoAi:
                    return ShoujoAi;
                case MediaGenre.ShounenAi:
                    return ShounenAi;
                case MediaGenre.SliceOfLife:
                    return SliceOfLife;
                case MediaGenre.Space:
                    return Space;
                case MediaGenre.Sports:
                    return Sports;
                case MediaGenre.Supernatural:
                    return Supernatural;
                case MediaGenre.SuperPower:
                    return SuperPower;

                case MediaGenre.Thriller:
                    return Thriller;

                case MediaGenre.Vampire:
                    return Vampire;
                default:
                    return 0.0;
            }
        }

        #region Private Variables
        private double _action;
        private double _adventure;
        private double _animeInfluenced;

        private double _cars;
        private double _comedy;

        private double _dementia;
        private double _demons;
        private double _drama;

        private double _ecchi;

        private double _fantasy;

        private double _game;

        private double _harem;
        private double _historical;
        private double _horror;

        private double _kids;

        private double _magic;

        private double _martialArts;
        private double _mecha;
        private double _military;
        private double _music;
        private double _mystery;

        private double _parody;
        private double _psychological;

        private double _romance;

        private double _samurai;
        private double _school;
        private double _sciFi;
        private double _shoujoAi;
        private double _shounenAi;
        private double _sliceOfLife;
        private double _space;
        private double _sports;
        private double _supernatural;
        private double _superPower;

        private double _thriller;

        private double _vampire;
        #endregion

        #region Properties
        public double Action 
        { 
            get { return _action;}
            set { genresWithData.Add(MediaGenre.Action); _action = value; }
        }

        public double Adventure
        {
            get { return _adventure; }
            set { genresWithData.Add(MediaGenre.Adventure); _adventure = value;
            }
        }

        [JsonProperty(PropertyName = "Anime Influenced")]
        public double AnimeInfluenced
        {
            get { return _animeInfluenced; }
            set { genresWithData.Add(MediaGenre.AnimeInfluenced); _animeInfluenced = value; }
        }

        public double Cars
        {
            get { return _cars; }
            set { genresWithData.Add(MediaGenre.Cars); _cars = value; }
        }
        public double Comedy
        {
            get { return _comedy; }
            set { genresWithData.Add(MediaGenre.Comedy); _comedy = value; }
        }

        public double Dementia
        {
            get { return _dementia; }
            set { genresWithData.Add(MediaGenre.Dementia); _dementia = value; }
        }
        public double Demons
        {
            get { return _demons; }
            set { genresWithData.Add(MediaGenre.Demons); _demons = value; }
        }
        public double Drama
        {
            get { return _drama; }
            set { genresWithData.Add(MediaGenre.Drama); _drama = value; }
        }

        public double Ecchi
        {
            get { return _ecchi; }
            set { genresWithData.Add(MediaGenre.Ecchi); _ecchi = value; }
        }

        public double Fantasy
        {
            get { return _fantasy; }
            set { genresWithData.Add(MediaGenre.Fantasy); _fantasy = value; }
        }

        public double Game
        {
            get { return _game; }
            set { genresWithData.Add(MediaGenre.Game); _game = value; }
        }

        public double Harem
        {
            get { return _harem; }
            set { genresWithData.Add(MediaGenre.Harem); _harem = value; }
        }
        public double Historical { 
            get { return _historical;}
            set { genresWithData.Add(MediaGenre.Historical); _historical = value; }
        }
        public double Horror
        {
            get { return _horror; }
            set { genresWithData.Add(MediaGenre.Horror); _horror = value; }
        }

        public double Kids
        {
            get { return _kids; }
            set { genresWithData.Add(MediaGenre.Kids); _kids = value; }
        }

        public double Magic
        {
            get { return _magic; }
            set { genresWithData.Add(MediaGenre.Magic); _magic = value; }
        }
    
        [JsonProperty(PropertyName = "Martial Arts")]
        public double MartialArts
        {
            get { return _martialArts; }
            set { genresWithData.Add(MediaGenre.MartialArts); _martialArts = value; }
        }
        public double Mecha
        {
            get { return _mecha; }
            set { genresWithData.Add(MediaGenre.Mecha); _mecha = value; }
        }
        public double Military
        {
            get { return _military; }
            set { genresWithData.Add(MediaGenre.Military); _military = value; }
        }
        public double Music
        {
            get { return _music; }
            set { genresWithData.Add(MediaGenre.Music); _music = value; }
        }
        public double Mystery
        {
            get { return _mystery; }
            set { genresWithData.Add(MediaGenre.Mystery); _mystery = value; }
        }

        public double Parody
        {
            get { return _parody; }
            set { genresWithData.Add(MediaGenre.Parody); _parody = value; }
        }
        public double Psychological
        {
            get { return _psychological; }
            set { genresWithData.Add(MediaGenre.Psychological); _psychological = value; }
        }

        public double Romance  { 
            get { return _romance;}
            set { genresWithData.Add(MediaGenre.Romance); _romance = value; }
        }

        public double Samurai
        {
            get { return _samurai; }
            set { genresWithData.Add(MediaGenre.Samurai); _samurai = value; }
        }
        public double School
        {
            get { return _school; }
            set { genresWithData.Add(MediaGenre.School); _school = value; }
        }

        [JsonProperty(PropertyName = "Sci-Fi")]
        public double SciFi
        {
            get { return _sciFi; }
            set { genresWithData.Add(MediaGenre.SciFi); _sciFi = value; }
        }
    
        [JsonProperty(PropertyName = "Shoujo Ai")]
        public double ShoujoAi
        {
            get { return _shoujoAi; }
            set { genresWithData.Add(MediaGenre.ShoujoAi); _shoujoAi = value; }
        }

        [JsonProperty(PropertyName = "Shounen Ai")]
        public double ShounenAi
        {
            get { return _shounenAi; }
            set { genresWithData.Add(MediaGenre.ShounenAi); _shounenAi = value; }
        }

        [JsonProperty(PropertyName = "Slice of Life")]
        public double SliceOfLife
        {
            get { return _sliceOfLife; }
            set { genresWithData.Add(MediaGenre.SliceOfLife); _sliceOfLife = value; }
        }
        public double Space
        {
            get { return _space; }
            set { genresWithData.Add(MediaGenre.Space); _space = value; }
        }
        public double Sports
        {
            get { return _sports; }
            set { genresWithData.Add(MediaGenre.Sports); _sports = value; }
        }
        public double Supernatural
        {
            get { return _supernatural; }
            set { genresWithData.Add(MediaGenre.Supernatural); _supernatural = value; }
        }

        [JsonProperty(PropertyName = "Super Power")]
        public double SuperPower
        {
            get { return _superPower; }
            set { genresWithData.Add(MediaGenre.SuperPower); _superPower = value; }
        }

        public double Thriller
        {
            get { return _thriller; }
            set { genresWithData.Add(MediaGenre.Thriller); _thriller = value; }
        }

        public double Vampire
        {
            get { return _vampire; }
            set { genresWithData.Add(MediaGenre.Vampire); _vampire = value; }
        }
        #endregion
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
        public int anime_watched { get; set; }
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
            string beginningUrl = "http://static.hummingbird.me/characters/images/000/0";
            string firstTwo = "" + _waifuCharID[0] + _waifuCharID[1] + "/";
            string lastThree = "" + _waifuCharID[2] + _waifuCharID[3] + _waifuCharID[4] + '/';
            string afterCutID = "thumb_small/";
            string fullWaifuIDExtension =  _waifuCharID + ".jpg?1375255551";

            string fullUrl = beginningUrl + firstTwo + lastThree + afterCutID + fullWaifuIDExtension;
            return new Uri(fullUrl, UriKind.Absolute);
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

        public void AddUnDocumentedToDocumented(UserInfo _undocumentedUserinfo)
        {
            try
            {
                id = _undocumentedUserinfo.id;
                life_spent_on_anime = _undocumentedUserinfo.life_spent_on_anime;
                anime_total = _undocumentedUserinfo.anime_total;
                anime_watched = _undocumentedUserinfo.anime_watched;
                top_genres = _undocumentedUserinfo.top_genres;
            }
            catch (Exception) { }
        }
    }

    public class UserInfoRootObject
    {
        public UserInfo user_info { get; set; }
    }
}
