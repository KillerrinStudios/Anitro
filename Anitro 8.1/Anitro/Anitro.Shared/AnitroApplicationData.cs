using Anitro.Data_Structures.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit;
using KillerrinStudiosToolkit.Enumerators;

namespace Anitro
{
    public class AnitroApplicationData : KillerrinStudiosToolkit.KillerrinApplicationData
    {
        public AnitroApplicationData(string packageID, string publisherID)
            :base(packageID,
                  publisherID,
                  "Anitro",
                  "1.2.0.0",
                  "Developed by Andrew Godfroy of Killerrin Studios. Hummingbird is owned and developed by the crew of hummingbird.me",
                  "http://www.hummingbird.me")
        {

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
                if (OS == ClientOSType.WindowsPhone81) return "db0cc3c7";

                // if not, return the Windows81 key
                return "db0cc3c7";
            }
        }
    }
}
