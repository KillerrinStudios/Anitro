using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anitro.Data_Structures
{
    public class Settings
    {
        public double AutoGenerateLibraryAfterXDays;

        public Settings()
        {
            AutoGenerateLibraryAfterXDays = 1.0;
        }
        public Settings(Settings settings)
        {
            SetFromSettings(settings);
        }
        public void SetFromSettings(Settings settings)
        {
            AutoGenerateLibraryAfterXDays = settings.AutoGenerateLibraryAfterXDays;

        }

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

    }
}
