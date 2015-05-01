using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Anitro.APIs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using KillerrinStudiosToolkit.Helpers;
using KillerrinStudiosToolkit.Interfaces;
using Anitro.Data_Structures.API_Classes;

namespace Anitro.Data_Structures
{
    public enum UserSaveSettings
    {
        All,
        JustUser
    }

    public class User : ISerializable
    {
        public bool IsLoggedIn
        {
            get
            {
                bool iLI = (authToken != "") && (Username != "");

                Debug.WriteLine("User Logged In: " + iLI.ToString());
                return iLI;
            }
        }

        public bool IsTestAccount { get { return Username == "killerrindev"; } }

        public string AuthToken
        {
            get
            {
                if (IsLoggedIn) {
                    Debug.WriteLine("Grabbing users Auth Token: "+ authToken);
                    return authToken;
                }
                else { 
                    Debug.WriteLine("Grabbing valid-placeholder AuthToken");
                    return "swmRwwrimshWG8EtjKZK"; 
                }
            }
            set { authToken = value; }
        }
        private string authToken;

        public string Username; // { get; set; }
        public string AvatarURL;
        public string EmailAddress; // { get; set; }
        public string Password; // { get; set; }

        public UserInfo UserInfo;

        public Library animeLibrary;
        public ObservableCollection<ActivityFeedObject> activityFeed;


        public User()
        {
            Username = "";
            Password = "";
            EmailAddress = "";
            AuthToken = "";
            AvatarURL = "";

            UserInfo = new API_Classes.UserInfo();

            animeLibrary = new Library("");
            activityFeed = new ObservableCollection<ActivityFeedObject>();
        }
        public User(string _username) : this()
        {
            Username        = _username;
            animeLibrary.Owner   = _username;
        }
        public User(User storageUser)
        {
            SetFromOtherUser(storageUser);
        }

        private void SetFromOtherUser(User otherUser)
        {
            if (otherUser != null) {
                Username = otherUser.Username;
                Password = otherUser.Password;

                EmailAddress = otherUser.EmailAddress;
                
                AuthToken = otherUser.AuthToken;
                AvatarURL = otherUser.AvatarURL;

                //-- Load UserInfo from actual storage
                if (otherUser.UserInfo == null)
                    UserInfo = new UserInfo();
                else
                    UserInfo = otherUser.UserInfo;

                //-- Load Library from actual storage
                if (otherUser.animeLibrary == null)
                    animeLibrary = new Library(otherUser.Username);
                else
                    animeLibrary = new Library(otherUser.animeLibrary);
            }
            else {
                UserInfo = new UserInfo();
                animeLibrary = new Library("");
            }

            // Create activity feed to be populated later
            activityFeed = new ObservableCollection<ActivityFeedObject>();
        }
        public User CreateStorageObject()
        {
            User storageUser = new User(Username);
            storageUser.Password = Password;
            storageUser.EmailAddress = EmailAddress;
            storageUser.AuthToken = AuthToken;
            storageUser.AvatarURL = AvatarURL;

            storageUser.UserInfo = UserInfo;

            storageUser.animeLibrary = null;
            storageUser.activityFeed = null;

            return storageUser;
        }

        #region Helper Methods
        public void Login(string _user, string _auth, string pass, string email = "")
        {
            Username = _user;
            animeLibrary.Owner = _user;

            EmailAddress = email;

            AuthToken = _auth;
            Password = pass;

            animeLibrary.ClearLibrary(LibrarySelection.All);

            Consts.justSignedIn = true;
        }
        public void Logout()
        {
            Username = "";
            animeLibrary.Owner = "";
            EmailAddress = "";
            
            AuthToken = "";
            Password = "";

            AvatarURL = "";

            UserInfo = new UserInfo();

            animeLibrary.ClearLibrary(LibrarySelection.All);
            activityFeed = new ObservableCollection<ActivityFeedObject>();
        }
        #endregion

        #region Storage Tools
        #region Json Tools
        public string ThisToJson(UserSaveSettings saveSettings = UserSaveSettings.All)
        {
            Debug.WriteLine("ThisToJson(): Converting to JSON");
            switch (saveSettings)
            {
                case UserSaveSettings.All:
                    return JsonConvert.SerializeObject(this);
                case UserSaveSettings.JustUser:
                    return JsonConvert.SerializeObject(CreateStorageObject());
                default:
                    return "";
            }
        }

        public void JsonToThis(string json)
        {
            JObject jObject = JObject.Parse(json);
            User storageUser = JsonConvert.DeserializeObject<User>(jObject.ToString());

            SetFromOtherUser(storageUser);
        }
        #endregion

        #region Serializable Implimentation
        byte[] ISerializable.Serialize() { return Serialize(); }
        public byte[] Serialize(UserSaveSettings saveSettings = UserSaveSettings.All) 
        {
            string json = ThisToJson(saveSettings);
            SerializableString serializedString = new SerializableString(json);
            return serializedString.Serialize();
        }

        object ISerializable.Deserialize() { return Deserialize(); }
        public object Deserialize()
        {
            return "";
        }
        #endregion

        public async Task<bool> Save(UserSaveSettings saveSettings = UserSaveSettings.All)
        {
            try {
                Debug.WriteLine("Save(): Begun");
                StorageTools.isSavingComplete = false;

                bool result = await StorageTools.SaveToStorage(StorageTools.StorageConsts.UserFile, this);

                Debug.WriteLine("Save(): Success: " + result);
                return result;
            }
            catch (Exception) {
                Debug.WriteLine("Save(): Failed");
                StorageTools.isSavingComplete = true;
                return false;
            }
        }
        public static async Task<User> Load(bool reobtainAuthentication, bool testData = false)
        {
            Debug.WriteLine("User.Load(" + reobtainAuthentication + ", " + testData + "): Entering");
            User user = new User();

            if (!testData)
            {
                if (await StorageTools.DoesFileExist(StorageTools.StorageConsts.UserFile))
                {
                    try
                    {
                        SerializableString json = await StorageTools.LoadFileFromStorage(StorageTools.StorageConsts.UserFile);
                        string jsonDeserializedString = (json.Deserialize() as string);
                        Debug.WriteLine(jsonDeserializedString);
                        user.JsonToThis(jsonDeserializedString);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("User.Load(" + testData + "): Failed");
                        Consts.LoggedInUser.DeleteFile();
                        user = new User();
                    }
                }
            }
            else
            {
                SerializableString json = await StorageTools.LoadPackagedFile(StorageTools.StorageConsts.AssetsFolder, StorageTools.StorageConsts.TestUserFile);
                user.JsonToThis((json.Deserialize() as string));
            }

            User previousUser = Consts.LoggedInUser;
            Consts.LoggedInUser = user;

            if (reobtainAuthentication) {
                await APIs.Hummingbird.APIv1.Post.Login(user.Username, user.Password, true);
            }


            Debug.WriteLine("User.Load(" + reobtainAuthentication + ", " + testData + "): Exiting");
            return user;
        }
        public async Task DeleteFile()
        {
            try
            {
                await StorageTools.DeleteFile(StorageTools.StorageConsts.UserFile, Windows.Storage.StorageDeleteOption.PermanentDelete);
            }
            catch (Exception) { }
        }
        #endregion
    }
}
