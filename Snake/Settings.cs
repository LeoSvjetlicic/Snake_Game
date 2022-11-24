using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Settings
    {
        public static int Width { set; get; }
        public static int Height { set; get; }
        public static string Directions;
        public Settings()
        {
            Width = 16;
            Height = 16;
            Directions = "left";
        }
    }
}
