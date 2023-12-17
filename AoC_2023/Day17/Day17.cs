using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day17
    {
        public class Day17_Input : Dictionary<int, Dictionary<int, int>> //Define input type
        {
        }
        public record struct Day17_MapState(Point position, int heatloss, byte dirCode, byte numOfStaight);
        public static void Day17_Main()
        {
            var input = Day17_ReadInput();
            Console.WriteLine($"Day17 Part1: {Day17_Part1(input)}");
            Console.WriteLine($"Day17 Part2: {Day17_Part2(input)}");
        }

        public static Day17_Input Day17_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day17\\Day17_input.txt").ReadToEnd();
            }

            var result = new Day17_Input();

            var row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(row, new Dictionary<int, int>());
                var column = 0;
                foreach (var num in line)
                {
                    result[row].Add(column, int.Parse(num.ToString()));
                    column++;
                }
                row++;
            }

            return result;
        }


        public static int Day17_Part1(Day17_Input input)
        {
            return Day17_Simulate(input, 3, 0);
        }

        public static int Day17_Part2(Day17_Input input)
        {
            return Day17_Simulate(input, 10, 4);
        }

        public static int Day17_Simulate(Day17_Input input, int numberOfMaxInDirection, int minimumToADirection)
        {
            var ToCheckQueue = new PriorityQueue<Day17_MapState, int>();
            var PositionCheck = new Dictionary<(Point, byte), Day17_MapState>();
            var minHeatLoss = int.MaxValue;
            var MaxX = input.Keys.Max();
            var MaxY = input[0].Keys.Max();

            ToCheckQueue.Enqueue(new Day17_MapState(new Point(0, 0), 0, 4, 0), MaxX + MaxY);

            var found = false;
            while (ToCheckQueue.Count > 0)
            {
                var ToCheck = ToCheckQueue.Dequeue();

                if (found) break;
                for (byte i = 0; i < 4; i++)
                {
                    if (ToCheck.dirCode != 4 && i != ToCheck.dirCode && (ToCheck.numOfStaight) < minimumToADirection) continue;
                    var dir = CommonFunctions.CrossDirections[i];
                    if (((i + 2) % 4) == ToCheck.dirCode) continue; //can't go back
                    var X = ToCheck.position.X + dir.X;
                    var Y = ToCheck.position.Y + dir.Y;
                    if (!input.ContainsKey(X) || !input[X].ContainsKey(Y)) continue;
                    var Heatloss = ToCheck.heatloss + input[X][Y];
                    byte nextNumOfDir = (byte)((i == ToCheck.dirCode) ? (ToCheck.numOfStaight + 1) : 1);
                    if (nextNumOfDir > numberOfMaxInDirection) continue;
                    if (X == MaxX && Y == MaxY )
                    {
                        if (nextNumOfDir >= minimumToADirection)
                        {
                            minHeatLoss = Heatloss;
                            found = true;
                            break;
                        }
                    }
                    var Priority = Heatloss + ((MaxX - X) + (MaxY - Y)) * 1;
                    var newPos = new Point(X, Y);
                    var newState = new Day17_MapState(newPos, Heatloss, i, nextNumOfDir);
                    (Point, byte) cacheKey = new(newPos, i);
                    if (nextNumOfDir >= minimumToADirection)
                    {
                        if (PositionCheck.ContainsKey(cacheKey))
                        {
                            var CachedPos = PositionCheck[cacheKey];
                            if (CachedPos.heatloss <= newState.heatloss && CachedPos.numOfStaight <= newState.numOfStaight) continue;
                            if (CachedPos.heatloss <= newState.heatloss) PositionCheck[cacheKey] = newState;
                        }
                        else PositionCheck.Add(cacheKey, newState);
                    }
                    ToCheckQueue.Enqueue(newState, Priority);
                }
            }

            return minHeatLoss;
        }
    }
    public class Day17_Test
    {
        [Theory]
        [InlineData("2413432311323\r\n3215453535623\r\n3255245654254\r\n3446585845452\r\n4546657867536\r\n1438598798454\r\n4457876987766\r\n3637877979653\r\n4654967986887\r\n4564679986453\r\n1224686865563\r\n2546548887735\r\n4322674655533", 102)]
        public static void Day17Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day17.Day17_Part1(Day17.Day17_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("2413432311323\r\n3215453535623\r\n3255245654254\r\n3446585845452\r\n4546657867536\r\n1438598798454\r\n4457876987766\r\n3637877979653\r\n4654967986887\r\n4564679986453\r\n1224686865563\r\n2546548887735\r\n4322674655533", 94)]
        [InlineData("111111111111\r\n999999999991\r\n999999999991\r\n999999999991\r\n999999999991", 71)]
        public static void Day17Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day17.Day17_Part2(Day17.Day17_ReadInput(rawinput)));
        }
    }
}
