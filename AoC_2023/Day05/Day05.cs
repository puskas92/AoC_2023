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
    public static class Day05
    {
        public class Day05_Input : List<string> //Define input type
        {
        }
        public static void Day05_Main()
        {
            var input = Day05_ReadInput();
            Console.WriteLine($"Day05 Part1: {Day05_Part1(input)}");
            Console.WriteLine($"Day05 Part2: {Day05_Part2(input)}");
        }

        public static Day05_Input Day05_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day05\\Day05_input.txt").ReadToEnd();
            }

            var result = new Day05_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day05_Part1(Day05_Input input)
        {

            return 0;
        }

        public static int Day05_Part2(Day05_Input input)
        {
            return 0;
        }


    }
    public class Day05_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day05Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part1(Day05.Day05_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day05Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part2(Day05.Day05_ReadInput(rawinput)));
        }
    }
}
