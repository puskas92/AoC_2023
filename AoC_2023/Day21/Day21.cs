using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public class Day21_Input : Dictionary<int, Dictionary<int, char>> //Define input type
        {
        }
        public static void Day21_Main()
        {
            var input = Day21_ReadInput();
            Console.WriteLine($"Day21 Part1: {Day21_Part1(input, 64)}");
            Console.WriteLine($"Day21 Part2: {Day21_Part2(input, 26501365)}");
        }

        public static Day21_Input Day21_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day21\\Day21_input.txt").ReadToEnd();
            }

            var result = new Day21_Input();

            var row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(row, new Dictionary<int, char>());
                var column = 0;
                foreach (var character in line) {
                    result[row].Add(column, character);
                    column++;
                }
            row++;
            }

            return result;
        }


        public static long Day21_Part1(Day21_Input input, int step)
        {
            var ToCheckQueue = new Queue<(int d, int X, int Y)>();
            var startPosX = input.ToList().Find(f => f.Value.ContainsValue('S')).Key;
            var startPosY = input[startPosX].ToList().Find(f => f.Value == 'S').Key;
            ToCheckQueue.Enqueue((0, startPosX, startPosY));
            var visitedPlaces = new HashSet<(int d, int X, int Y)>();
            while (ToCheckQueue.Count > 0)
            {
                var ToCheck = ToCheckQueue.Dequeue();
                if (visitedPlaces.Contains(ToCheck)) continue;
                visitedPlaces.Add(ToCheck);
                if (ToCheck.d > step) break;

                foreach(var dir in CommonFunctions.CrossDirections)
                {
                    var X = ToCheck.X + dir.X;
                    var Y = ToCheck.Y + dir.Y;
                    if(input.ContainsKey(X) && input[X].ContainsKey(Y) && (input[X][Y] == '.' || input[X][Y] == 'S'))
                    {
                        ToCheckQueue.Enqueue((ToCheck.d + 1, X, Y));
                    }
                }
            }

            //for(var i = 0; i<= input.Keys.Max(); i++)
            //{
            //    var s = "";
            //    for(var j = 0; j <= input[i].Keys.Max(); j++)
            //    {
            //        if (visitedPlaces.Contains((step, i, j))) s += 'O';
            //        else s += input[i][j];
            //    }
            //    Console.WriteLine(s);
            //}

            return visitedPlaces.Count(f => f.d == step) ;
        }

        public static long Day21_Part2(Day21_Input input, int step)
        {
            var StepsToCheck = new Dictionary<int, int>()
            {
                {65, 0 },
                {196,0 },
                {327,0 }
            };

            var maxStep = StepsToCheck.Keys.Max();

            var maxX = input.Keys.Max() +1;
            var maxY = input.Keys.Max() +1;

            var ToCheckQueue = new Queue<(int d, long X, long Y)>();
            var startPosX = input.ToList().Find(f => f.Value.ContainsValue('S')).Key;
            var startPosY = input[startPosX].ToList().Find(f => f.Value == 'S').Key;
            ToCheckQueue.Enqueue((0, startPosX, startPosY));
            var visitedPlaces = new HashSet<(int d, long X, long Y)>();
            while (ToCheckQueue.Count > 0)
            {
                var ToCheck = ToCheckQueue.Dequeue();
                if (visitedPlaces.Contains(ToCheck)) continue;
                visitedPlaces.Add(ToCheck);
                if (StepsToCheck.ContainsKey(ToCheck.d-1) && StepsToCheck[ToCheck.d - 1]==0)
                {
                    var visited = visitedPlaces.Count(f => f.d == ToCheck.d-1);
                    StepsToCheck[ToCheck.d-1] = visited;
                }
                if (ToCheck.d > maxStep) break;

                foreach (var dir in CommonFunctions.CrossDirections)
                {
                    long X = (ToCheck.X + dir.X);
                    long Y = (ToCheck.Y + dir.Y);
                    int shortX = (int)(((X % maxX) + maxX) % maxX);
                    int shortY = (int)(((Y % maxY) + maxY) % maxY);
                    Debug.Assert(input.ContainsKey(shortX) && input[shortX].ContainsKey(shortY));

                    if ((input[shortX][shortY] == '.' || input[shortX][shortY] == 'S'))
                    {
                        ToCheckQueue.Enqueue((ToCheck.d + 1, X, Y));
                    }
                }
            }

            long C = StepsToCheck[65];
            long AplusB = StepsToCheck[196] - C;
            long A = (long)(0.5 * (StepsToCheck[327] - C - 2 * AplusB));
            long B = AplusB - A;

            long Z = 202300; // 26501365 = 2023*100;


            long result = Z * Z * A + Z * B + C;
            return result;
        }


    }
    public class Day21_Test
    {
        [Theory]
        [InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 6, 16)]
        public static void Day21Part1Test(string rawinput, int step,  long expectedValue)
        {
            Assert.Equal(expectedValue, Day21.Day21_Part1(Day21.Day21_ReadInput(rawinput), step));
        }

        //[Theory]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 6, 16)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 10, 50)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 50, 1594)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 100, 6536)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 500, 167004)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 1000, 668697)]
        //[InlineData("...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", 5000, 16733044)]

        //public static void Day21Part2Test(string rawinput, int step, long expectedValue)
        //{
        //    Assert.Equal(expectedValue, Day21.Day21_Part2(Day21.Day21_ReadInput(rawinput), step));
        //}
    }
}
