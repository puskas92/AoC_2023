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
    public static class Day21
    {
        public class Day21_Input : List<string> //Define input type
        {
        }
        public static void Day21_Main()
        {
            var input = Day21_ReadInput();
            Console.WriteLine($"Day21 Part1: {Day21_Part1(input)}");
            Console.WriteLine($"Day21 Part2: {Day21_Part2(input)}");
        }

        public static Day21_Input Day21_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day21\\Day21_input.txt").ReadToEnd();
            }

            var result = new Day21_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                //result.Add(line);
            }

            return result;
        }


        public static int Day21_Part1(Day21_Input input)
        {

            return 0;
        }

        public static int Day21_Part2(Day21_Input input)
        {
            return 0;
        }


    }
    public class Day21_Test
    {
        [Theory]
        [InlineData("ABC", 0)]
        public static void Day21Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day21.Day21_Part1(Day21.Day21_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("ABC", 0)]
        public static void Day21Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day21.Day21_Part2(Day21.Day21_ReadInput(rawinput)));
        }
    }
}
