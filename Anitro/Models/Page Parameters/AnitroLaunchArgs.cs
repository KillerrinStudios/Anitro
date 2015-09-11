using Anitro.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models.Page_Parameters
{
    public class AnitroLaunchArgs
    {
        public const char LaunchReasonParameterSeperator = '&';
        public const char ParameterParameterSeperator = '=';

        public bool Handled = false;
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

        #region Parameter Parsing
        public bool ParseSecondaryTile(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                Activation = AnitroLaunchActivation.Normal;
                LaunchReason = AnitroLaunchReason.Normal;
                Parameter = "";
                Handled = true;
                return true;
            }

            Activation = AnitroLaunchActivation.SecondaryTile;

            string[] splitSegments = args.Split(LaunchReasonParameterSeperator);
            if (splitSegments.Length < 2)
            {
                Activation = AnitroLaunchActivation.Normal;
                LaunchReason = AnitroLaunchReason.Normal;
                Parameter = "";
                Handled = true;
                return false;
            }

            LaunchReason = ParseLaunchReason(splitSegments[0]);
            Parameter = splitSegments[1];
            Handled = false;
            return true;
        }

        public bool ParseProtocol(Uri protocol)
        {
            // Regular Protocol: anitro://
            // Example:          anitro://https://hummingbird.me/anime/gate-jieitai-kanochi-nite-kaku-tatakaeri
            string originalString = protocol.OriginalString;
            if (string.IsNullOrWhiteSpace(originalString)) return false;

            if (originalString.Contains("hummingbird.me"))
            {
                LaunchReason = AnitroLaunchReason.GoToDetails;

                string afterDotMe = originalString.Substring(originalString.IndexOf(".me") + 4);
                string mediaName = afterDotMe.Substring(afterDotMe.IndexOf('/') + 1);

                if (afterDotMe.ToLower().Contains("anime"))         Parameter = CreateAnimeParameter(mediaName);
                else if (afterDotMe.ToLower().Contains("manga"))    Parameter = CreateMangaParameter(mediaName);
                return true;
            }


            // Regular Protocol: anitro://    
            // Fixed Protocol:   anitro:///   - Because of a bug with Windows and Uris, all regular Protocol URIs must start with an additional '/'
            // Example:          anitro:///GoToDetails&anime=gate-jieitai-kanochi-nite-kaku-tatakaeri
            string[] splitOriginalString = originalString.Substring(originalString.IndexOf(':') + 4).Split(LaunchReasonParameterSeperator);
            if (splitOriginalString.Length < 2) { Debug.WriteLine("Protocol couldn't be parsed"); return false; }

            LaunchReason = ParseLaunchReason(splitOriginalString[0]);
            Parameter = splitOriginalString[1];
            return true;
        }

        public bool ParseSpeechProtocol(string speech)
        {
            Parameter = speech;
            return true;
        }

        public static AnitroLaunchReason ParseLaunchReason(string reason)
        {
            //Debug.WriteLine(string.Format("ParseLaunchReason({0})", reason));
            switch (reason)
            {
                case "Search": return AnitroLaunchReason.Search;
                case "GoToDetails": return AnitroLaunchReason.GoToDetails;
                case "ShowMyLibrary": return AnitroLaunchReason.ShowMyLibrary;
                case "StatusUpdate": return AnitroLaunchReason.StatusUpdate;
                case "RateShow": return AnitroLaunchReason.RateShow;
                case "Normal":
                case "comedic_favouriteAnime":
                case "comedic_wiafu":
                default: return AnitroLaunchReason.Normal;
            }
        }
        #endregion

        public string GetWorthwhileParameter()
        {
            string[] splitSegments = Parameter.Split(ParameterParameterSeperator);
            if (splitSegments.Length == 1) return splitSegments[0];
            else if (splitSegments.Length >= 2)
                return splitSegments[1];
            return "";
        }

        public string CreateProtocol()
        {
            return LaunchReason.ToString() + LaunchReasonParameterSeperator + Parameter;
        }

        public override string ToString()
        {
            return string.Format("Handled: {0}, Activation: {1} | Launch Reason: {2} | Parameter: {3}", Handled.ToString(), Activation.ToString(), LaunchReason.ToString(), Parameter);
        }

        #region Create Parameter
        public static string CreateAnimeParameter(string animeID)   { return "anime"  + ParameterParameterSeperator + animeID; }
        public static string CreateMangaParameter(string mangaID)   { return "manga"  + ParameterParameterSeperator + mangaID; }
        public static string CreateSearchParameter(string query)    { return "query"  + ParameterParameterSeperator + query;   }
        public static string CreateStatusParameter(string status)   { return "status" + ParameterParameterSeperator + status;  }
        public static string CreateLibraryParameter(string library) { return "libray" + ParameterParameterSeperator + library; }
        #endregion
    }
}
