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
    public static class Day18
    {
        public class Day18_Input : List<string> //Define input type
        {
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
                //result.Add(line);
            }

            return result;
        }


        public static int Day18_Part1(Day18_Input input)
        {

            return 0;
        }

        public static int Day18_Part2(Day18_Input input)
        {
            return 0;
        }


    }
    public class Day18_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day18Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part1(Day18.Day18_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day18Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part2(Day18.Day18_ReadInput(rawinput)));
        }
    }
}
