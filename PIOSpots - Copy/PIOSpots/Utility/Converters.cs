using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace PIOSpots.Utility
{
    public class CardStringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string pats = value as string;
            if (pats == "") return null;
            if (pats != null)
            {
                pats = Directory.GetCurrentDirectory() + @"/deck/" + pats + ".png";
                BitmapImage image = new BitmapImage();
                using (FileStream Stream = File.OpenRead(pats))
                {
                    image.BeginInit();
                    image.StreamSource = Stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit(); // load tse image from tse Stream
                } // close tse Stream
                return image;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var val1 = 0.0;
            var val2 = 0.0;
            try
            {
                val1 = System.Convert.ToDouble(values[0]);
                val2 = System.Convert.ToDouble(values[1]);
            }
            catch (Exception ex)
            {
                return 0.0;
            }
            return val1*val2;
        }       

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}
