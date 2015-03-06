using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using KillerrinStudiosToolkit;

namespace Anitro
{
    public class DebugTools : KillerrinStudiosToolkit.DebugTools
    {
        // Testing Code
        public static bool HandleBugSenseInDebug = false;

        public static string testAccountUsername = "killerrin";
        public static string testAccountPassword = "";

        public static void WriteLine(string s, params object[] args)
        {
            Debug.WriteLine(s, args);
        }
    }
}
