using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Circle
    {
        public int X { set; get; }
        public int Y { set; get; }
        public Circle()
        {
            X = 0;
            Y = 0;
        }
        public Circle(int X,int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
