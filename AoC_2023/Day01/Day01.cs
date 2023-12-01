using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day01
    {
        public class Day01_Input : List<string> //Define input type
        {
        }
        public static void Day01_Main()
        {
            var input = Day01_ReadInput();
            Console.WriteLine($"Day01 Part1: {Day01_Part1(input)}");
            Console.WriteLine($"Day01 Part2: {Day01_Part2(input)}");
        }

        public static Day01_Input Day01_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day01\\Day01_input.txt").ReadToEnd();
            }

            var result = new Day01_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
               result.Add(line);
            }

            return result;
        }


        public static int Day01_Part1(Day01_Input input)
        {
            char[] nums = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0' ];
            return input.Sum(f =>
                       int.Parse(string.Concat(f[f.IndexOfAny(nums)], f[f.LastIndexOfAny(nums)])));
        }

        public static int Day01_Part2(Day01_Input input)
        {
            var numbers = new Dictionary<string, char>
            {
                ["one"] = '1',
                ["two"] = '2',
                ["three"] = '3',
                ["four"] = '4',
                ["five"] = '5',
                ["six"] = '6',
                ["seven"] = '7',
                ["eight"] = '8',
                ["nine"] = '9'
            };

            return input.Select(f =>
            {
                foreach (var num in numbers)
                {
                    f = f.Replace(num.Key, num.Key + num.Value + num.Key);
                }
                return f;
            }).Sum(f =>
                     int.Parse(string.Concat(f[f.IndexOfAny(numbers.Values.ToArray())], f[f.LastIndexOfAny(numbers.Values.ToArray())])));
        }
    }
    public class Day01_Test
    {
        [Theory]
        [InlineData("1abc2\r\npqr3stu8vwx\r\na1b2c3d4e5f\r\ntreb7uchet", 142)]
        public static void Day01Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day01.Day01_Part1(Day01.Day01_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("two1nine\r\neightwothree\r\nabcone2threexyz\r\nxtwone3four\r\n4nineeightseven2\r\nzoneight234\r\n7pqrstsixteen", 281)]
        public static void Day01Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day01.Day01_Part2(Day01.Day01_ReadInput(rawinput)));
        }
    }
}
