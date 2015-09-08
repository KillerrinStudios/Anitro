using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace Killerrin_Studios_Toolkit
{
    public enum StorageLocationPrefix
    {
        None,
        Solution,
        LocalFolder,
        RoamingFolder,
        TempFolder
    }

    public class StorageTask
    {
        public static StorageTask Instance { get; } = new StorageTask();

        #region Properties
        #region Prefix's and FileNames
        public const string SolutionPrefix = "ms-appx:///";
        public const string LocalFolderPrefix = "ms-appdata:///local/";
        public const string RoamingFolderPrefix = "ms-appdata:///roaming/";
        public const string TempFolderPrefix = "ms-appdata:///temp/";

        public static Uri CreateUri(StorageLocationPrefix prefix, string path)
        {
            switch (prefix)
            {
                case StorageLocationPrefix.Solution: return new Uri(SolutionPrefix + path, UriKind.Absolute);
                case StorageLocationPrefix.LocalFolder: return new Uri(LocalFolderPrefix + path, UriKind.Absolute);
                case StorageLocationPrefix.RoamingFolder: return new Uri(RoamingFolderPrefix + path, UriKind.Absolute);
                case StorageLocationPrefix.TempFolder: return new Uri(TempFolderPrefix + path, UriKind.Absolute);
                case StorageLocationPrefix.None: 
                default: return new Uri(path, UriKind.RelativeOrAbsolute);
            }
        }
        #endregion

        #region Storage Folders
        public static StorageFolder PackageFolder { get { return Windows.ApplicationModel.Package.Current.InstalledLocation; } }

        public static StorageFolder LocalCacheFolder { get { return ApplicationData.Current.LocalCacheFolder; } }
        public static StorageFolder LocalFolder { get { return ApplicationData.Current.LocalFolder; } }
        public static StorageFolder SharedLocalFolder { get { return ApplicationData.Current.SharedLocalFolder; } }

        public static StorageFolder TemporaryFolder { get { return ApplicationData.Current.TemporaryFolder; } }

        public static StorageFolder RoamingFolder { get { return ApplicationData.Current.RoamingFolder; } }
        public static ulong RoamingStorageQuota { get { return ApplicationData.Current.RoamingStorageQuota; } }
        #endregion

        #region Settings Data Containers
        public static ApplicationDataContainer LocalSettings { get { return ApplicationData.Current.LocalSettings; } }
        public static ApplicationDataContainer RoamingSettings { get { return ApplicationData.Current.RoamingSettings; } }
        #endregion
        #endregion

        public StorageTask()
        {
        }

        public static PasswordVault GetPasswordVault()
        {
            PasswordVault vault = new PasswordVault();
            return vault;
        }

        #region Get Files/Folders
        public StorageFolder GetPublisherCacheFolder(string folderName)
        {
            return ApplicationData.Current.GetPublisherCacheFolder(folderName);
        }
        public async Task ClearPublisherCacheFolderAsync(string folderName)
        {
            await ApplicationData.Current.ClearPublisherCacheFolderAsync(folderName);
        }


        public async Task<StorageFile> GetFile(StorageFolder folder, string fileName)
        {
            return await folder.GetFileAsync(fileName);
        }
        public async Task<StorageFolder> GetFolder(StorageFolder folder, string folderName)
        {
            return await folder.GetFolderAsync(folderName);
        }
        public async Task<IReadOnlyList<IStorageItem>> GetItems(StorageFolder folder, CommonFileQuery query)
        {
            return await folder.GetItemsAsync();
        }
        #endregion

        #region Create/Read
        #region Create
        public async Task<bool> CreateFile(StorageFolder folder, string fileName, IBuffer buffer)
        {
            CreationCollisionOption collisionOption = CreationCollisionOption.ReplaceExisting;

            StorageFile file = await folder.CreateFileAsync(fileName, collisionOption);
            await FileIO.WriteBufferAsync(file, buffer);
            return true;
        }
        public async Task<bool> CreateFile(StorageFolder folder, string fileName, string content)
        {
            CreationCollisionOption collisionOption = CreationCollisionOption.ReplaceExisting;

            StorageFile file = await folder.CreateFileAsync(fileName, collisionOption);
            await FileIO.WriteTextAsync(file, content);
            return true;
        }
        #endregion

        #region Read
        public async Task<IBuffer> ReadFileBuffer(StorageFile file)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            return buffer;
        }
        public async Task<string> ReadFileString(StorageFile file)
        {
            return await FileIO.ReadTextAsync(file);
        }
        public async Task<byte[]> ReadFileBytes(StorageFile file)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            DataReader reader = DataReader.FromBuffer(buffer);

            byte[] bytes = new byte[buffer.Length];
            reader.ReadBytes(bytes);
            return bytes;
        }
        #endregion
        #endregion

        #region Helpers
        public async Task<IStorageItem> DoesItemExist(StorageFolder folder, string fileName)
        {
            var storageItem = await folder.TryGetItemAsync(fileName);
            return storageItem;
        }
        public async Task<bool> DeleteItem(IStorageItem item, StorageDeleteOption deletionOption)
        {
            await item.DeleteAsync(deletionOption);
            return true;
        }
        #endregion

        #region Converters
        public static StorageFile IStorageItemToStorageFile(IStorageItem item)
        {
            if (item is StorageFile) return (StorageFile)item;
            return null;
        }
        public static StorageFolder IStorageItemToStorageFolder(IStorageItem item)
        {
            if (item is StorageFolder) return (StorageFolder)item;
            return null;
        }

        public static IStorageItem StorageFileToIStorageItem(StorageFile file)
        {
            return (IStorageItem)file;
        }
        public static IStorageItem StorageFolderToIStorageItem(StorageFolder folder)
        {
            return (IStorageItem)folder;
        }
        #endregion
    }
}
