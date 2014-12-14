using Anitro.Data_Structures.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;


namespace Anitro.APIs
{
    public static class InAppPurchaseHelper
    {
        public static class ProductIDs
        {
            public static string UnlockAnitro = "UnlockAnitro";
        }

        public static LicenseInformation licenseInfo
        {
            get
            {
                if (DebugTools.DebugMode) return CurrentAppSimulator.LicenseInformation;
                return CurrentApp.LicenseInformation;
            }
        }

        public static ListingInformation listingInformation;
        public static Guid guid;
        public static Uri linkUri;

        public static bool latestLicensePulled = false;

        private static string[] productWhitelist =
        {
            "killerrin",
            "fruroar"
        };

        public static LicencesOwned licensesOwned = new LicencesOwned();

        public static async Task<bool> CheckProductInformation()
        {
            Debug.WriteLine("CheckProductInformation(): Entering");

            try
            {
                if (DebugTools.DebugMode)
                {
                    Windows.Storage.StorageFile windowsStoreProxyFile = await StorageTools.LoadStorageFileFromPackage("", "WindowsStoreProxy.xml");
                    await CurrentAppSimulator.ReloadSimulatorAsync(windowsStoreProxyFile);
                    Debug.WriteLine("Simulator Reloaded");

                    listingInformation = await CurrentAppSimulator.LoadListingInformationAsync();
                    Debug.WriteLine("ListingInformation Recieved");

                    guid = CurrentAppSimulator.AppId;
                    linkUri = CurrentAppSimulator.LinkUri;
                    Debug.WriteLine("Guid and LinkUri Set");

                    CurrentAppSimulator.LicenseInformation.LicenseChanged += licenseInfo_LicenseChanged;
                    Debug.WriteLine("LicenseInformation Recieved");
                }
                else
                {
                    listingInformation = await CurrentApp.LoadListingInformationAsync();
                    Debug.WriteLine("ListingInformation Recieved");

                    guid = CurrentApp.AppId;
                    linkUri = CurrentApp.LinkUri;
                    Debug.WriteLine("Guid and LinkUri Set");

                    CurrentApp.LicenseInformation.LicenseChanged += licenseInfo_LicenseChanged;
                    Debug.WriteLine("LicenseInformation Recieved");
                }


                latestLicensePulled = await CheckIfCustomerOwnsLicenses();
            }
            catch (Exception ex) {
                DebugTools.PrintOutException("License Load failed", ex);
                latestLicensePulled = false;
            
            }

            if (!latestLicensePulled)
            {
                Debug.WriteLine("CheckProductInformation() Failed");
                licensesOwned = await LicencesOwned.Load();

                return false;
            }

            return true;
        }

        public static async Task<bool> CheckIfCustomerOwnsLicenses()
        {
            try
            {
                ProductLicense UnlockAnitro = licenseInfo.ProductLicenses[ProductIDs.UnlockAnitro];
                if (UnlockAnitro.IsActive)
                {
                    licensesOwned.AnitroUnlocked = true;

                    Debug.WriteLine("Product is owned");
                    Debug.WriteLine(UnlockAnitro.ProductId);
                    Debug.WriteLine(UnlockAnitro.ExpirationDate);
                    Debug.WriteLine(UnlockAnitro.IsActive);
                }

                await licensesOwned.Save();
            }
            catch (Exception) { return false; }

            if (!DebugTools.DebugMode)
            {
                if (Consts.LoggedInUser.IsLoggedIn)
                {
                    bool userOnWhitelist = CheckProductOwnerWhitelist(Consts.LoggedInUser.Username);
                    if (userOnWhitelist)
                    {
                        Debug.WriteLine("User is on Whitelist, Enabling Anitro Unlock");
                        licensesOwned.AnitroUnlocked = true;
                    }
                }
            }

            return true;
        }

        public static bool CheckProductOwnerWhitelist (string _username)
        {
            foreach(string userWL in productWhitelist) {
                if (userWL.ToLower() == _username.ToLower()) {
                    return true;
                }
            }

            return false;
        }

        public static async void PurchaseAnitroUnlock()
        {
            if (licenseInfo.ProductLicenses[ProductIDs.UnlockAnitro].IsActive) {
                Debug.WriteLine("User already owns this product");
                return;
            }

            ///http://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn532254.aspx
            PurchaseResults purchaseResults;

            try
            {
                if (DebugTools.DebugMode)
                {
                    Debug.WriteLine("Debug mode active, Simulating product purchase");
                    purchaseResults = await CurrentAppSimulator.RequestProductPurchaseAsync(ProductIDs.UnlockAnitro);
                    Debug.WriteLine("Finished Simulating");
                }
                else
                {
                    Debug.WriteLine("Requesting Product Purchase");
                    purchaseResults = await CurrentApp.RequestProductPurchaseAsync(ProductIDs.UnlockAnitro);
                    Debug.WriteLine("User finished interacting with purchase screen");
                }

                switch (purchaseResults.Status)
                {
                    case ProductPurchaseStatus.AlreadyPurchased:
                        Debug.WriteLine("User already owns this product");

                        if (!DebugTools.DebugMode)
                        {
                            licensesOwned.AnitroUnlocked = true;
                            await licensesOwned.Save();
                        }
                        break;
                    case ProductPurchaseStatus.Succeeded:
                        Debug.WriteLine("User now owns this product");

                        if (licenseInfo.ProductLicenses[ProductIDs.UnlockAnitro].IsActive)
                        {
                            licensesOwned.AnitroUnlocked = true;
                            await licensesOwned.Save();
                        }
                        break;

                    case ProductPurchaseStatus.NotPurchased:
                        Debug.WriteLine("User chose to not purchase the product");
                        if (DebugTools.DebugMode)
                        {
                            Debug.WriteLine("Simulating Purchase");
                            licensesOwned.AnitroUnlocked = true;
                        }
                        break;
                    case ProductPurchaseStatus.NotFulfilled:
                        Debug.WriteLine("A previous purchase was not fulfilled");
                        break;
                    default:
                        Debug.WriteLine("An unknown response occured");
                        break;
                }
            }
            catch (Exception ex) 
            {
                DebugTools.PrintOutException("Product Purchase Error (" + ProductIDs.UnlockAnitro + ")", ex);
            }
        }

        private static void licenseInfo_LicenseChanged()
        {
            Debug.WriteLine("licenseInfo_LicenseChanged()");

            if (DebugTools.DebugMode) { }
            else { }
        }
    }
}
