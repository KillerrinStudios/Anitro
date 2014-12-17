using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.Enumerators
{
    public enum MediaType
    {
        None,
        Unknown,

        // Anime
        Anime,
        TV,
        OVA,
        OAV,
        Movie,

        // Manga
        Manga,
        LightNovel,

        // Audio
        Music,
        Soundtrack,
        CharacterCD,
        AudioBook,

        // Other
        Special,
        Game,
        Other,
    }
}
