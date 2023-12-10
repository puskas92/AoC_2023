using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2023
{
    public static class CommonFunctions
    {
        public static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }

        public static List<(int X, int Y)> CrossDirections = new List<(int X, int Y)>
        {
            new (1,0),
            new (0, -1),
            new (-1, 0),
            new (0,1)
        };
    }
}
