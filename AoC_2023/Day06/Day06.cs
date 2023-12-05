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
    public static class Day06
    {
        public class Day06_Input : List<string> //Define input type
        {
        }
        public static void Day06_Main()
        {
            var input = Day06_ReadInput();
            Console.WriteLine($"Day06 Part1: {Day06_Part1(input)}");
            Console.WriteLine($"Day06 Part2: {Day06_Part2(input)}");
        }

        public static Day06_Input Day06_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day06\\Day06_input.txt").ReadToEnd();
            }

            var result = new Day06_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day06_Part1(Day06_Input input)
        {

            return 0;
        }

        public static int Day06_Part2(Day06_Input input)
        {
            return 0;
        }


    }
    public class Day06_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day06Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part1(Day06.Day06_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day06Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day06.Day06_Part2(Day06.Day06_ReadInput(rawinput)));
        }
    }
}
