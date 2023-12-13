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
    public static class Day13
    {
        public class Day13_Input : List<Day13_Map> //Define input type
        {
        }
        public class Day13_Map
        {
            public Dictionary<int, Dictionary<int, bool>> map;

            public int SymmetryScore(int errorLimit)
            {

                for (var i = 0; i <= map.Keys.Max() - 1; i++)
                {
                    var DisMatchNumber = 0;
                    for (var j = 0; j <= map.Keys.Max(); j++)
                    {
                        if (!map.Keys.Contains(i + j + 1) || !map.Keys.Contains(i - j))
                        {
                            if (DisMatchNumber == errorLimit) return 100 * (i + 1);
                            else break;
                        }
                        for (var k = 0; k <= map[i].Keys.Max(); k++)
                        {
                            if (map[i - j][k] != map[i + j + 1][k])
                            {
                                DisMatchNumber +=1;
                                if(DisMatchNumber > errorLimit) break;
                            }
                        }
                        if (DisMatchNumber > errorLimit) break;
                    }
                }

                for (var i = 0; i <= map[0].Keys.Max() - 1; i++)
                {
                    var DisMatchNumber = 0;
                    for (var j = 0; j <= map[0].Keys.Max(); j++)
                    {
                        if (!map[0].Keys.Contains(i + j + 1) || !map[0].Keys.Contains(i - j))
                        {
                            if (DisMatchNumber == errorLimit) return i + 1;
                            else break;
                        }
                        for (var k = 0; k <= map.Keys.Max(); k++)
                        {
                            if (map[k][i - j] != map[k][i + j + 1])
                            {
                                DisMatchNumber += 1;
                                if (DisMatchNumber > errorLimit) break;
                            }
                        }
                        if (DisMatchNumber > errorLimit) break;
                    }
                }

                return 0;
            }
            }
        public static void Day13_Main()
        {
            var input = Day13_ReadInput();
            Console.WriteLine($"Day13 Part1: {Day13_Part1(input)}");
            Console.WriteLine($"Day13 Part2: {Day13_Part2(input)}");
        }

        public static Day13_Input Day13_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day13\\Day13_input.txt").ReadToEnd();
            }

            var result = new Day13_Input();

            result.Add(new Day13_Map());
            result[0].map = new Dictionary<int, Dictionary<int, bool>>();
            int i = 0;
            int row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                if(line == "")
                {
                    result.Add(new Day13_Map());
                    i++;
                    result[i].map = new Dictionary<int, Dictionary<int, bool>>();
                    row = 0;
                }
                else
                {
                    result[i].map.Add(row, new Dictionary<int, bool>());
                    var column = 0;
                    foreach(var character in line)
                    {
                        result[i].map[row].Add(column, (character=='#'));
                        column++;
                    }
                    row++;
                }
            }

            return result;
        }


        public static int Day13_Part1(Day13_Input input)
        {

            return input.Sum(f => f.SymmetryScore(0));
        }

        public static int Day13_Part2(Day13_Input input)
        {
            return input.Sum(f => f.SymmetryScore(1));
        }


    }
    public class Day13_Test
    {
        [Theory]
        [InlineData("#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#", 405)]
        public static void Day13Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day13.Day13_Part1(Day13.Day13_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#", 400)]
        public static void Day13Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day13.Day13_Part2(Day13.Day13_ReadInput(rawinput)));
        }
    }
}
