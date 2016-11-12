using PIOSpots.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PIOSpots.Utility
{
    public class PioRangeTo169Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)return null;
            var weights = value as List<double>;
            var temp = string.Join(" ", weights);
            var range = Enumerable.Range(1, 169).Select(j => 0.0).ToList();
            for (var i = 0; i < weights.Count; i++)
            {
                var s = Range.Hands1326[i];
                string rep = "";
                if (s[1] == s[3]) rep = s[0].ToString() + s[2] + "s";
                if (s[1] != s[3])
                {
                    if (s[0] != s[2]) rep = s[0].ToString() + s[2] + "o";
                    else rep = s[0].ToString() + s[2];
                }
                var repindex = Range.Hands169.IndexOf(rep);
                range[repindex] += weights[i];
            }
            for (var t = 0; t < range.Count; t++)
            {
                var w = 12;
                var hand = Range.Hands169[t];
                if (hand.Length == 2) w = 6;
                else if (hand[2] == 's') w = 4;
                range[t] = range[t] / w;
            }
            var temp2 = range.Select((item, index) => new { Hand = Range.Hands169[index], Weight = item });
            return range;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
