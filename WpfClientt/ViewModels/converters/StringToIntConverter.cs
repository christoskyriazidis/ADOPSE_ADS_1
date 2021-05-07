using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfClientt.viewModels {
    /// <summary>
    /// Converts string to int.
    /// </summary>
    public class StringToIntConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? 0 : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int result = -1;
            if(value != null && value.ToString().Length > 0) {
                try {
                    result = int.Parse((string)value);
                }catch(FormatException) {
                }
            }

            return result;
        }
    }
}
