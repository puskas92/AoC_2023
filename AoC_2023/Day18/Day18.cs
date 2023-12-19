using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day18
    {
        public class Day18_Input : List<Day18_Command> //Define input type
        {
        }
        public class Day18_Command
        {
            public (int X, int Y) Part1Direction;
            public int Part1Length;
            public (int X, int Y) Part2Direction;
            public int Part2Length;

            public Day18_Command(string raw)
            {
                var splitted = raw.Split(' ');
                var dirChar = splitted[0];
                Part1Direction = dirChar.First() switch
                {
                    'U' => (-1, 0),
                    'D' => (1, 0),
                    'L' => (0, -1),
                    'R' => (0, 1),
                    _ => (0,0)
                };
                    
                    
                Part1Length = int.Parse(splitted[1]);

                Part2Length = Convert.ToInt32(String.Concat(splitted[2].Trim('(').Trim(')').Trim('#').SkipLast(1)), 16);
                Part2Direction = splitted[2].Trim('(').Trim(')').Trim('#').Last() switch
                {
                    '3' => (-1, 0),
                    '1' => (1, 0),
                    '2' => (0, -1),
                    '0' => (0, 1),
                    _ => (0, 0)
                };
            }
        }
        public static void Day18_Main()
        {
            var input = Day18_ReadInput();
            Console.WriteLine($"Day18 Part1: {Day18_Part1(input)}");
            Console.WriteLine($"Day18 Part2: {Day18_Part2(input)}");
        }

        public static Day18_Input Day18_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day18\\Day18_input.txt").ReadToEnd();
            }

            var result = new Day18_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day18_Command(line));
            }

            return result;
        }


        public static long Day18_Part1(Day18_Input input)
        {
            (long X, long Y) pos = new(0, 0);
            (long X, long Y) nextpos;

            long doublearea = 0;
            long numOfPoints = 0;
            foreach (var command in input)
            {
                nextpos = (pos.X + (command.Part1Direction.X * command.Part1Length), pos.Y + (command.Part1Direction.Y * command.Part1Length));
                doublearea += pos.X * nextpos.Y;
                doublearea -= nextpos.X * pos.Y;
                pos = nextpos;
                numOfPoints += (command.Part1Length);
            }
            Debug.Assert(doublearea % 2 == 0);
            return Math.Abs((long)(doublearea / 2)) + numOfPoints / 2 + 1;
        }

        public static long Day18_Part2(Day18_Input input)
        {
            (long X, long Y) pos = new(0, 0);
            (long X, long Y) nextpos;

            long  doublearea = 0;
            long numOfPoints = 0;
            foreach (var command in input)
            {
                nextpos = (pos.X + (command.Part2Direction.X * command.Part2Length), pos.Y + (command.Part2Direction.Y * command.Part2Length));
                doublearea += pos.X * nextpos.Y;
                doublearea -= nextpos.X * pos.Y;
                pos = nextpos;
                numOfPoints += (command.Part2Length) ;
            }
            Debug.Assert(doublearea % 2 == 0);
            return Math.Abs((long)(doublearea / 2)) + numOfPoints/2 + 1;
        }


    }
    public class Day18_Test
    {
        [Theory]
        [InlineData("R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", 62)]
        public static void Day18Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part1(Day18.Day18_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", 952408144115)]
        public static void Day18Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part2(Day18.Day18_ReadInput(rawinput)));
        }
    }
}
