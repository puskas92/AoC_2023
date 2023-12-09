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
    public static class Day09
    {
        public class Day09_Input : List<Day09_HistoryLine> //Define input type
        {
        }
        public class Day09_HistoryLine
        {
            public int ForwardHistory = 0;
            public int BackwardHistory = 0;

            public Day09_HistoryLine(List<int> value)
            {
                var i = 0;
                var length = value.Count();
                List<int> firstPosHistory = new List<int>();
                do
                {
                    var j = 0;
                    firstPosHistory.Add(value.First());
                    for (j = 0; j < length - 1 - i; j++)
                    {
                        value[j] = value[j + 1] - value[j];
                    }
                    i++;
                } while ((i <= length) && !value.SkipLast(i).All(f => f == 0));

                if (value.SkipLast(i).All(f => f == 0))
                { 
                    for (var j = i; j >= 1; j--)
                    {
                        this.ForwardHistory += value[length - j];
                        this.BackwardHistory = firstPosHistory[j - 1] - this.BackwardHistory;
                    }
                }
            }
        }

        public static void Day09_Main()
        {
            var input = Day09_ReadInput();
            Console.WriteLine($"Day09 Part1: {Day09_Part1(input)}");
            Console.WriteLine($"Day09 Part2: {Day09_Part2(input)}");
        }

        public static Day09_Input Day09_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day09\\Day09_input.txt").ReadToEnd();
            }

            var result = new Day09_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day09_HistoryLine(line.Trim().Split(' ').Where(f=> f!="").Select(f=>int.Parse(f)).ToList()));
            }

            return result;
        }


        public static int Day09_Part1(Day09_Input input)
        {
            return input.Sum(f => f.ForwardHistory);
        }

        public static int Day09_Part2(Day09_Input input)
        {
            return input.Sum(f => f.BackwardHistory);
        }

 
    }
    public class Day09_Test
    {
        [Theory]
        [InlineData("0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45", 114)]
        public static void Day09Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part1(Day09.Day09_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45", 2)]
        public static void Day09Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part2(Day09.Day09_ReadInput(rawinput)));
        }
    }
}
