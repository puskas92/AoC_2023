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
    public static class Day14
    {
        public class Day14_Input
        {
            public Dictionary<int, Dictionary<int, char>> map = new Dictionary<int, Dictionary<int, char>>();

            public void MoveToDir((int X, int Y) dir)
            {
                if(dir.X == -1 && dir.Y == 0)
                {
                    for(int i = 0; i<=map.Keys.Max(); i++)
                    {
                        for (int j = 0; j <= map[i].Keys.Max(); j++)
                        {
                            if (map[i][j] != 'O') continue;
                            int k = i-1;
                            while (map.ContainsKey(k) && map[k][j] == '.')
                            { k--; }
                            map[i][j] = '.';
                            map[k + 1][j] = 'O';
                        }
                    }
                }
            }

            public int NorthSupportScore()
            {
                var score = 0;
                for (int i = 0; i <= map.Keys.Max(); i++)
                {
                    for (int j = 0; j <= map[i].Keys.Max(); j++)
                    {
                        if (map[i][j] == 'O')
                        {
                            score += (map.Keys.Max() + 1 - i);
                        } 
                    }
                }
                return score;
            }
        }
        public static void Day14_Main()
        {
            var input = Day14_ReadInput();
            Console.WriteLine($"Day14 Part1: {Day14_Part1(input)}");
            Console.WriteLine($"Day14 Part2: {Day14_Part2(input)}");
        }

        public static Day14_Input Day14_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day14\\Day14_input.txt").ReadToEnd();
            }

            var result = new Day14_Input();

            var row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.map.Add(row, new Dictionary<int, char>());
                var column = 0;
                foreach(var character in line)
                {
                    result.map[row].Add(column, character);
                    column++;
                }

                row++;
            }

            return result;
        }


        public static int Day14_Part1(Day14_Input input)
        {
            input.MoveToDir(new(-1, 0));
            return input.NorthSupportScore();
        }

        public static int Day14_Part2(Day14_Input input)
        {
            return 0;
        }


    }
    public class Day14_Test
    {
        [Theory]
        [InlineData("O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....", 136)]
        public static void Day14Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day14.Day14_Part1(Day14.Day14_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....", 64)]
        public static void Day14Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day14.Day14_Part2(Day14.Day14_ReadInput(rawinput)));
        }
    }
}
