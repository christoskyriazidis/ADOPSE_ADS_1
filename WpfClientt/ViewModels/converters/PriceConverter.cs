using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfClientt.viewModels.converters {
    public class PriceConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? 0 : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            int price = 0;
            if(value != null && value.ToString().Length > 0) {
                try {
                    price = int.Parse((string)value);
                }catch(FormatException ignored) {//If coudln't format,price stays 0.
                }
            }

            return price >= 0 ? price : 0;
        }
    }
}
