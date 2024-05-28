using System;
using System.Globalization;
using System.Windows.Data;

namespace MakeoProject.Views
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                // Format date et heure en français
                return dateTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("fr-FR"));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
