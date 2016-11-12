using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Circle
    {
        public double radius;
        public Circle(double r)
        {
            radius = r;
        }
        public double Area()
        {
            return Math.Pow(radius,2) * Math.PI;
        }
    }
}
