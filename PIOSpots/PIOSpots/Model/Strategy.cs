using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PIOSpots.Model
{
    
    public class StrategyOption
    {
        public static List<Color> ColorList = new List<Color>() { Colors.Crimson, Colors.DodgerBlue, Colors.MediumSeaGreen, Colors.GreenYellow, Colors.DarkOrange, Colors.Lavender };
        public string Title { get; set; }
        public List<double> Frequencies { get; set; }
        public Color DisplayColor { get; set; }
    }    
    public class DisplayStrategyOption
    {
        public Color DisplayColor { get; set; }
        public double Weight { get; set; }
    }
}
