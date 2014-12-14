using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro
{
    public class KillerrinApplicationData
    {
        public string Name = "Anitro";
        public string Version = "1.2.0.0";
        public string Description = "Developed by Andrew Godfroy of Killerrin Studios. Hummingbird is owned and developed by the crew of hummingbird.me";

        public string Developer = "Killerrin Studios";
        public string Website = "http://www.killerrin.com";
        public string OtherWebsite = "http://www.hummingbird.me";
        public string Twitter = "http://www.twitter.com/killerrin";
        
        public string FeedbackUrl = "support@killerrin.com";
        public string FeedbackSubject = "feedback - ";

        public string SupportUrl = "support@killerrin.com";
        public string SupportSubject = "support - ";

#if WINDOWS_PHONE_APP
        public AnitroClientOSType OS = AnitroClientOSType.WindowsPhone81;
#elif WINDOWS_APP
        public AnitroClientOSType OS = AnitroClientOSType.Windows81;
#endif

        public KillerrinApplicationData()
        {
            FeedbackSubject += Name;
            SupportSubject += Name;
        }

        // Mashape API Key
        public string MashapeKey
        {
            get
            {
                // If we're in Debug Mode, Return the Mashape Testing Key to make testing easier
                if (DebugTools.DebugMode) return "JIyg90lZ0KRmT0qivz8ECXjvl0rd18lS";

                // If not, return the production key
                return "TkLbJdjaFrDjcjuGrKc5XvJREP0pgnYs";
            }
        }

        public static string PubCenterApplicationID
        {
            get
            {
                if (DebugTools.DebugMode) return "test_client";
                return "e4487080-07e5-4855-899a-a5ba5df17371";
            }
        }
        public static string PubCenterAdUnitID
        {
            get
            {
                if (DebugTools.DebugMode) return "Image320_50";
                return "186550";
            }
        }

        public string BugSenseKey
        {
            get
            {
                // If our OS Type is WindowsPhone81, return that key
                if (OS == AnitroClientOSType.WindowsPhone81) return "db0cc3c7";

                // if not, return the Windows81 key
                return "db0cc3c7";
            }
        }

        public string AnitroPackageName
        {
            get
            {
                // If the OS is Windows Phone 8.1, return that store key
                if (OS == AnitroClientOSType.WindowsPhone81)
                    return "6377b104-7df2-4d29-8ebb-88527728e673";

                // If not, the OS is Windows 8.1 so return that store key
                return "665e06bf-b4de-4cde-8ae3-c20c1a98b12b";             
            }
        }
    }
}
