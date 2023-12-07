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
    public static class Day08
    {
        public class Day08_Input : List<string> //Define input type
        {
        }
        public static void Day08_Main()
        {
            var input = Day08_ReadInput();
            Console.WriteLine($"Day08 Part1: {Day08_Part1(input)}");
            Console.WriteLine($"Day08 Part2: {Day08_Part2(input)}");
        }

        public static Day08_Input Day08_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day08\\Day08_input.txt").ReadToEnd();
            }

            var result = new Day08_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day08_Part1(Day08_Input input)
        {

            return 0;
        }

        public static int Day08_Part2(Day08_Input input)
        {
            return 0;
        }


    }
    public class Day08_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day08Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part1(Day08.Day08_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day08Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part2(Day08.Day08_ReadInput(rawinput)));
        }
    }
}
