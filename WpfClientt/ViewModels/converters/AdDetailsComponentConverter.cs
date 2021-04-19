using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfClientt.viewModels.converters {
    /// <summary>
    /// Converts strings of type id-title to long id.
    /// </summary>
    public class AdDetailsComponentConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value != null && typeof(int?).IsAssignableFrom(targetType)) {
                string str = (string)value;
                return long.Parse(str.Substring(0, str.IndexOf("-")));
            }
            return null;
        }
    }
}
