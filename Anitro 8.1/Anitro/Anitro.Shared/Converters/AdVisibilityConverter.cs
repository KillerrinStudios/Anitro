using Anitro.APIs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Anitro.Converters
{
    public class AdVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string strParameter = parameter as string;
            if (strParameter.Contains("TrueOverride")) { Debug.WriteLine("Forcing Ads On"); return Visibility.Visible; }
            else if (strParameter.Contains("FalseOverride")) { Debug.WriteLine("Forcing Ads Off"); return Visibility.Collapsed; }

            if (InAppPurchaseHelper.licensesOwned.AnitroUnlocked)
            {
                Debug.WriteLine("Turning Off Ads");
                return Visibility.Collapsed;
            }

            Debug.WriteLine("Turning On Ads");
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return new NotImplementedException();
        }
    }
}
