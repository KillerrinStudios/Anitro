using Anitro.APIs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Anitro
{
    public sealed partial class UnlockAnitroFlyout : SettingsFlyout
    {
        public UnlockAnitroFlyout()
        {
            this.InitializeComponent();
            SetUnlockView();
        }

        private void SetUnlockView()
        {
            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                Debug.WriteLine("Product Owned, Setting View");

                unlockAnitro_Button.IsEnabled = false;
                purchaseThankYouMessage.Visibility = Windows.UI.Xaml.Visibility.Visible;
                purchasedText.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private async void UnlockAnitro_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked) return;
            Debug.WriteLine("UnlockAnitro_Button_Tapped()");

            InAppPurchaseHelper.PurchaseAnitroUnlock();

            SetUnlockView();
        }
    }
}
