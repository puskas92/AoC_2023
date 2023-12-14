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
    public static class Day15
    {
        public class Day15_Input : List<string> //Define input type
        {
        }
        public static void Day15_Main()
        {
            var input = Day15_ReadInput();
            Console.WriteLine($"Day15 Part1: {Day15_Part1(input)}");
            Console.WriteLine($"Day15 Part2: {Day15_Part2(input)}");
        }

        public static Day15_Input Day15_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day15\\Day15_input.txt").ReadToEnd();
            }

            var result = new Day15_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day15_Part1(Day15_Input input)
        {

            return 0;
        }

        public static int Day15_Part2(Day15_Input input)
        {
            return 0;
        }


    }
    public class Day15_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day15Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day15.Day15_Part1(Day15.Day15_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day15Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day15.Day15_Part2(Day15.Day15_ReadInput(rawinput)));
        }
    }
}
