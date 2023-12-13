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
        public class Day14_Input : List<string> //Define input type
        {
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

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day14_Part1(Day14_Input input)
        {

            return 0;
        }

        public static int Day14_Part2(Day14_Input input)
        {
            return 0;
        }


    }
    public class Day14_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day14Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day14.Day14_Part1(Day14.Day14_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day14Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day14.Day14_Part2(Day14.Day14_ReadInput(rawinput)));
        }
    }
}
