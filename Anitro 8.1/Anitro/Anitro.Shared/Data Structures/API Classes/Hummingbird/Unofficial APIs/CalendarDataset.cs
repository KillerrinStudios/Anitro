using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes.Hummingbird.Unofficial_APIs
{
    public class CalendarDataset
    {
        public string title { get; set; }
        public string sdate { get; set; }
        public int episd { get; set; }
    }

    public class CalendarRootObject
    {
        public bool success { get; set; }
        public List<CalendarDataset> dataset { get; set; }
    }
}
