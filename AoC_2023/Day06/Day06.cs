using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day06
    {
        public class Day06_Input : List<(long time, long distance)> //Define input type
        {
        }
        public static void Day06_Main()
        {
            var input = Day06_ReadInput();
            Console.WriteLine($"Day06 Part1: {Day06_Part1(input)}");
            Console.WriteLine($"Day06 Part2: {Day06_Part2(input)}");
        }

        public static Day06_Input Day06_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day06\\Day06_input.txt").ReadToEnd();
            }

            var result = new Day06_Input();

            var lines = rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToList();
            var times = lines[0].Split(':')[1].Trim().Split(' ').Where(f=>f!="").Select(f=>long.Parse(f)).ToList();
            var distances = lines[1].Split(':')[1].Trim().Split(' ').Where(f => f != "").Select(f => long.Parse(f)).ToList();

            for(var i=0;i<times.Count; i++)
            {
                result.Add(new(times[i], distances[i]));
            }

            return result;
        }


        public static long Day06_Part1(Day06_Input input)
        {
            //d = x*(t-x) => x^2-tx+d = 0
            long result = 1;

            foreach(var race in input)
            {
                long a = 1;
                long b = -1 * race.time;
                long c = race.distance;
                var D = Math.Pow(b, 2) - 4 * a * c;
                if (D < 0)
                { //no result
                    result *= 0;
                    Debug.Print("no solution for one race");
                    continue;
                }

                var x1 = (-b - Math.Sqrt(D)) / (2 * a);
                var x2 = (-b + Math.Sqrt(D)) / (2 * a);

                Debug.Assert(x1 <= x2);

                var subresult = (long)Math.Floor(x2) - (long)Math.Ceiling(x1) +1;
                if ((long)Math.Floor(x2) == x2) subresult--;
                if ((long)Math.Ceiling(x1) == x1) subresult--;
                result *= subresult;
            }

            return result;
        }

        public static long Day06_Part2(Day06_Input input)
        {
            var timestring = "";
            var distancestring = "";
            foreach(var race in input)
            {
                timestring += race.time.ToString();
                distancestring += race.distance.ToString();
            }
            var newInput = new Day06_Input();
            newInput.Add(new(long.Parse(timestring), long.Parse(distancestring)));
            return Day06_Part1(newInput);
        }


    }
    public class Day06_Test
    {
        [Theory]
        [InlineData("Time:      7  15   30\r\nDistance:  9  40  200", 288)]
        public static void Day06Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part1(Day06.Day06_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("Time:      7  15   30\r\nDistance:  9  40  200", 71503)]
        public static void Day06Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part2(Day06.Day06_ReadInput(rawinput)));
        }
    }
}
