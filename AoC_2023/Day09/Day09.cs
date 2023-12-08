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
        public class Day09_Input : List<string> //Define input type
        {
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
                //result.Add(line);
            }

            return result;
        }


        public static int Day09_Part1(Day09_Input input)
        {

            return 0;
        }

        public static int Day09_Part2(Day09_Input input)
        {
            return 0;
        }


    }
    public class Day09_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day09Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part1(Day09.Day09_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day09Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day09.Day09_Part2(Day09.Day09_ReadInput(rawinput)));
        }
    }
}
