using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;

using KillerrinStudiosToolkit;

#if WINDOWS_PHONE_APP
using Microsoft.Advertising.Mobile.UI;
using Anitro.APIs;
#else
using Microsoft.Advertising.WinRT.UI;
using Anitro.APIs;
#endif

namespace Anitro
{
    public class XamlControlHelper : KillerrinStudiosToolkit.XamlControlHelper
    {
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

#if WINDOWS_PHONE_APP
        public static bool AnitroAdMediatorSettings(object sender)
        {
            if (sender == null) return true;
            var adControl = (sender as Microsoft.AdMediator.WindowsPhone81.AdMediatorControl);

            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                Debug.WriteLine("Anitro Unlock Purchased, Turning off Ads");

                adControl.IsEnabled = false;
                adControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //adControl.Disable();
                Debug.WriteLine("Ads Turned Off");
                return false;
            }
            else { }

            Debug.WriteLine("AdControl Loaded");
            return true;
        }
#endif
    }
}
