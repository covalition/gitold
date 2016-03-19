using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Convert;

namespace Gitoza
{
    public class MultiplicationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values == null || values.Length != 2)
                return "?";
            Debug.WriteLine(ToDouble(values[0]) * ToDouble(values[1]));
            return ToDouble(values[0]) * ToDouble(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
