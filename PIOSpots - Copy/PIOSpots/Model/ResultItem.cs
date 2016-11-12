using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIOSpots.Model
{
    public class ResultItem
    {
        public DateTime Date { get; set; }
        public string hand { get; set; }
        public string Node { get; set; }
        public string Action { get; set; }
        public double Frequency { get; set; }
        public string CorrectAction { get; set; }
        public double CorrectFrequency { get; set; }
        public double Difference { get; set; }
    }
}
