using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Killerrin_Studios_Toolkit;
using Anitro.Models.Enums;
using Windows.UI.Popups;
using Anitro.Helpers;
using Anitro.ViewModels;
using AnimeTrackingServiceWrapper;
using GalaSoft.MvvmLight.Command;

namespace Anitro.Models
{
    public class LicencesOwned : ModelBase
    {
        private bool m_anitroUnlocked = false;
        public bool AnitroUnlocked
        {
            get { return m_anitroUnlocked; }
            set
            {
                m_anitroUnlocked = value;
                RaisePropertyChanged(nameof(AnitroUnlocked));
            }
        }

        public LicencesOwned()
        {
        }

        public LicencesOwned(LicencesOwned _licensesOwned)
        {
            AnitroUnlocked = _licensesOwned.AnitroUnlocked;
        }

        [JsonIgnore]
        public RelayCommand PremiumFeatureMessageBoxCommand { get { return new RelayCommand(() => { PremiumFeatureMessageBox(null); }); } }
        public async Task<PremiumFeaturesMessageBoxResult> PremiumFeatureMessageBox(IProgress<APIProgressReport> progress)
        {
            Debug.WriteLine("PremiumFeatureMessageBox(): User requires the Anitro Unlock");
            PremiumFeaturesMessageBoxResult result = PremiumFeaturesMessageBoxResult.OkOrCancelled;

            var messageDialog = new MessageDialog("This feature requires the \"Anitro Unlock\" IAP to use");
            messageDialog.Commands.Add(new UICommand("Ok", delegate (IUICommand command)
                {
                    result = PremiumFeaturesMessageBoxResult.OkOrCancelled;
                })
            );
            messageDialog.Commands.Add(new UICommand("Purchase", delegate (IUICommand command)
                {
                    InAppPurchaseHelper.PurchaseAnitroUnlock();
                    result = PremiumFeaturesMessageBoxResult.OkOrCancelled;

                    if (MainViewModel.Instance.AnitroLicense.AnitroUnlocked) { result = PremiumFeaturesMessageBoxResult.Purchased; }
                    else { result = PremiumFeaturesMessageBoxResult.OkOrCancelled; }
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

            if (progress != null)
                progress.Report(new APIProgressReport(100.0, "Completed", APIResponse.Successful, result, null));
            return result;
        }

        public override string ToString()
        {
            return "Anitro Unlocked: " + m_anitroUnlocked;
        }

        #region Save/Load/Delete
        public const string Filename = "Anitro.license";
        public async static Task<bool> Save(LicencesOwned anitroLicense)
        {
            Debug.WriteLine("Saving Hummingbird User");

            string json = JsonConvert.SerializeObject(anitroLicense);
            StorageTask storageTask = new StorageTask();
            bool result = await storageTask.CreateFile(StorageTask.LocalFolder, Filename, json);

            return result;
        }

        public async static Task<LicencesOwned> Load()
        {
            Debug.WriteLine("Loading Licences Owned");

            StorageTask storageTask = new StorageTask();
            var storageItem = await storageTask.DoesItemExist(StorageTask.LocalFolder, Filename);
            if (storageItem == null) return new LicencesOwned();

            if (storageItem is Windows.Storage.StorageFile)
            {
                string json = await storageTask.ReadFileString(StorageTask.IStorageItemToStorageFile(storageItem));
                JObject jObject = JObject.Parse(json);
                LicencesOwned licence = JsonConvert.DeserializeObject<LicencesOwned>(jObject.ToString());
                return licence;
            }

            return new LicencesOwned();
        }

        public async static Task<bool> DeleteSavedLicense()
        {
            Debug.WriteLine("Deleting Licences Owned");

            StorageTask storageTask = new StorageTask();
            var storageItem = await storageTask.DoesItemExist(StorageTask.LocalFolder, Filename);

            if (storageItem == null) return false;
            return await storageTask.DeleteItem(storageItem, Windows.Storage.StorageDeleteOption.PermanentDelete);
        }
        #endregion
    }
}
