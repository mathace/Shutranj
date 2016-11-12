using PIOSpots.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PIOSpots.Utility
{
    public class StrategyOptionToDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var val = value as List<StrategyOption>;
            val.ForEach(so => so.Frequencies = RangeOrders.ToRange169(so.Frequencies));
            var ret = new List<List<DisplayStrategyOption>>();
            //for (int i=0;i<val.Count;i++)
            //{
            //    var option = val[i];
            //    for(int j=0;j<option.Frequencies.Count;j++)
            //    {
            //        ret[i].Add(new DisplayStrategyOption() { DisplayColor = val[i].DisplayColor, Weight = val[i].Frequencies[j] });
            //    }
            //}
            for (int i = 0; i < 169; i++)
            {
                var item = new List<DisplayStrategyOption>();
                foreach (var opt in val)
                {
                    item.Add(new DisplayStrategyOption()
                    {
                        DisplayColor = opt.DisplayColor,
                        Weight = opt.Frequencies[i]
                    });
                }
                ret.Add(item);
            }
        
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
