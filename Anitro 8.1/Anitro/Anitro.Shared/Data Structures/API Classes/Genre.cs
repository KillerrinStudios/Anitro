using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures.API_Classes
{
    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        private string _description;
        public string description
        {
            get { return _description; }
            set { 
                if (!string.IsNullOrEmpty(value))
                {
                    _description = value;
                }
                else { _description = ""; }
            }
        }
    }
}
