using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataBinding
{
    internal class NumToCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //int number = (int)value;
            //string money = number.ToString();
            //var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            //var result = String.Format(info, "{0:c}", number);
            var cul = CultureInfo.GetCultureInfo("vi-VN");
            //money
            
            string result = (string)value;

            if (result != "0")
            {
                result = double.Parse((string)value).ToString("#,# đ", cul.NumberFormat);
            }
            else
            {
                // Do nothing
            }
            
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
