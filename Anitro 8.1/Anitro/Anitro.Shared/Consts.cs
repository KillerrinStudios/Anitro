using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.System;

using Anitro.Data_Structures;
using Windows.UI.Popups;
using Anitro.APIs;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Anitro.Data_Structures.Enumerators;

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

        private static PremiumFeaturesMessageBoxResult PremiumFeaturesMessageBoxResult;
        public static async Task<PremiumFeaturesMessageBoxResult> PremiumFeatureMessageBox(CoreDispatcher dispatcher, Frame frame)
        {
            Debug.WriteLine("PremiumFeatureMessageBox(): User requires the Anitro Unlock");

            PremiumFeaturesMessageBoxResult = PremiumFeaturesMessageBoxResult.OkOrCancelled;

            var messageDialog = new MessageDialog("This feature requires the \"Anitro Unlock\" IAP to use");
            messageDialog.Commands.Add(new UICommand("Ok", delegate(IUICommand command)
                {
                    PremiumFeaturesMessageBoxResult = PremiumFeaturesMessageBoxResult.OkOrCancelled;
                })
            );
            messageDialog.Commands.Add(new UICommand("Purchase", delegate(IUICommand command)
                {
                    InAppPurchaseHelper.PurchaseAnitroUnlock();

                    if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked) { PremiumFeaturesMessageBoxResult = PremiumFeaturesMessageBoxResult.Purchased; }
                    else { PremiumFeaturesMessageBoxResult = PremiumFeaturesMessageBoxResult.OkOrCancelled; }
                })
            );
            //messageDialog.Commands.Add(new UICommand("What's Anitro Unlock?", delegate(IUICommand command)
            //    {
            //        PremiumFeaturesMessageBoxResult = PremiumFeaturesMessageBoxResult.NavigateToUnlock;
            //    })
            //);

            messageDialog.DefaultCommandIndex = 0; // Set the command that will be invoked by default
            messageDialog.CancelCommandIndex = 0; // Set the command to be invoked when escape is pressed
            await messageDialog.ShowAsync();

            return PremiumFeaturesMessageBoxResult;
        }
    }
}
