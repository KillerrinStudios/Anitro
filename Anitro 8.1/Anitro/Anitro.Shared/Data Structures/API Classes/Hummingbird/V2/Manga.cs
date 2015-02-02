using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes.Hummingbird.V2
{
    public class MangaV2
    {
        public string id { get; set; }
        public string romaji_title { get; set; }
        public string poster_image { get; set; }
        public string synopsis { get; set; }
        public int chapter_count { get; set; }
        public int volume_count { get; set; }
        public List<string> genres { get; set; }
        public string manga_type { get; set; }
        public string updated_at { get; set; }
        public string english_title { get; set; }
    }
}
