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
                var pattern = CalculatePattern(GroupOfDamagedSprings);
                return CalculatePossibleArrengement( SpringPackage, GroupOfDamagedSprings, pattern);
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
                var pattern = CalculatePattern(GroupOfDamagedSpringsToTest);

                //foreach(var group in GroupOfDamagedSprings)
                //{
                //    var subPattern = @"(\.|\?)*";
                //    subPattern += @"(#|\?){" + group.ToString() + @"}(\.|\?)*";
                //    //subPattern += @")";
                //    var matches = Regex.Matches(SpringPackageToTest, subPattern);
                //    if(matches.Count == 5)
                //    {
                //        Console.WriteLine("Heureka");
                //    }
                //}
                return CalculatePossibleArrengement(SpringPackageToTest, GroupOfDamagedSpringsToTest, pattern);
            }

            public static long CalculatePossibleArrengement(string SpringPackage, List<int> GroupOfDamagedSprings, string pattern)
            {
                if (SpringPackage.Trim('.').All(f => f == '?')) return CalculatePossibleArrengementAllQuestion(SpringPackage.Trim('.').Length, GroupOfDamagedSprings);
                if (!IsMatch(SpringPackage, pattern)) return 0;
                else
                { 
                    var placeOfQuestionMark = SpringPackage.IndexOf('?');
                    if (placeOfQuestionMark == -1) return 1;
                    else
                    {
                        StringBuilder sb = new StringBuilder(SpringPackage);
                        sb[placeOfQuestionMark] = '.';
                        long result = CalculatePossibleArrengement(sb.ToString(), GroupOfDamagedSprings, pattern);

                        sb[placeOfQuestionMark] = '#';
                        result += CalculatePossibleArrengement(sb.ToString(), GroupOfDamagedSprings, pattern);

                        return result;
                    }
                }
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

                //this is almost working
                //double result = 1;
                //for (int i = 0; i < numberOfSpring; i++)
                //{
                //    result *= (double)(numberOfQuestionMarks - (numberOfSpring - 1) - i) / (numberOfSpring - i);
                //}
                //return (int)result;

                for(var i= 0; i< numberOfQuestionMarks; i++)
                {
                    result += CalculatePossibleArrengementAllQuestionSimplified(numberOfQuestionMarks - 2 - i, numberOfSpring - 1);
                }

                AllQuestionMarkCache.Add(cacheKey, result);
                return result;
                
            }

            public static bool IsMatch(string SpringPackage, string pattern)
            {
                //very slow for some inputs in Part2
                return Regex.Match(SpringPackage, pattern).Success;
            }

            public static string CalculatePattern(List<int> GroupOfDamagedSprings)
            {
                var pattern = @"^(\.|\?)*";
                foreach (var group in GroupOfDamagedSprings)
                {
                    pattern += @"(#|\?){" + group.ToString() + @"}(\.|\?)+";
                }
                pattern = String.Concat(pattern.SkipLast(1));
                pattern += @"*$";
                return pattern;
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
            //return input.Sum(f => f.CalculatePossibleArrengementWithSelfPart2());
            long result = 0;
            for(var i= 0; i < input.Count; i++)
            {
                Console.WriteLine(i);
                result += input[i].CalculatePossibleArrengementWithSelfPart2();
            }
            return result;
        }


    }
    public class Day12_Test
    {
        [Theory]
        [InlineData("????????? 1,1,3\r\n...?????????..... 1,1,3\r\n??????? 1,1,1\r\n?????????? 2,1,3\r\n???? 1,1\r\n???????????? 1,1,1\r\n?????????????? 1,1,1\r\n?????????????? 1,1,1,1\r\n?????????????? 1,1,1,1,1", 965)]
        [InlineData("???.### 1,1,3\r\n.??..??...?##. 1,1,3\r\n?#?#?#?#?#?#?#? 1,3,1,6\r\n????.#...#... 4,1,1\r\n????.######..#####. 1,6,5\r\n?###???????? 3,2,1", 21)]
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
