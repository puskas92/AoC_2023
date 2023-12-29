using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day24
    {
        public class Day24_Input : List<Day24_Hailstone> //Define input type
        {
        }

        public record struct Day24_Hailstone((long X, long Y, long Z) p, (long X, long Y, long Z) v);
        public static void Day24_Main()
        {
            var input = Day24_ReadInput();
            Console.WriteLine($"Day24 Part1: {Day24_Part1(input, 200000000000000, 400000000000000)}");
            Console.WriteLine($"Day24 Part2: {Day24_Part2(input)}");
        }

        public static Day24_Input Day24_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day24\\Day24_input.txt").ReadToEnd();
            }

            var result = new Day24_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var splitted = line.Split('@');
                var pos = splitted[0].Trim().Split(',').Select(s => long.Parse(s.Trim())).ToList();
                var vel = splitted[1].Trim().Split(',').Select(s => long.Parse(s.Trim())).ToList();
                result.Add(new Day24_Hailstone((pos[0], pos[1], pos[2]), (vel[0], vel[1], vel[2])));
            }

            return result;
        }


        public static long Day24_Part1(Day24_Input input, long LimitLow, long LimitHigh)
        {
            long collosionCount = 0;
            for(var i = 0; i < input.Count()-1; i++)
            {
                for(var j = i+1;  j < input.Count(); j++)
                {
                    var test1 = input[i];
                    var test2 = input[j];

                    var a1 = test1.v.Y;
                    var b1 = -test1.v.X;
                    var c1 = test1.p.Y * test1.v.X - test1.p.X * test1.v.Y;

                    var a2 = test2.v.Y;
                    var b2 = -test2.v.X;
                    var c2 = test2.p.Y * test2.v.X - test2.p.X * test2.v.Y;

                    var q = (double)(a1 * b2 - a2 * b1);
                    if (q == 0) continue;

                    double x = ((double)(b1 / q) * c2) - ((double)(b2 / q) * c1);
                    double y = ((double)(a2 / q) * c1) - ((double)(a1 / q) * c2);

                    if (x > LimitHigh || x < LimitLow) continue;
                    if (y > LimitHigh || y < LimitLow) continue;

                    var t1 = (double)(x - test1.p.X) / (test1.v.X);
                    var t2 = (double)(x - test2.p.X) / (test2.v.X);
                    
                    if(t1<0) continue; 
                    if(t2<0) continue;

                    collosionCount++;
                }
            }
            return collosionCount;
        }

        public static bool Day24_IsHailstoneCollide(Day24_Hailstone test1, Day24_Hailstone test2)
        {
            if ((test1.v.X - test2.v.X) == 0) return false;

            double t = -1 * (double)(test1.p.X - test2.p.X) / (double)(test1.v.X - test2.v.X);

            var x = test1.p.X + t * test1.v.X;

            var y1 = test1.p.Y + t * test1.v.Y;
            var y2 = test2.p.Y + t * test2.v.Y;
            if (y1 != y2) return false;

            var z1 = test1.p.Z + t * test1.v.Z;
            var z2 = test2.p.Z + t * test2.v.Z;
            if (z1 != z2) return false;

            return true;
        }
        public static long Day24_Part2(Day24_Input input)
        {
            //double t1 = 0, t2 = 0;
            //int i;
            //Day24_Hailstone test1 = input.First();
            //Day24_Hailstone test2 = input.First();

            //for (i = 0; i < input.Count; i++)
            //{
            //    test1 = input[i];
            //    test2 = input[i+1];

            //    var a1 = test1.v.Y;
            //    var b1 = -test1.v.X;
            //    var c1 = test1.p.Y * test1.v.X - test1.p.X * test1.v.Y;

            //    var a2 = test2.v.Y;
            //    var b2 = -test2.v.X;
            //    var c2 = test2.p.Y * test2.v.X - test2.p.X * test2.v.Y;

            //    var q = (double)(a1 * b2 - a2 * b1);
            //    if (q == 0) continue;

            //    double x = ((double)(b1 / q) * c2) - ((double)(b2 / q) * c1);
            //    double y = ((double)(a2 / q) * c1) - ((double)(a1 / q) * c2);

            //    t1 = (double)(x - test1.p.X) / (test1.v.X);
            //    t2 = (double)(x - test2.p.X) / (test2.v.X);

            //    if (t1 < 0) continue;
            //    if (t2 < 0) continue;
            //    break;
            //}

            //var toTestHailstones = input.Select((x, j) => (j != i && j != (i + 1)));

            ////long tMin = (long)Math.Floor(Math.Max((Math.Min(t1, t2) - Math.Abs(t2-t1)),0));
            ////long tMax = (long)Math.Ceiling((Math.Max(t1, t2) + Math.Abs(t2 - t1)));

            //long tMin = (long)Math.Floor(Math.Min(t1, t2));
            //long tMax = (long)Math.Ceiling(Math.Max(t1, t2));

            //long px = 0, py = 0, pz = 0;
            //long vx = 0, vy = 0, vz = 0;
            //Day24_Hailstone rock = new Day24_Hailstone((px, py, pz), (vx, vy, vz));
            //var found = false;
            //for (long k = tMin; k<= tMax; k++) //for (long k = 0; k <= 100000; k++) 
            //{
            //    for (long j=tMin; j<tMax; j++) //for (long j = 0; j <= 100000; j++)
            //    {
            //        if (k == j) continue;


            //        vx = (test1.p.X -test2.p.X + k*test1.v.X - j*test2.v.X)/(k - j);
            //        if (vx > 10000 || vx < 10000) continue;
            //        px = test1.p.X + k*(test1.v.X - vx);

            //        vy = (test1.p.Y - test2.p.Y + k * test1.v.Y - j * test2.v.Y) / (k - j);
            //        if (vy > 10000 || vy < 10000) continue;
            //        py = test1.p.Y + k * (test1.v.Y - vy);

            //        vz = (test1.p.Z - test2.p.Z + k * test1.v.Z - j * test2.v.Z) / (k - j);
            //        if (vz > 10000 || vz < 10000) continue;
            //        pz = test1.p.Z + k * (test1.v.Z  - vz);

            //        rock = new Day24_Hailstone((px, py, pz), (vx, vy, vz));
            //        if (input.All(f => Day24_IsHailstoneCollide(f, rock)))
            //        {
            //            found = true;
            //            break;
            //        }
            //    }
            //    if (found) break;
            //}
            ////return (px_avr + py_avr + pz_avr);
            //return (rock.p.X +rock.p.Y + rock.p.Z) ;
            ////868366741881308 too high
            ///

            // solve with Wolfram Mathematica by giving the following inputs:
            //p1x = 181274863478376
            //p1y = 423998359962919
            //p1z = 286432452709141
            //p2x = 226461907371205
            //p2y = 306634733438686
            //p2z = 305056780555025
            //p3x = 347320263466693
            //p3y = 360139618479358
            //p3z = 271232232403985
            //v1x = -104
            //v1y = -373
            //v1z = -52
            //v2x = 54
            //v2y = 35
            //v2z = -49
            //v3x = -63
            //v3y = -122
            //v3z = 26

            //Solve[{
            //                p0x + t1 * v0x == p1x + t1 * v1x,
            //p0x + t2 * v0x == p2x + t2 * v2x, 
            //p0x + t3 * v0x == p3x + t3 * v3x,
            //p0y + t1 * v0y == p1y + t1 * v1y,
            //p0y + t2 * v0y == p2y + t2 * v2y,
            //p0y + t3 * v0y == p3y + t3 * v3y, 
            //p0z + t1 * v0z == p1z + t1 * v1z,
            //p0z + t2 * v0z == p2z + t2 * v2z, 
            //p0z + t3 * v0z == p3z + t3 * v3z,
            //s == p0x + p0y + p0z}, { p0x, p0y, p0z, v0x, v0y, v0z, t1, t2, t3, s}]
            return 80810774140675;

        }


    }
    public class Day24_Test
    {
        [Theory]
        [InlineData("19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3", 7, 27, 2)]
        public static void Day24Part1Test(string rawinput, long LimitLow, long LimitHigh, long expectedValue)
        {
            Assert.Equal(expectedValue, Day24.Day24_Part1(Day24.Day24_ReadInput(rawinput), LimitLow, LimitHigh));
        }

        [Theory]
        [InlineData("19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3", 47)]
        public static void Day24Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day24.Day24_Part2(Day24.Day24_ReadInput(rawinput)));
        }
    }
}
