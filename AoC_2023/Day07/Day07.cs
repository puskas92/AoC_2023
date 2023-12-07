using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day07
    {
        public class Day07_Input : List<Day07_Hand> //Define input type
        {
        }
        public class Day07_Hand
        {
            public string cards;
            public int bid;
            
            public int HandTypeStrengthPart1;
            public int HandTypeStrengthPart2;
            public Day07_Hand(string line)
            {
                var split = line.Split(' ');
                cards = split[0];
                bid = int.Parse(split[1]);
                var cardsAmount = new Dictionary<char, int>();
                foreach (var character in cards)
                {
                    if (!cardsAmount.ContainsKey(character)) cardsAmount.Add(character, 0);
                    cardsAmount[character]++;
                }

                HandTypeStrengthPart1 = CalculateType(cardsAmount);

                var numberOfJokers = 0;
                if (cardsAmount.ContainsKey('J'))
                {
                    numberOfJokers = cardsAmount['J'];
                    cardsAmount['J'] = 0;
                }

                cardsAmount = cardsAmount.OrderByDescending(f => f.Value).ToDictionary();
                cardsAmount[cardsAmount.First().Key] += numberOfJokers;

                HandTypeStrengthPart2 = CalculateType(cardsAmount);
            }

            public static int CalculateType(Dictionary<char, int> cardsAmount)
            {
                int HandTypeStrenght;
                if (cardsAmount.ContainsValue(5)) HandTypeStrenght = 6;
                else if (cardsAmount.ContainsValue(4)) HandTypeStrenght = 5;
                else if (cardsAmount.ContainsValue(3) && cardsAmount.ContainsValue(2)) HandTypeStrenght = 4;
                else if (cardsAmount.ContainsValue(3)) HandTypeStrenght = 3;
                else if (cardsAmount.Count(f => f.Value == 2) == 2) HandTypeStrenght = 2;
                else if (cardsAmount.ContainsValue(2)) HandTypeStrenght = 1;
                else HandTypeStrenght = 0;
                return HandTypeStrenght;
            }

            public override string ToString()
            {
                return cards + " " + HandTypeStrengthPart1 + " " + HandTypeStrengthPart2 + " " + bid;
            }
        }

        public class Day07_Part1_Hand_Comparer : IComparer<Day07_Hand>
        {
            public int Compare(Day07_Hand? x, Day07_Hand? y)
            {
                var SubCompare = x.HandTypeStrengthPart1.CompareTo(y.HandTypeStrengthPart1);
                if (SubCompare != 0) return SubCompare;
                else
                {
                    for (var i = 0; i < x.cards.Length; i++)
                    {
                        SubCompare = CardOrder[x.cards[i]].CompareTo(CardOrder[y.cards[i]]);
                        if (SubCompare != 0) return SubCompare;
                    }
                }

                return SubCompare;
            }

            public static Dictionary<char, int> CardOrder = new Dictionary<char, int>
            {
                {'2',2 },
                {'3', 3},
                {'4', 4},
                {'5', 5},
                {'6', 6},
                {'7', 7},
                {'8', 8},
                {'9', 9},
                {'T', 10},
                {'J', 11},
                {'Q', 12},
                {'K', 13},
                {'A', 14}
            };
        }

        public class Day07_Part2_Hand_Comparer : IComparer<Day07_Hand>
        {
            public int Compare(Day07_Hand? x, Day07_Hand? y)
            {
                var SubCompare = x.HandTypeStrengthPart2.CompareTo(y.HandTypeStrengthPart2);
                if (SubCompare != 0) return SubCompare;
                else
                {
                    for (var i = 0; i < x.cards.Length; i++)
                    {
                        SubCompare = CardOrder[x.cards[i]].CompareTo(CardOrder[y.cards[i]]);
                        if (SubCompare != 0) return SubCompare;
                    }
                }

                return SubCompare;
            }

            public static Dictionary<char, int> CardOrder = new Dictionary<char, int>
            {
                {'2',2 },
                {'3', 3},
                {'4', 4},
                {'5', 5},
                {'6', 6},
                {'7', 7},
                {'8', 8},
                {'9', 9},
                {'T', 10},
                {'J', 1},
                {'Q', 12},
                {'K', 13},
                {'A', 14}
            };
        }

        public static void Day07_Main()
        {
            var input = Day07_ReadInput();
            Console.WriteLine($"Day07 Part1: {Day07_Part1(input)}");
            Console.WriteLine($"Day07 Part2: {Day07_Part2(input)}");
        }

        public static Day07_Input Day07_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day07\\Day07_input.txt").ReadToEnd();
            }

            var result = new Day07_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day07_Hand(line));
            }

            return result;
        }


        public static int Day07_Part1(Day07_Input input)
        {
            input.Sort(new Day07_Part1_Hand_Comparer());
            var result = 0;
            for(var i = 1; i<= input.Count; i++)
            {
                result += i * input[i-1].bid;
            }
            return result;
        }

        public static int Day07_Part2(Day07_Input input)
        {
            input.Sort(new Day07_Part2_Hand_Comparer());
            var result = 0;
            for (var i = 1; i <= input.Count; i++)
            {
                result += i * input[i - 1].bid;
            }
            return result;
        }

    }
    public class Day07_Test
    {
        [Theory]
        [InlineData("32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483", 6440)]
        public static void Day07Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part1(Day07.Day07_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483", 5905)]
        public static void Day07Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day07.Day07_Part2(Day07.Day07_ReadInput(rawinput)));
        }
    }
}
