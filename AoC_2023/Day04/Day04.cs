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
    public static class Day04
    {
        public class Day04_Input : List<string> //Define input type
        {
        }
        public static void Day04_Main()
        {
            var input = Day04_ReadInput();
            Console.WriteLine($"Day04 Part1: {Day04_Part1(input)}");
            Console.WriteLine($"Day04 Part2: {Day04_Part2(input)}");
        }

        public static Day04_Input Day04_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day04\\Day04_input.txt").ReadToEnd();
            }

            var result = new Day04_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day04_Part1(Day04_Input input)
        {

            return 0;
        }

        public static int Day04_Part2(Day04_Input input)
        {
            return 0;
        }


    }
    public class Day04_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day04Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part1(Day04.Day04_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day04Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part2(Day04.Day04_ReadInput(rawinput)));
        }
    }
}
