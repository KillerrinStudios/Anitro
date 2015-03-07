using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes
{
    public class CalendarEntry
    {
        public Anime Anime { get; set; }

        public DateTime StartDate { get; set; }
        public int EpisodeNumber { get; set; }

        public CalendarEntry()
        {
            Anime = new Anime("");
            StartDate = new DateTime();
            EpisodeNumber = 0;
        }

        public CalendarEntry(Anitro.Data_Structures.API_Classes.Hummingbird.Unofficial_APIs.CalendarDataset dataset)
        {
               DateTime start = new DateTime();
            if (DateTime.TryParse(dataset.sdate, out start))
                StartDate = start;
            else StartDate = new DateTime();
            
            EpisodeNumber = dataset.episd;

            Anime = new Anime(dataset.title);
        }
    }
}
