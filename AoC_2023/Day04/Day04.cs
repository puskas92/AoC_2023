using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day04
    {
        public class Day04_Input : Dictionary<int, Day04_Card> //Define input type
        {
        }

        public class Day04_Card
        {
            public int numberOfMatch;
            public int points;
            public int id;
            public List<int> winningNumbers;
            public List<int> numbersYouHave;

            public Day04_Card(int id, List<int> winningNumbers, List<int> numbersYouHave)
            {
                this.id = id;
                this.winningNumbers = winningNumbers;
                this.numbersYouHave = numbersYouHave;

                numberOfMatch = numbersYouHave.Intersect(winningNumbers).Count();
                points = (numberOfMatch == 0) ? 0 : (int)Math.Pow(2, numberOfMatch - 1);
            }
        };
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
                var id = int.Parse(line.Split(':')[0].Split(' ').Last());
                var winningNumbers = line.Split(':')[1].Split('|')[0].Trim().Split(' ').Where(f=> f!="").Select(f => int.Parse(f)).ToList();
                var numbersYouHave = line.Split('|')[1].Trim().Split(' ').Where(f => f != "").Select(f => int.Parse(f)).ToList();
                var card = new Day04_Card(id,winningNumbers,numbersYouHave);
                result.Add(id, card);
            }

            return result;
        }


        public static int Day04_Part1(Day04_Input input)
        {
            return input.Sum(f => f.Value.points);
        }

        public static int Day04_Part2(Day04_Input input)
        {
            var numberOfCards = new List<int>();
            foreach (var card in input) numberOfCards.Add(1);
            for (int i = 0; i<=input.Count-1; i++)
            {
                for(int j = 1; j <= input[i+1].numberOfMatch; j++)
                {
                    if (numberOfCards.Count < (i + j)) continue;
                    numberOfCards[i + j] += numberOfCards[i];
                }
            }

            return numberOfCards.Sum();
        }


    }
    public class Day04_Test
    {
        [Theory]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 13)]
        public static void Day04Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part1(Day04.Day04_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 30)]
        public static void Day04Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day04.Day04_Part2(Day04.Day04_ReadInput(rawinput)));
        }
    }
}
