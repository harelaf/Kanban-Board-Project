using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Presentation.View
{
    public class HeightConverter : IValueConverter
    {
        /// <summary>
        /// this converter is only for the purpose of keeping the listbox of the columns alligned with the window's height.
        /// gets the actual window height and returns the height for the list. used in the xaml.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double x = (Double)value;
            if (x > 110)
            {
                return x - 110;
            }
            return value;
        }

        //not needed. gust for implementation of the interface
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
