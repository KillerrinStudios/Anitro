using Anitro.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class AnitroLaunchArgs
    {
        public AnitroLaunchActivation Activation = AnitroLaunchActivation.Normal;
        public AnitroLaunchReason LaunchReason = AnitroLaunchReason.Normal;
        public string Parameter = "";

        public AnitroLaunchArgs()
        {

        }
        public AnitroLaunchArgs(AnitroLaunchActivation activation, AnitroLaunchReason reason, string parameter)
        {
            Activation = activation;
            LaunchReason = reason;
            Parameter = parameter;
        }

        public bool ParseProtocol(Uri protocol)
        {
            string originalString = protocol.OriginalString;
            if (string.IsNullOrWhiteSpace(originalString)) return false;

            if (originalString.Contains("hummingbird.me"))
            {
                LaunchReason = AnitroLaunchReason.GoToDetails;

                string afterDotMe = originalString.Substring(originalString.IndexOf(".me") + 4);
                if (afterDotMe.ToLower().Contains("anime"))
                    Parameter += "anime=";
                else if (afterDotMe.ToLower().Contains("manga"))
                    Parameter += "manga=";

                Parameter += afterDotMe.Substring(afterDotMe.IndexOf('/') + 1);
                return true;
            }

            string[] splitOriginalString = originalString.Split('|');
            if (splitOriginalString.Length < 2) return false;

            LaunchReason = ParseLaunchReason(splitOriginalString[0]);

            Parameter = splitOriginalString[1];

            return true;
        }

        public static AnitroLaunchReason ParseLaunchReason(string reason)
        {
            switch (reason)
            {
                case "Search": return AnitroLaunchReason.Search;
                case "GoToDetails": return AnitroLaunchReason.GoToDetails;
                case "StatusUpdate": return AnitroLaunchReason.StatusUpdate;
                case "RateShow": return AnitroLaunchReason.RateShow;
                default: return AnitroLaunchReason.Normal;
            }
        }
    }
}
