using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Anitro.Data_Structures.Enumerators;

namespace Anitro.Data_Structures.API_Classes
{
    public static class APIConverter
    {
        public static List<Anime> AnimeList (Anitro.Data_Structures.API_Classes.Hummingbird.V1.AnimeList list)
        {
            Debug.WriteLine("APIConverter: AnimeList: Entering");
            List<Anime> listOfAnime = new List<Anime>();

            foreach (var a in list.anime)
            {
                listOfAnime.Add(new Anime(a));
            }
            return listOfAnime;
        }

        public static MediaGenre StringToMediaGenre(string g)
        {
            if (string.IsNullOrWhiteSpace(g)) return MediaGenre.None;
            Debug.WriteLine("Original Genre: " + g);

            switch (g) {
                case "Action":              return MediaGenre.Action;
                case "Adventure":           return MediaGenre.Adventure;
                case "Anime Influenced":    return MediaGenre.AnimeInfluenced;
                case "Cars":                return MediaGenre.Cars;
                case "Comedy":              return MediaGenre.Comedy;
                case "Dementia":            return MediaGenre.Dementia;
                case "Demons":              return MediaGenre.Demons;
                case "Drama":               return MediaGenre.Drama;
                case "Ecchi":               return MediaGenre.Ecchi;
                case "Fantasy":             return MediaGenre.Fantasy;
                case "Game":                return MediaGenre.Game;
                case "Harem":               return MediaGenre.Harem;
                case "Historical":          return MediaGenre.Historical;
                case "Horror":              return MediaGenre.Horror;
                case "Kids":                return MediaGenre.Kids;
                case "Magic":               return MediaGenre.Magic;
                case "Martial Arts":        return MediaGenre.MartialArts;
                case "Mecha":               return MediaGenre.Mecha;
                case "Military":            return MediaGenre.Military;
                case "Music":               return MediaGenre.Music;
                case "Mystery":             return MediaGenre.Mystery;
                case "Parody":              return MediaGenre.Parody;
                case "Psychological":       return MediaGenre.Psychological;
                case "Romance":             return MediaGenre.Romance;
                case "Samurai":             return MediaGenre.Samurai;
                case "School":              return MediaGenre.School;
                case "Sci-Fi":              return MediaGenre.SciFi;
                case "Shoujo Ai":           return MediaGenre.ShoujoAi;
                case "Shounen Ai":          return MediaGenre.ShounenAi;
                case "Slice of Life":       return MediaGenre.SliceOfLife;
                case "Space":               return MediaGenre.Space;
                case "Sports":              return MediaGenre.Sports;
                case "Supernatural":        return MediaGenre.Supernatural;
                case "Super Power":         return MediaGenre.SuperPower;
                case "Thriller":            return MediaGenre.Thriller;
                case "Vampire":             return MediaGenre.Vampire;
                case "None":
                case "Unknown":
                default:                    return MediaGenre.Unknown;
            }
        }

        public static MediaType StringToMediaType(string m)
        {
            switch (m) {
                case "None"             : return MediaType.None;
                case "Anime"            : return MediaType.Anime;
                case "TV"               : return MediaType.TV;
                case "OVA"              : return MediaType.OVA;
                case "OAV"              : return MediaType.OAV;
                case "Movie"            : return MediaType.Movie;
                case "Manga"            : return MediaType.Manga;
                case "LightNovel"       : return MediaType.LightNovel;
                case "Music"            : return MediaType.Music;
                case "Soundtrack"       : return MediaType.Soundtrack;
                case "CharacterCD"      : return MediaType.CharacterCD;
                case "AudioBook"        : return MediaType.AudioBook;
                case "Special"          : return MediaType.Special;
                case "Game"             : return MediaType.Game;
                case "Other"            : return MediaType.Other;
                case "Unknown"          :
                default                 : return MediaType.Unknown;
            }
        }

        public static AiringStatus StringToAiringStatus(string p)
        {
            if (string.IsNullOrWhiteSpace(p)) return AiringStatus.Unknown;
            Debug.WriteLine("Airing Status: " + p);

            switch (p) {
                case "None": return AiringStatus.None;

                case "Not Yet Aired"    :
                case "NotYetAired"      : return AiringStatus.NotYetAired;

                case "Currently Airing" :
                case "CurrentlyAiring"  : return AiringStatus.CurrentlyAiring;

                case "Finished Airing"  :
                case "FinishedAiring"   : return AiringStatus.FinishedAiring;

                case "Unknown"          :
                default                 : return AiringStatus.Unknown;
            }
        }

        public static LibrarySelection StringToLibrarySelection(string status)
        {
            switch (status) {
                case "currently-watching":
                    return LibrarySelection.CurrentlyWatching;
                case "currently-reading":
                    return LibrarySelection.CurrentlyReading;

                case "plan-to-watch":
                    return LibrarySelection.PlanToWatch;
                case "plan-to-read":
                    return LibrarySelection.PlanToRead;

                case "completed":
                    return LibrarySelection.Completed;
                case "on-hold":
                    return LibrarySelection.OnHold;
                case "dropped":
                    return LibrarySelection.Dropped;

                case "favourites":
                    return LibrarySelection.Favourites;
                case "recent":
                    return LibrarySelection.Recent;
                case "search":
                    return LibrarySelection.Search;

                case "all":
                    return LibrarySelection.All;
                default:
                    return LibrarySelection.None;
            }
        }

        public static string LibrarySelectionToString(LibrarySelection lS)
        {
            switch (lS) {
                case LibrarySelection.CurrentlyWatching:
                    return "currently-watching";
                case LibrarySelection.CurrentlyReading:
                    return "currently-reading";

                case LibrarySelection.PlanToWatch:
                    return "plan-to-watch";
                case LibrarySelection.PlanToRead:
                    return "plan-to-read";

                case LibrarySelection.Completed:
                    return "completed";
                case LibrarySelection.OnHold:
                    return "on-hold";
                case LibrarySelection.Dropped:
                    return "dropped";

                case LibrarySelection.All:
                case LibrarySelection.Favourites:
                case LibrarySelection.Recent:
                case LibrarySelection.Search:
                case LibrarySelection.None:
                default:
                    return "";
            }
        }

        public static string PrivacySettingsToString(PrivacySettings pS)
        {
            switch (pS) {
                case PrivacySettings.Public:     return "public";
                case PrivacySettings.Private:    return "private";
                default: return "";
            }
        }

        public static bool PrivacySettingsToBool(PrivacySettings pS)
        {
            switch (pS) {
                case PrivacySettings.Private:   return true;
                case PrivacySettings.Public:    return false;
                default:                        return false;
            }
        }

        public static PrivacySettings BoolToPrivacySettings(bool pS)
        {
            switch (pS) {
                case true:  return PrivacySettings.Private;
                case false: return PrivacySettings.Public;
                default:    return PrivacySettings.Public;
            }
        }


    }
}
