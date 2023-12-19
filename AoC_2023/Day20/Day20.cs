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
    public static class Day20
    {
        public class Day20_Input : List<string> //Define input type
        {
        }
        public static void Day20_Main()
        {
            var input = Day20_ReadInput();
            Console.WriteLine($"Day20 Part1: {Day20_Part1(input)}");
            Console.WriteLine($"Day20 Part2: {Day20_Part2(input)}");
        }

        public static Day20_Input Day20_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day20\\Day20_input.txt").ReadToEnd();
            }

            var result = new Day20_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day20_Part1(Day20_Input input)
        {

            return 0;
        }

        public static int Day20_Part2(Day20_Input input)
        {
            return 0;
        }


    }
    public class Day20_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day20Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day20.Day20_Part1(Day20.Day20_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day20Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day20.Day20_Part2(Day20.Day20_ReadInput(rawinput)));
        }
    }
}
