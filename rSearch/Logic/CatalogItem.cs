using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CatalogItem
    {
        public int Stack { get; set; }
        public int Players { get; set; }
        public Positions Position { get; set; }
        public string ImagePath { get; set; }

        public string Description { get; set; }
        public string Comment { get; set; }
    }
}
