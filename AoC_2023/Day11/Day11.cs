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
    public static class Day11
    {
        public class Day11_Input : List<(int X,int Y)> //Define input type
        {
        }
        public static void Day11_Main()
        {
            var input = Day11_ReadInput();
            Console.WriteLine($"Day11 Part1: {Day11_Part1(input,2)}");
            Console.WriteLine($"Day11 Part2: {Day11_Part2(input, 1000000)}");
        }

        public static Day11_Input Day11_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day11\\Day11_input.txt").ReadToEnd();
            }

            var result = new Day11_Input();

            var i = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var j = -1;
                while (true)
                {
                    j = line.IndexOf('#', j+1);
                    if (j == -1) break;
                    result.Add(new(i, j));
                }
                i++;
            }

            return result;
        }


        public static long Day11_Part1(Day11_Input input, int scale)
        {
            long sum = 0;

            var rows = input.Select(f => f.X).Distinct().ToList();
            var columns = input.Select(f=> f.Y).Distinct().ToList();

            for(var i = 0; i< input.Count-1; i++)
            {
                for(var j = i+1; j<input.Count; j++)
                {
                    sum += Math.Abs(input[i].X - input[j].X) + Math.Abs(input[i].Y - input[j].Y);
                    for(var k = Math.Min(input[i].X, input[j].X) +1; k < Math.Max(input[i].X, input[j].X); k++)
                    {
                        if (!rows.Contains(k)) sum+=(scale-1);
                    }
                    for (var k = Math.Min(input[i].Y, input[j].Y) + 1; k < Math.Max(input[i].Y, input[j].Y); k++)
                    {
                        if (!columns.Contains(k)) sum+=(scale-1);
                    }
                }
            }

            return sum;
        }

        public static long Day11_Part2(Day11_Input input, int scale)
        {
            return Day11_Part1(input, scale);
        }


    }
    public class Day11_Test
    {
        [Theory]
        [InlineData("...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....",2, 374)]
        public static void Day11Part1Test(string rawinput, int scale, int expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part1(Day11.Day11_ReadInput(rawinput), scale));
        }

        [Theory]
        [InlineData("...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....",10, 1030)]
        [InlineData("...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....", 100, 8410)]
        public static void Day11Part2Test(string rawinput,int scale, int expectedValue)
        {
            Assert.Equal(expectedValue, Day11.Day11_Part2(Day11.Day11_ReadInput(rawinput), scale));
        }
    }
}
