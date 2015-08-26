using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Anitro.Controls
{
    public sealed partial class NumericTextBox : TextBox
    {
        public double Number
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Text)) return 0.0;
                return Convert.ToDouble(Text);
            }
            set { Text = value.ToString(); }
        }

        public NumericTextBox()
        {
            this.InitializeComponent();
        }

        public static bool IsOnlyNumbers(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text.Length == 0) return;

            double result;
            if (double.TryParse(Text, out result))
                return;

            Text = Text.Remove(Text.Length - 1);
            SelectionStart = Text.Length;
        }

        private void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            if (Text.Length == 0) return;

            double result;
            if (double.TryParse(Text, out result))
                return;

            Text = "";
        }
    }
}
