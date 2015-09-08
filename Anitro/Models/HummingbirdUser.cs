using AnimeTrackingServiceWrapper.Implementation.HummingbirdV1.Models;
using AnimeTrackingServiceWrapper.UniversalServiceModels.ActivityFeed;
using Killerrin_Studios_Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class HummingbirdUser : User
    {
        [JsonIgnore]
        private ObservableCollection<AActivityFeedItem> m_activityFeed = new ObservableCollection<AActivityFeedItem>();
        [JsonIgnore]
        public ObservableCollection<AActivityFeedItem> ActivityFeed
        {
            get { return m_activityFeed; }
            set
            {
                if (m_activityFeed == value) return;
                m_activityFeed = value;
                RaisePropertyChanged(nameof(ActivityFeed));
            }
        }

        private UserObjectHummingbirdV1 m_hummingbirdUserInfo = new UserObjectHummingbirdV1();
        public UserObjectHummingbirdV1 HummingbirdUserInfo
        {
            get { return m_hummingbirdUserInfo; }
            set
            {
                if (m_hummingbirdUserInfo == value) return;
                m_hummingbirdUserInfo = value;
                RaisePropertyChanged(nameof(HummingbirdUserInfo));
            }
        }

        public HummingbirdUser()
        {
            Service = AnimeTrackingServiceWrapper.ServiceName.Hummingbird;
            UserInfo.AvatarUrl = new Uri("http://www.example.com/", UriKind.Absolute);
            UserInfo.Username = "";
        }

        #region Save/Load/Delete
        public static string Filename { get; } = "Hummingbird.user";
        public async static Task<bool> Save(HummingbirdUser user)
        {
            Debug.WriteLine("Saving Hummingbird User");

            string json = JsonConvert.SerializeObject(user);
            StorageTask storageTask = new StorageTask();
            bool result = await storageTask.CreateFile(StorageTask.LocalFolder, Filename, json);

            return result;
        }

        public async static Task<HummingbirdUser> Load()
        {
            Debug.WriteLine("Loading Hummingbird User");

            StorageTask storageTask = new StorageTask();
            var storageItem = await storageTask.DoesItemExist(StorageTask.LocalFolder, Filename);
            if (storageItem == null) return new HummingbirdUser();

            if (storageItem is Windows.Storage.StorageFile)
            {
                string json = await storageTask.ReadFileString(StorageTask.IStorageItemToStorageFile(storageItem));
                JObject jObject = JObject.Parse(json);
                HummingbirdUser user = JsonConvert.DeserializeObject<HummingbirdUser>(jObject.ToString());
                return user;
            }

            return new HummingbirdUser();
        }

        public async static Task<bool> DeleteUser()
        {
            Debug.WriteLine("Deleting Hummingbird User");

            StorageTask storageTask = new StorageTask();
            var storageItem = await storageTask.DoesItemExist(StorageTask.LocalFolder, Filename);

            if (storageItem == null) return false;
            return await storageTask.DeleteItem(storageItem, Windows.Storage.StorageDeleteOption.PermanentDelete);
        }
        #endregion
    }
}
