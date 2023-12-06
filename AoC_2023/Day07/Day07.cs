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
    public static class Day07
    {
        public class Day07_Input : List<string> //Define input type
        {
        }
        public static void Day07_Main()
        {
            var input = Day07_ReadInput();
            Console.WriteLine($"Day07 Part1: {Day07_Part1(input)}");
            Console.WriteLine($"Day07 Part2: {Day07_Part2(input)}");
        }

        public static Day07_Input Day07_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day07\\Day07_input.txt").ReadToEnd();
            }

            var result = new Day07_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day07_Part1(Day07_Input input)
        {

            return 0;
        }

        public static int Day07_Part2(Day07_Input input)
        {
            return 0;
        }


    }
    public class Day07_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day07Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part1(Day07.Day07_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day07Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part2(Day07.Day07_ReadInput(rawinput)));
        }
    }
}
