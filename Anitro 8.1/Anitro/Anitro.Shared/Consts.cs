using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.System;

using Anitro.Data_Structures;

namespace Anitro
{
    public static class Consts
    {
        // Application Information
        public static KillerrinApplicationData appData = new KillerrinApplicationData();

        // Logged-In Settings
        public static User LoggedInUser = new User();
        public static Settings AppSettings = new Settings();

        // Persisting Application Variables
        public static bool isApplicationClosing = false;
        public static bool justSignedIn = false;
        public static bool forceLibrarySave = false;
        public static bool forceLibraryRemoveal = false;
        public static bool testLibraryLoad = false;
        
        public static bool openedFromProtocolOrTile = false;
        public static bool uriAssociationHandled = false;

        public static Random random = new Random();

        #region Helper Methods
        public static bool IsConnectedToInternet() { return NetworkInterface.GetIsNetworkAvailable(); }

        public static async Task<bool> LaunchReview()
        {
#if WINDOWS_PHONE_APP
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid="+Consts.appData.AnitroPackageName));
#else
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:review?PFN="+Consts.appData.AnitroPackageName));
#endif
            return result;
        }

        public static LauncherOptions AnitroCompanionLauncherOptions
        {
            get
            {
                return new LauncherOptions()
                {
                    TreatAsUntrusted = false,
                    PreferredApplicationDisplayName = "Anitro Companion",
                    PreferredApplicationPackageFamilyName = "59141KillerrinStudios.AnitroCompanion_vsha6q420kgm4",
                    FallbackUri = new Uri("http://www.windowsphone.com/en-us/store/app/anitro-companion/5aabf421-ec67-4a15-8bf6-b441ce8bc49e", UriKind.Absolute),
                };
            }
        }

        public static Uri CreateAnitroCompanionUri(string slug = "")
        {
            if (string.IsNullOrEmpty(slug))
            {
                return new Uri("anitrols:" + slug, UriKind.Absolute);
            }
            else
            {
                return new Uri("anitrols:type=Anime&args=" + slug, UriKind.Absolute);
            }
        }
        #endregion

        public static void UpdateLoggedInUser(User user)
        {
            if (user.IsLoggedIn)
            {
                LoggedInUser = new User(user);
            }
        }

        #region Extension Methods
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++) {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
        #endregion
    }
}
