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
    public static class Day16
    {
        public class Day16_Input : List<string> //Define input type
        {
        }
        public static void Day16_Main()
        {
            var input = Day16_ReadInput();
            Console.WriteLine($"Day16 Part1: {Day16_Part1(input)}");
            Console.WriteLine($"Day16 Part2: {Day16_Part2(input)}");
        }

        public static Day16_Input Day16_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day16\\Day16_input.txt").ReadToEnd();
            }

            var result = new Day16_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day16_Part1(Day16_Input input)
        {

            return 0;
        }

        public static int Day16_Part2(Day16_Input input)
        {
            return 0;
        }


    }
    public class Day16_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day16Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day16.Day16_Part1(Day16.Day16_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day16Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day16.Day16_Part2(Day16.Day16_ReadInput(rawinput)));
        }
    }
}
