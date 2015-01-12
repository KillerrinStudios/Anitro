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
    public class Consts : KillerrinStudiosToolkit.Consts
    {
        // Application Information
#if WINDOWS_PHONE_APP
        public static AnitroApplicationData appData = new AnitroApplicationData("6377b104-7df2-4d29-8ebb-88527728e673",
                                                                                "");
#elif WINDOWS_APP
        public static AnitroApplicationData appData = new AnitroApplicationData("665e06bf-b4de-4cde-8ae3-c20c1a98b12b",
                                                                                "");
#endif

        // Logged-In Settings
        public static User LoggedInUser = new User();
        public static Settings AppSettings = new Settings();

        // Persisting Application Variables
        public static bool justSignedIn = false;
        public static bool forceLibrarySave = false;
        public static bool forceLibraryRemoveal = false;
        public static bool testLibraryLoad = false;
        
        public static bool openedFromProtocolOrTile = false;
        public static bool uriAssociationHandled = false;

        #region Helper Methods
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
    }
}
