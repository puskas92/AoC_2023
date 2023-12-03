using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day03
    {
        public class Day03_Input : List<Day03_PartNumber> //Define input type
        {
        }

        public record Day03_PartNumber(int number, List<Day03_AdjacentSymbol> adjacentSymbols);
        public record struct Day03_AdjacentSymbol(int X, int Y, char symbol);

        public static void Day03_Main()
        {
            var input = Day03_ReadInput();
            Console.WriteLine($"Day03 Part1: {Day03_Part1(input)}");
            Console.WriteLine($"Day03 Part2: {Day03_Part2(input)}");
        }

        public static Day03_Input Day03_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day03\\Day03_input.txt").ReadToEnd();
            }

            var result = new Day03_Input();
            var map = new Dictionary<int, Dictionary<int, char>>();

            var row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                map.Add(row, new Dictionary<int, char>());
                var column = 0;
                foreach(var character in line)
                {
                    map[row].Add(column, character);
                    column++;
                }
                row++;
            }

            foreach(var mapLine in map)
            {
                var isNumberStarted = false;
                var numberString = "";
                foreach(var mapChar in mapLine.Value)
                {
                    if (char.IsNumber(mapChar.Value))
                    {
                        numberString += mapChar.Value;
                        isNumberStarted = true;
                    }
                    else if (isNumberStarted)
                    {
                        Day03_PartNumber number = NumberFoundSearchForAdjacent(map,  numberString, mapLine.Key, mapChar.Key);

                        result.Add(number);

                        numberString = "";
                        isNumberStarted = false;
                    }
                }
                if(isNumberStarted)
                {
                    Day03_PartNumber number = NumberFoundSearchForAdjacent(map,  numberString, mapLine.Key, mapLine.Value.Count);
                    result.Add(number);
                }

            }

            return result;
        }

        private static Day03_PartNumber NumberFoundSearchForAdjacent(Dictionary<int, Dictionary<int, char>> map, string numberString, int row, int column)
        {
            var number = new Day03_PartNumber(int.Parse(numberString), new List<Day03_AdjacentSymbol>());

            //search for adjacent symbols
            for (var X = row - 1; X <= row + 1; X++)
            {
                for (var Y = column - (numberString.Length + 1); Y <= column; Y++)
                {
                    if (map.ContainsKey(X) && map[X].ContainsKey(Y) && map[X][Y] != '.' && !char.IsNumber(map[X][Y])) number.adjacentSymbols.Add(new Day03_AdjacentSymbol(X, Y, map[X][Y]));
                }
            }

            return number;
        }

        public static int Day03_Part1(Day03_Input input)
        {
            return input.Sum(f => (f.adjacentSymbols.Count > 0) ? f.number : 0);
        }

        public static int Day03_Part2(Day03_Input input)
        {
            var parts = new Dictionary<Day03_AdjacentSymbol, List<Day03_PartNumber>>();
            foreach(var partnumber in input)
            {
                foreach(var symbol in partnumber.adjacentSymbols)
                {
                    if (!parts.ContainsKey(symbol)) parts.Add(symbol, new List<Day03_PartNumber>());
                    parts[symbol].Add(partnumber);
                }
            }

             return parts.Where(f => f.Key.symbol == '*' && f.Value.Count == 2)
                .Sum(f => (f.Value[0].number * f.Value[1].number));
        }


    }
    public class Day03_Test
    {
        [Theory]
        [InlineData("467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", 4361)]
        public static void Day03Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part1(Day03.Day03_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", 467835)]
        public static void Day03Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day03.Day03_Part2(Day03.Day03_ReadInput(rawinput)));
        }
    }
}
