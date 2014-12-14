using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;


#if WINDOWS_PHONE_APP
using Microsoft.Advertising.Mobile.UI;
using Anitro.APIs;
#else
using Microsoft.Advertising.WinRT.UI;
using Anitro.APIs;
#endif

namespace Anitro
{
    public static class XamlControlHelper
    {
        private static bool displayDebugText = false;

        public static void ChangeProgressIndicator(object progressBar, bool isEnabled)
        {
            if (progressBar == null) return;

            if (progressBar is ProgressRing)
            {
                (progressBar as ProgressRing).IsActive = isEnabled; 
                return;
            }

            else if (progressBar is ProgressBar)
            {
                (progressBar as ProgressBar).IsEnabled = isEnabled;
                return;
            }
        }

        public static void SetDebugString(object textBlock, string str, bool forceDisplay = false)
        {
            if (string.IsNullOrEmpty(str)) return;
            Debug.WriteLine(str);

            if (textBlock == null) return;

            if (displayDebugText || forceDisplay) {
                (textBlock as TextBlock).Text = str;
            }
        }

        public static void LoseFocusOnTextBox(object sender)
        {
            if (sender == null) return;

            var control = (sender as Control);
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }

        public static bool AnitroAdControlSettings(object sender)
        {
            if (sender == null) return true;
            var adControl = (sender as AdControl);

            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                Debug.WriteLine("Anitro Unlock Purchased, Turning off Ads");

                try { adControl.IsAutoRefreshEnabled = false; }
                catch (Exception) { }

                adControl.IsEnabled = false;
                adControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                Debug.WriteLine("Ads Turned Off");
                return false;
            }
            else { }

            Debug.WriteLine("AdControl Loaded");
            return true;
        }
    }
}
