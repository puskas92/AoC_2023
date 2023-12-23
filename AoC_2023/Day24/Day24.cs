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
    public static class Day24
    {
        public class Day24_Input : List<string> //Define input type
        {
        }
        public static void Day24_Main()
        {
            var input = Day24_ReadInput();
            Console.WriteLine($"Day24 Part1: {Day24_Part1(input)}");
            Console.WriteLine($"Day24 Part2: {Day24_Part2(input)}");
        }

        public static Day24_Input Day24_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day24\\Day24_input.txt").ReadToEnd();
            }

            var result = new Day24_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day24_Part1(Day24_Input input)
        {

            return 0;
        }

        public static int Day24_Part2(Day24_Input input)
        {
            return 0;
        }


    }
    public class Day24_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day24Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day24.Day24_Part1(Day24.Day24_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day24Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day24.Day24_Part2(Day24.Day24_ReadInput(rawinput)));
        }
    }
}
