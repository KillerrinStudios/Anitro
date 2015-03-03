using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Anitro.Data_Structures.API_Classes
{
    public class ActivityFeedObject
    {
        public string ServiceID { get; set; }

        public Uri StoryImage { get; set; }

        string m_header;
        public string Header { get { return m_header; } set { m_header = value; } }

        string m_content;
        public string Content 
        { 
            get { return m_content; } 
            set 
            { 
                m_content = WebUtility.HtmlDecode(value);
                m_content = m_content.Replace("<br>", Environment.NewLine);
            } 
        }

        public DateTime TimeStamp { get; set; }

        public ActivityFeedObject()
        {
            ServiceID = "";
            StoryImage = new Uri("https://hummingbird.me/default_avatar.jpg", UriKind.Absolute);
            Header = "";
            Content = "";
            TimeStamp = DateTime.UtcNow;
        }
    }
}
