using AnimeTrackingServiceWrapper.UniversalServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Anitro.Converters
{
    public class BooleanToPrivacySettings : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PrivacySettings b = ((PrivacySettings)value);
            if (b == PrivacySettings.Private) return true;
            else if (b == PrivacySettings.Public) return false;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool b = ((bool)value);
            if (b)
                return PrivacySettings.Private;
            return PrivacySettings.Public;


        }
    }
}
