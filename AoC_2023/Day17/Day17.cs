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
    public static class Day17
    {
        public class Day17_Input : List<string> //Define input type
        {
        }
        public static void Day17_Main()
        {
            var input = Day17_ReadInput();
            Console.WriteLine($"Day17 Part1: {Day17_Part1(input)}");
            Console.WriteLine($"Day17 Part2: {Day17_Part2(input)}");
        }

        public static Day17_Input Day17_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day17\\Day17_input.txt").ReadToEnd();
            }

            var result = new Day17_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day17_Part1(Day17_Input input)
        {

            return 0;
        }

        public static int Day17_Part2(Day17_Input input)
        {
            return 0;
        }


    }
    public class Day17_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day17Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day17.Day17_Part1(Day17.Day17_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day17Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day17.Day17_Part2(Day17.Day17_ReadInput(rawinput)));
        }
    }
}
