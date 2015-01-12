using System;
using System.Collections.Generic;
using System.Text;
using KillerrinStudiosToolkit;

namespace Anitro.APIs
{
    public partial class StorageTools : KillerrinStudiosToolkit.StorageTools
    {
        public partial class StorageConsts : KillerrinStudiosToolkit.StorageTools.StorageConsts
        {
            public static string UserFile = "User.hmb";
            public static string TestUserFile = "User.hmb";

            public static string SettingsFile = "Settings.hmb";
            public static string LicenseFile = "Licenses.hmb";

            public static string LibraryFile = "Library.hmb";
            public static string AvatarImage = "UserAvatar.jpg";
        }
    }
}
