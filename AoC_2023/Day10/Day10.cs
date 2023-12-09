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
    public static class Day10
    {
        public class Day10_Input : List<string> //Define input type
        {
        }
        public static void Day10_Main()
        {
            var input = Day10_ReadInput();
            Console.WriteLine($"Day10 Part1: {Day10_Part1(input)}");
            Console.WriteLine($"Day10 Part2: {Day10_Part2(input)}");
        }

        public static Day10_Input Day10_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day10\\Day10_input.txt").ReadToEnd();
            }

            var result = new Day10_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day10_Part1(Day10_Input input)
        {

            return 0;
        }

        public static int Day10_Part2(Day10_Input input)
        {
            return 0;
        }


    }
    public class Day10_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day10Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part1(Day10.Day10_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day10Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part2(Day10.Day10_ReadInput(rawinput)));
        }
    }
}
