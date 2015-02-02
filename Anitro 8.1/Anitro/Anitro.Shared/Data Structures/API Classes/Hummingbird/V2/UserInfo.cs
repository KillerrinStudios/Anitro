using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes.Hummingbird.V2
{
    public class ItemV2
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class FavoriteV2
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public int fav_rank { get; set; }
        public ItemV2 item { get; set; }
    }

    public class UserV2
    {
        public string id { get; set; }
        public string cover_image_url { get; set; }
        public string avatar_template { get; set; }
        public string rating_type { get; set; }
        public string mini_bio { get; set; }
        public string about { get; set; }
        public bool is_followed { get; set; }
        public string title_language_preference { get; set; }
        public string location { get; set; }
        public string website { get; set; }
        public string waifu { get; set; }
        public string waifu_or_husbando { get; set; }
        public string waifu_slug { get; set; }
        public string waifu_char_id { get; set; }
        public string last_sign_in_at { get; set; }
        public string current_sign_in_at { get; set; }
        public bool is_admin { get; set; }
        public int following_count { get; set; }
        public int follower_count { get; set; }
        public object is_pro { get; set; }
    }

    public class GenreV2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object description { get; set; }
    }

    public class TopGenreV2
    {
        public GenreV2 genre { get; set; }
        public double num { get; set; }
    }

    public class UserInfoV2
    {
        public string id { get; set; }
        public int life_spent_on_anime { get; set; }
        public int anime_watched { get; set; }
        public List<TopGenreV2> top_genres { get; set; }
        public List<int> favorite_ids { get; set; }
    }

    public class UserInfoV2RootObject
    {
        public List<FavoriteV2> favorites { get; set; }
        public List<AnimeV2> anime { get; set; }
        public List<UserV2> users { get; set; }
        public List<MangaV2> manga { get; set; }
        public UserInfoV2 user_info { get; set; }
    }
}
