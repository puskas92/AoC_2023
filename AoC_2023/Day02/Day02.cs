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
    public static class Day02
    {
        public class Day02_Input : List<string> //Define input type
        {
        }
        public static void Day02_Main()
        {
            var input = Day02_ReadInput();
            Console.WriteLine($"Day02 Part1: {Day02_Part1(input)}");
            Console.WriteLine($"Day02 Part2: {Day02_Part2(input)}");
        }

        public static Day02_Input Day02_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day02\\Day02_input.txt").ReadToEnd();
            }

            var result = new Day02_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day02_Part1(Day02_Input input)
        {

            return 0;
        }

        public static int Day02_Part2(Day02_Input input)
        {
            return 0;
        }


    }
    public class Day02_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day02Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part1(Day02.Day02_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day02Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part2(Day02.Day02_ReadInput(rawinput)));
        }
    }
}
