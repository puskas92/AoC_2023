using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day12
    {
        public class Day12_Input : List<Day12_SpringGroup> //Define input type
        {
        }
        public class Day12_SpringGroup
        {
            public string SpringPackage;
            public List<int> GroupOfDamagedSprings;

            public Day12_SpringGroup(string raw)
            {
                var split = raw.Split(' ');
                 SpringPackage = split[0].Trim();
                 GroupOfDamagedSprings = split[1].Trim().Split(',').Select(f => int.Parse(f)).ToList();

            }

            public long CalculatePossibleArrengementWithSelfPart1()
            {
                return CalculatePossibleArrengement2(SpringPackage, GroupOfDamagedSprings); ;
            }

            public long CalculatePossibleArrengementWithSelfPart2()
            {
                string SpringPackageToTest = SpringPackage;
                var GroupOfDamagedSpringsToTest = new List<int>();
                GroupOfDamagedSpringsToTest.AddRange(GroupOfDamagedSprings);
                for (var i = 2; i<=5; i++)
                {
                    SpringPackageToTest += "?" + SpringPackage;
                    GroupOfDamagedSpringsToTest.AddRange(GroupOfDamagedSprings);
                }
                return CalculatePossibleArrengement2(SpringPackageToTest, GroupOfDamagedSpringsToTest);
            }

            private static Dictionary<(string, string), long> Cache2 = new Dictionary<(string, string), long>();
            public static long CalculatePossibleArrengement2(string SpringPackage, List<int> GroupOfDamagedSprings)
            {
                SpringPackage = SpringPackage.Trim('.');
                if (SpringPackage.All(f => f == '?')) return CalculatePossibleArrengementAllQuestion(SpringPackage.Length, GroupOfDamagedSprings);

                (string, string) cacheKey = (SpringPackage, string.Join(',', GroupOfDamagedSprings));
                if(Cache2.ContainsKey(cacheKey)) return Cache2[cacheKey];

                var firstGroup = GroupOfDamagedSprings.First();
                var remainingGroupOfSprings = GroupOfDamagedSprings.Skip(1).ToList();

                var remainingLength = (remainingGroupOfSprings.Count >0) ? (remainingGroupOfSprings.Sum() + remainingGroupOfSprings.Count - 1) : -1;

                long result = 0;
                for(var i = 0; i< SpringPackage.Length - firstGroup - remainingLength; i++)
                {
                    if (i > 0 && SpringPackage.Take(i - 1).Contains('#')) break;
                    var pattern = @"^(\.|\?)(#|\?){" + firstGroup.ToString() + @"}(\.|\?)$";
                    var stringToSearch = "";
                    if (i == 0)
                    {
                        stringToSearch = '.' + String.Concat(SpringPackage.Take(firstGroup + 1));
                    }
                    else
                    {
                        stringToSearch =  String.Concat(SpringPackage.Skip(i - 1).Take(firstGroup + 2));
                    }
                    if (stringToSearch.Length < firstGroup + 2) stringToSearch += '.';
                    if (Regex.Match(stringToSearch, pattern).Success)
                    {
                        if (remainingGroupOfSprings.Count == 0)
                        {
                            if (SpringPackage.Skip(i + firstGroup + 1).Contains('#')) result += 0;         
                           else result += 1;
                        }
                        else
                        {
                            var subSpringPackage = String.Concat(SpringPackage.Skip(i + firstGroup + 1));
                            result += CalculatePossibleArrengement2(subSpringPackage, remainingGroupOfSprings);
                        }
                    }
                }

                if (!Cache2.ContainsKey(cacheKey)) Cache2.Add(cacheKey, result);
                return result;
            }

            public static long CalculatePossibleArrengementAllQuestion(int numberOfQuestionMarks, List<int> GroupOfDamagedSprings)
            {
                var count = GroupOfDamagedSprings.Count();
                var shortlength = numberOfQuestionMarks - GroupOfDamagedSprings.Sum() + count;
                return CalculatePossibleArrengementAllQuestionSimplified(shortlength, count);

            }

            public static Dictionary<(int, int), long> AllQuestionMarkCache = new Dictionary<(int, int), long>();

            public static long CalculatePossibleArrengementAllQuestionSimplified(int numberOfQuestionMarks, int numberOfSpring)
            {

                if (numberOfQuestionMarks <= 0 || numberOfSpring <= 0) return 0;
                if (numberOfSpring == 1) return numberOfQuestionMarks;
                var check = numberOfQuestionMarks - ((numberOfSpring * 2) - 1);
                if (check < 0) return 0;
                if (check == 0) return 1;
                (int, int) cacheKey = new(numberOfQuestionMarks, numberOfSpring);
                if (AllQuestionMarkCache.ContainsKey(cacheKey)) return AllQuestionMarkCache[cacheKey];
                long result = 0;

                for(var i= 0; i< numberOfQuestionMarks; i++)
                {
                    result += CalculatePossibleArrengementAllQuestionSimplified(numberOfQuestionMarks - 2 - i, numberOfSpring - 1);
                }

                AllQuestionMarkCache.Add(cacheKey, result);
                return result;
                
            }
        }

    
    public static void Day12_Main()
        {
            var input = Day12_ReadInput();
            Console.WriteLine($"Day12 Part1: {Day12_Part1(input)}");
            Console.WriteLine($"Day12 Part2: {Day12_Part2(input)}");
        }

        public static Day12_Input Day12_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day12\\Day12_input.txt").ReadToEnd();
            }

            var result = new Day12_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day12_SpringGroup(line));
            }

            return result;
        }


        public static long Day12_Part1(Day12_Input input)
        {

            return input.Sum(f=>f.CalculatePossibleArrengementWithSelfPart1());
        }

        public static long Day12_Part2(Day12_Input input)
        {
           return input.Sum(f => f.CalculatePossibleArrengementWithSelfPart2());
        }


    }
    public class Day12_Test
    {
        [Theory]
        [InlineData("????????? 1,1,3\r\n...?????????..... 1,1,3\r\n??????? 1,1,1\r\n?????????? 2,1,3\r\n???? 1,1\r\n???????????? 1,1,1\r\n?????????????? 1,1,1\r\n?????????????? 1,1,1,1\r\n?????????????? 1,1,1,1,1", 965)]
        [InlineData("???.### 1,1,3\r\n.??..??...?##. 1,1,3\r\n?#?#?#?#?#?#?#? 1,3,1,6\r\n????.#...#... 4,1,1\r\n????.######..#####. 1,6,5\r\n?###???????? 3,2,1", 21)]
        [InlineData(".###?..#??????#???? 4,8,1\r\n???.????????#? 1,3,1,1\r\n.#?.??#?????##.# 1,2,1,3,1\r\n?.#????#?????#??#?? 1,6,1,4,1", 30)]
        [InlineData(".###?..#??????#???? 4,8,1", 3)]
        [InlineData("???.????????#? 1,3,1,1", 19)]
        [InlineData(".#?.??#?????##.# 1,2,1,3,1", 3)]
        [InlineData("?.#????#?????#??#?? 1,6,1,4,1", 5)]
        public static void Day12Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day12.Day12_Part1(Day12.Day12_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("???.### 1,1,3\r\n.??..??...?##. 1,1,3\r\n?#?#?#?#?#?#?#? 1,3,1,6\r\n????.#...#... 4,1,1\r\n????.######..#####. 1,6,5\r\n?###???????? 3,2,1", 525152)]
        public static void Day12Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day12.Day12_Part2(Day12.Day12_ReadInput(rawinput)));
        }
    }
}
