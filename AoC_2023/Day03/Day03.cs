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
    public static class Day03
    {
        public class Day03_Input : List<string> //Define input type
        {
        }
        public static void Day03_Main()
        {
            var input = Day03_ReadInput();
            Console.WriteLine($"Day03 Part1: {Day03_Part1(input)}");
            Console.WriteLine($"Day03 Part2: {Day03_Part2(input)}");
        }

        public static Day03_Input Day03_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day03\\Day03_input.txt").ReadToEnd();
            }

            var result = new Day03_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day03_Part1(Day03_Input input)
        {

            return 0;
        }

        public static int Day03_Part2(Day03_Input input)
        {
            return 0;
        }


    }
    public class Day03_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day03Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part1(Day03.Day03_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day03Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part2(Day03.Day03_ReadInput(rawinput)));
        }
    }
}
