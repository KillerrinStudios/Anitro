using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Anitro.APIs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Anitro.Data_Structures.Structures
{
    public class LicencesOwned
    {
        public bool AnitroUnlocked = false;

        public LicencesOwned()
        {
            AnitroUnlocked = false;
        }

        public LicencesOwned(LicencesOwned _licensesOwned)
        {
            SetFromLicense(_licensesOwned);
        }

        private void SetFromLicense(LicencesOwned _licensesOwned)
        {
            AnitroUnlocked = _licensesOwned.AnitroUnlocked;
        }

        #region Storage Tools
        #region Json Tools
        public string ThisToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void JsonToThis(string json)
        {
            JObject jObject = JObject.Parse(json);
            LicencesOwned licensesOwned = JsonConvert.DeserializeObject<LicencesOwned>(jObject.ToString());

            SetFromLicense(licensesOwned);
        }
        #endregion
        public static async System.Threading.Tasks.Task<LicencesOwned> Load()
        {
            Debug.WriteLine("Settings.Load(" + "): Entering");
            LicencesOwned licensesOwned = new LicencesOwned();

            if (await StorageTools.DoesFileExist(StorageTools.StorageConsts.LicenseFile))
            {
                try
                {
                    string json = await StorageTools.LoadFileFromStorage(StorageTools.StorageConsts.LicenseFile);
                    licensesOwned.JsonToThis(json);
                }
                catch (Exception)
                {
                    InAppPurchaseHelper.licensesOwned.DeleteFile();
                    licensesOwned = new LicencesOwned();
                }
            }

            return licensesOwned;
        }
        public async System.Threading.Tasks.Task<bool> Save()
        {
            try
            {
                Debug.WriteLine("Save(): Begun");
                StorageTools.isSavingComplete = false;

                string json = ThisToJson();
                bool result = await StorageTools.SaveToStorage(StorageTools.StorageConsts.LicenseFile, new KillerrinStudiosToolkit.Helpers.SaveableString(json));

                Debug.WriteLine("Save(): Success!");
                return result;
            }
            catch (Exception)
            {
                Debug.WriteLine("Save(): Failed");
                StorageTools.isSavingComplete = true;
                return false;
            }
        }
        public async System.Threading.Tasks.Task DeleteFile()
        {
            try
            {
                await StorageTools.DeleteFile(StorageTools.StorageConsts.LicenseFile, Windows.Storage.StorageDeleteOption.PermanentDelete);
            }
            catch (Exception) { }
        }
        #endregion
    }
}
