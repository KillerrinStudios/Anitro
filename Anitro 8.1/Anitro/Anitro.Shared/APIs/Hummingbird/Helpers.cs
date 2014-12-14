using Anitro.APIs.Events;
using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Anitro.APIs.Hummingbird
{
    public class Helpers
    {
        public static event APICompletedEventHandler APICompletedEventHandler;
        public static event APIFeedbackEventHandler FeedbackEventHandler;


        private static string Domain = "http://www.hummingbird.me";
        public static string CreateHummingbirdUrl(string endpoint, HummingbirdAPILevel apiLevel = HummingbirdAPILevel.None, bool omitWWW = false)
        {
            string apiPoint;
            switch (apiLevel)
            {
                case HummingbirdAPILevel.Version1:
                    apiPoint = "/api/v1/";
                    break;
                case HummingbirdAPILevel.Version2:
                    apiPoint = "/api/v2/";
                    break;

                case HummingbirdAPILevel.None:
                default:
                    apiPoint = "/";
                    break;
            }


            string fullPath;
            if (omitWWW) fullPath = "http://" + Domain.Substring(11) + apiPoint + endpoint;
            else fullPath = Domain + apiPoint + endpoint;


            Debug.WriteLine(fullPath);
            return fullPath;
        }
        public static Uri CreateHummingbirdUri(string endpoint, HummingbirdAPILevel apiLevel = HummingbirdAPILevel.None, bool omitWWW = false)
        {
            return new Uri(CreateHummingbirdUrl(endpoint, apiLevel, omitWWW), UriKind.Absolute);
        }

        public static string ConvertToAPIConpliantString(string _text, char charToParse = ' ', char replacementChar = '-')
        {
            Debug.WriteLine("ConvertToAPIConpliantString()");
            string text = _text;
            text.ToLower();
            char[] txtarr = text.ToCharArray();
            text = "";
            foreach (char c in txtarr)
            {
                if (c == charToParse) { text += replacementChar; }
                else { text += c; }
            }

            return text;
        }
    }
}
