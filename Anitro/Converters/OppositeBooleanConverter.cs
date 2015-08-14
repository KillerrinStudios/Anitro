﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Anitro.Converters
{
    public class OppositeBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = ((bool)value);
            return !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool b = ((bool)value);
            return !b;
        }
    }
}
