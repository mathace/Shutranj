using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spots
{
    public class Spot
    {
        public string GameType { get; set; }
        public string Description { get; set; }
        public string FolderPath { get; set; }
        public string OOPName { get; set; }
        public string OOPPosition { get; set; }
        public string IPName { get; set; }
        public string IPPosition { get; set; }
        public string BB { get; set; }
        public HeroType Herotype { get; set; }
    }
    public enum HeroType { Any, OOP, IP  }
}
