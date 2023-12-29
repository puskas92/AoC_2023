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
            long px_avr = 0, py_avr =0, pz_avr = 0;
            long vx_avr = 0, vy_avr = 0, vz_avr = 0;
            var n = input.Count();
            List<int> time = new List<int>();
            foreach (var hailstone in input)
            {
                px_avr += hailstone.p.X;
                py_avr += hailstone.p.Y;
                pz_avr += hailstone.p.Z;
                vx_avr += hailstone.v.X;
                vy_avr += hailstone.v.Y;
                vz_avr += hailstone.v.Z;
                time.Add(0);
            }
            px_avr = px_avr / n;
            py_avr = py_avr / n;
            pz_avr = pz_avr / n;
            vx_avr = vx_avr / n;
            vy_avr = vy_avr / n;
            vz_avr = vz_avr / n;

            var px = px_avr;
            var py = py_avr;
            var pz = pz_avr;
            var vx = vx_avr;
            var vy = vy_avr;
            var vz = vz_avr;
            var rock = new Day24_Hailstone((px, py, pz), (vx, vy, vz));

            while(!input.All(f=> Day24_IsHailstoneCollide(f, rock)))
            {
                double vx_delta = 0;
                double vy_delta = 0;
                double vz_delta = 0;
                for (var i = 0; i< input.Count; i++)
                {
                    var testHail = input[i];
                    double tx = -1 * (px - testHail.p.X) / (double)(vx - testHail.v.X);
                    double ty = -1 * (py - testHail.p.Y) / (double)(vy - testHail.v.Y);
                    double tz = -1 * (pz - testHail.p.Z) / (double)(vz - testHail.v.Z);
                    var c = 0;
                    double t = 0;
                    if (double.IsFinite(tx))
                    {
                        t += tx;
                        c++;
                    }
                    if (double.IsFinite(ty))
                    {
                        t += ty;
                        c++;
                    }
                    if (double.IsFinite(tz))
                    {
                        t += tz;
                        c++;
                    }
                    t = (c == 0) ? time[i] : ((t / c) + time[i] /2) ;
                    if (t <= 0) t = 1;
                    time[i] = (int)Math.Round(t);

                    vx = (int)Math.Round((((testHail.p.X - px) / t + testHail.v.X) + vx)/2);
                    vy = (int)Math.Round((((testHail.p.Y - py) / t + testHail.v.Y) + vy)/2);
                    vz = (int)Math.Round((((testHail.p.Z - pz) / t + testHail.v.Z) + vz)/2);

                    vx_delta += t * (testHail.v.X - vx);
                    vy_delta += t * (testHail.v.Y - vy);
                    vz_delta += t  * (testHail.v.Z - vz);
                }
                px = px_avr + (int)Math.Round(vx_delta / n);
                py = py_avr + (int)Math.Round(vy_delta / n);
                pz = pz_avr + (int)Math.Round(vz_delta / n);
            }


            return (px_avr + py_avr + pz_avr);
            //868366741881308 too high
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
