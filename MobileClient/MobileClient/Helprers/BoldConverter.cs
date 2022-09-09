using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MobileClient.Helpers {
    //public class BoldConverter : IValueConverter {
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    //        return ((bool)value) ? new FontAttributes(FontAttributes.Bold) : FontAttributes.None;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
    //        return ((FontAttributes)value) == FontAttributes.Bold;
    //    }
    //}

    public class BoldConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var isBold = (bool)value;
            if (isBold) {
                return FontAttributes.Bold;
            }
            else {
                return FontAttributes.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
