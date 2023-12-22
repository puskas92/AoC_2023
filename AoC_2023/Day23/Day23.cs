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
    public static class Day23
    {
        public class Day23_Input : List<string> //Define input type
        {
        }
        public static void Day23_Main()
        {
            var input = Day23_ReadInput();
            Console.WriteLine($"Day23 Part1: {Day23_Part1(input)}");
            Console.WriteLine($"Day23 Part2: {Day23_Part2(input)}");
        }

        public static Day23_Input Day23_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day23\\Day23_input.txt").ReadToEnd();
            }

            var result = new Day23_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day23_Part1(Day23_Input input)
        {

            return 0;
        }

        public static int Day23_Part2(Day23_Input input)
        {
            return 0;
        }


    }
    public class Day23_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day23Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day23.Day23_Part1(Day23.Day23_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day23Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day23.Day23_Part2(Day23.Day23_ReadInput(rawinput)));
        }
    }
}
