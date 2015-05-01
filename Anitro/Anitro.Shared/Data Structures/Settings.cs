using Anitro.APIs;
using Anitro.Data_Structures.Enumerators;
using KillerrinStudiosToolkit.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Data_Structures
{
    public class Settings
    {

        public double AutoGenerateLibraryAfterXDays;
        public SearchFilter SearchFilter;

        public Settings()
        {
            AutoGenerateLibraryAfterXDays = 1.0;
            SearchFilter = SearchFilter.Everything;

        }
        public Settings(Settings settings)
        {
            SetFromSettings(settings);
        }
        public void SetFromSettings(Settings settings)
        {
            AutoGenerateLibraryAfterXDays = settings.AutoGenerateLibraryAfterXDays;
            SearchFilter = settings.SearchFilter;
        }

        #region Save-Setters
        public async Task<bool> SetSearchFilter(SearchFilter filter)
        {
            SearchFilter = filter;
            bool result = await Save();
            return result;
        }
        #endregion

        #region Storage Tools
        #region Json Tools
        public string ThisToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void JsonToThis(string json)
        {
            JObject jObject = JObject.Parse(json);
            Settings settings = JsonConvert.DeserializeObject<Settings>(jObject.ToString());

            SetFromSettings(settings);
        }
        #endregion
        public static async System.Threading.Tasks.Task<Settings> Load()
        {
            Debug.WriteLine("Settings.Load("+"): Entering");
            Settings settings = new Settings();

            if (await StorageTools.DoesFileExist(StorageTools.StorageConsts.SettingsFile))
            {
                try
                {
                    SerializableString json = await StorageTools.LoadFileFromStorage(StorageTools.StorageConsts.SettingsFile);
                    settings.JsonToThis((json.Deserialize() as string));
                }
                catch (Exception)
                {
                    Debug.WriteLine("Settings.Load(" + "): Failed");
                    Consts.AppSettings.DeleteFile();
                    settings = new Settings();
                }
            }

            Debug.WriteLine("Settings.Load(" + "): Exiting");
            return settings;
        }
        public async System.Threading.Tasks.Task<bool> Save()
        {
            try {
                Debug.WriteLine("Save(): Begun");
                StorageTools.isSavingComplete = false;

                string json = ThisToJson();
                bool result = await StorageTools.SaveToStorage(StorageTools.StorageConsts.SettingsFile, new SerializableString(json));

                Debug.WriteLine("Save(): Success!");
                return result;
            }
            catch(Exception) {
                Debug.WriteLine("Save(): Failed");
                StorageTools.isSavingComplete = true;
                return false;
            }
        }
        public async System.Threading.Tasks.Task DeleteFile()
        {
            try
            {
                await StorageTools.DeleteFile(StorageTools.StorageConsts.SettingsFile, Windows.Storage.StorageDeleteOption.PermanentDelete);
            }
            catch (Exception) { }
        }
        #endregion

    }
}
