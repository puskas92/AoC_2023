using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day02
    {
        public class Day02_Input : Dictionary<int, List<Day02_SetOfCubes>> //Define input type
        {
        }
        public record struct Day02_SetOfCubes(int id = 0, int red = 0, int green = 0, int blue = 0)
        {
            public Day02_SetOfCubes(string input) : this(0, 0,0,0)
            {
                var splitted = input.Split(",").Select(x => x.Trim());
                foreach(var hand in splitted)
                {
                    switch(hand.Last())
                    {
                        case 'd': //red
                            this.red = int.Parse(Regex.Match(hand, @"\d+").Value);
                            break;
                        case 'n': //green
                            this.green = int.Parse(Regex.Match(hand, @"\d+").Value);
                            break;
                        case 'e': //blue
                            this.blue = int.Parse(Regex.Match(hand, @"\d+").Value);
                            break;
                        default:
                            Debug.Print("not implemented switch case");
                            break;
                    }
                }
            }

            public bool isPossibleHand()
            {
                return (this.red<= 12 && this.green<= 13 && this.blue<= 14);
            }
        }
        public static void Day02_Main()
        {
            var input = Day02_ReadInput();
            Console.WriteLine($"Day02 Part1: {Day02_Part1(input)}");
            Console.WriteLine($"Day02 Part2: {Day02_Part2(input)}");
        }

        public static Day02_Input Day02_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day02\\Day02_input.txt").ReadToEnd();
            }

            var result = new Day02_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var idsplit = line.Split(':');
                var id = int.Parse(Regex.Match(idsplit[0], @"\d+").Value);
                var setsplit = idsplit[1].Split(";");

                var listofSet = new List<Day02_SetOfCubes>();
                foreach(var set in setsplit)
                {
                    listofSet.Add(new Day02_SetOfCubes(set));
                }
                result.Add(id, listofSet);
            }

            return result;
        }


        public static int Day02_Part1(Day02_Input input)
        {

            return input.Sum(f=> f.Value.All(g => g.isPossibleHand()) ? f.Key : 0);
        }

        public static int Day02_Part2(Day02_Input input)
        {
            return input.Sum(f =>
            {
                var maxgreen = f.Value.Max(g => g.green);
                var maxred = f.Value.Max(g => g.red);
                var maxblue = f.Value.Max(g => g.blue);
                return maxgreen * maxred * maxblue;
            });
        }


    }
    public class Day02_Test
    {
        [Theory]
        [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 8)]
        public static void Day02Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part1(Day02.Day02_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 2286)]
        public static void Day02Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day02.Day02_Part2(Day02.Day02_ReadInput(rawinput)));
        }
    }
}
