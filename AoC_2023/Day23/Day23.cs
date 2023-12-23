using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day23
    {
        public class Day23_Input //Define input type
        {
            public Dictionary<int, Dictionary<int, char>> map;

            public (int X, int Y) EndPos;

            public (int X, int Y) StartPos = (0, 1);
            public Dictionary<(int X, int Y), List<((int X, int Y) pos, int distance)>> CalculateDistanceTreePart1()
            {
                
                var distanceTree = new Dictionary<(int X, int Y), List<((int X, int Y) pos, int distance)>>();
                distanceTree.Add(StartPos, new List<((int X, int Y), int distance)>());
                var endRow = map.Keys.Max();

                var toCheckQueue = new Queue<((int X, int Y) pos, (int X, int Y) startPos, (int X, int Y) prevPos, int dist, bool forward, bool backward)>();
                toCheckQueue.Enqueue((StartPos, StartPos, (-1,1), 0, true, true));

                var listOfVisitedJunk = new List<(int X, int Y)>();
                while(toCheckQueue.Count > 0)
                {
                    var toCheck = toCheckQueue.Dequeue();
                    //listOfVisited.Add(toCheck.pos);
                    var dist = toCheck.dist ;
                    var forward = toCheck.forward;
                    var backward = toCheck.backward;
                    var startPos = toCheck.startPos;
                    if (toCheck.pos.X == endRow)
                    {
                        EndPos = toCheck.pos;
                        Debug.Assert(forward == true);
                        distanceTree[startPos].Add((toCheck.pos, dist));
                        continue;
                    }

                    var possibleDirectionCount = 0;
                    foreach (var dir in CommonFunctions.CrossDirections)
                    {
                        var X = toCheck.pos.X + dir.X;
                        var Y = toCheck.pos.Y + dir.Y;
                        if (!map.ContainsKey(X) || !map[X].ContainsKey(Y)) continue;
                        if (map[X][Y] != '#') possibleDirectionCount += 1;
                    }

                    //if (possibleDirectionCount <= 1) continue;
                    if (possibleDirectionCount > 2)
                    {
                        if(!distanceTree.ContainsKey(toCheck.pos)) distanceTree.Add(toCheck.pos, new List<((int X, int Y), int distance)>());
                        if (forward)
                        {
                            if (!distanceTree[startPos].Contains((toCheck.pos, dist))) distanceTree[startPos].Add((toCheck.pos, dist));
                        }
                        if (backward)
                        {
                            if (!distanceTree[toCheck.pos].Contains((startPos, dist)))  distanceTree[toCheck.pos].Add((startPos, dist));
                        }
                        startPos = toCheck.pos;
                        dist = 0;
                        forward = true;
                        backward = true;
                        if (!listOfVisitedJunk.Contains(toCheck.pos)) listOfVisitedJunk.Add(toCheck.pos);
                        else continue;
                    }


                   
                  
                    foreach (var dir in CommonFunctions.CrossDirections)
                    {
                        var X = toCheck.pos.X + dir.X;
                        var Y = toCheck.pos.Y + dir.Y;
                        var dirforward = forward;
                        var dirbackward = backward;
                        if (!map.ContainsKey(X) || !map[X].ContainsKey(Y)) continue;
                        switch (map[X][Y])
                        {
                            case '#':
                                continue;
                            case '>':
                                if (dir.X == 0 && dir.Y == -1) dirforward = false;
                                else if (dir.X == 0 && dir.Y == 1) dirbackward = false;
                                break;
                            case '<':
                                if (dir.X == 0 && dir.Y == 1) dirforward = false;
                                else if (dir.X == 0 && dir.Y == -1) dirbackward = false;
                                break;
                            case '^':
                                if (dir.X == 1 && dir.Y == 0) dirforward = false;
                                else if (dir.X == -1 && dir.Y == 0) dirbackward = false;
                                break;
                            case 'v':
                                if (dir.X == -1 && dir.Y == 0) dirforward = false;
                                else if (dir.X == 1 && dir.Y == 0) dirbackward = false;
                                break;
                            default:
                                break;
                        }

                        (int X, int Y) toPoint = (X, Y);
                        //if (toPoint == startPos) continue;
                        // if (listOfVisited.Contains(toPoint) && !distanceTree.ContainsKey(toPoint)) continue;
                        if (toPoint == toCheck.prevPos) continue;
                        toCheckQueue.Enqueue((toPoint, startPos, toCheck.pos, dist + 1, dirforward, dirbackward));
                    }

                }
                return distanceTree;
            }

            public Dictionary<(int X, int Y), List<((int X, int Y) pos, int distance)>> CalculateDistanceTreePart2()
            {

                var distanceTree = new Dictionary<(int X, int Y), List<((int X, int Y) pos, int distance)>>();
                distanceTree.Add(StartPos, new List<((int X, int Y), int distance)>());
                var endRow = map.Keys.Max();

                var toCheckQueue = new Queue<((int X, int Y) pos, (int X, int Y) startPos, (int X, int Y) prevPos, int dist)>();
                toCheckQueue.Enqueue((StartPos, StartPos, (-1, 1), 0));

                var listOfVisitedJunk = new List<(int X, int Y)>();
                while (toCheckQueue.Count > 0)
                {
                    var toCheck = toCheckQueue.Dequeue();
                    //listOfVisited.Add(toCheck.pos);
                    var dist = toCheck.dist;
                    var startPos = toCheck.startPos;
                    if (toCheck.pos.X == endRow)
                    {
                        EndPos = toCheck.pos;
                        distanceTree[startPos].Add((toCheck.pos, dist));
                        continue;
                    }

                    var possibleDirectionCount = 0;
                    foreach (var dir in CommonFunctions.CrossDirections)
                    {
                        var X = toCheck.pos.X + dir.X;
                        var Y = toCheck.pos.Y + dir.Y;
                        if (!map.ContainsKey(X) || !map[X].ContainsKey(Y)) continue;
                        if (map[X][Y] != '#') possibleDirectionCount += 1;
                    }

                    //if (possibleDirectionCount <= 1) continue;
                    if (possibleDirectionCount > 2)
                    {
                        if (!distanceTree.ContainsKey(toCheck.pos)) distanceTree.Add(toCheck.pos, new List<((int X, int Y), int distance)>());
                        if (!distanceTree[startPos].Contains((toCheck.pos, dist))) distanceTree[startPos].Add((toCheck.pos, dist));
                        if (!distanceTree[toCheck.pos].Contains((startPos, dist))) distanceTree[toCheck.pos].Add((startPos, dist));
                        startPos = toCheck.pos;
                        dist = 0;
                        if (!listOfVisitedJunk.Contains(toCheck.pos)) listOfVisitedJunk.Add(toCheck.pos);
                        else continue;
                    }




                    foreach (var dir in CommonFunctions.CrossDirections)
                    {
                        var X = toCheck.pos.X + dir.X;
                        var Y = toCheck.pos.Y + dir.Y;
                        if (!map.ContainsKey(X) || !map[X].ContainsKey(Y)) continue;
                        if (map[X][Y] == '#') continue;
                        (int X, int Y) toPoint = (X, Y);
                        //if (toPoint == startPos) continue;
                        // if (listOfVisited.Contains(toPoint) && !distanceTree.ContainsKey(toPoint)) continue;
                        if (toPoint == toCheck.prevPos) continue;
                        toCheckQueue.Enqueue((toPoint, startPos, toCheck.pos, dist + 1));
                    }

                }
                return distanceTree;
            }
        }
        public static void Day23_Main()
        {
            var input = Day23_ReadInput();
            Console.WriteLine($"Day23 Part1: {Day23_Part1(input)}");
            Console.WriteLine($"Day23 Part2: {Day23_Part2(input)}");
        }

        public static Day23_Input Day23_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day23\\Day23_input.txt").ReadToEnd();
            }

            var result = new Day23_Input();
            result.map = new Dictionary<int, Dictionary<int, char>>();

            var column = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var row = 0;
                result.map.Add(column, new Dictionary<int, char>());
                foreach(var character in line)
                {
                    result.map[column].Add(row, character);
                    row++;
                }
                column++;
            }

            return result;
        }


        public static int Day23_Part1(Day23_Input input)
        {
            var distTree = input.CalculateDistanceTreePart1();

            int MaxTrip = CalculateMaxPathWithDistTree(input, distTree);

            return MaxTrip;
        }

        private static int CalculateMaxPathWithDistTree(Day23_Input input, Dictionary<(int X, int Y), List<((int X, int Y) pos, int distance)>> distTree)
        {
            var MaxTrip = 0;
            var ToCheckQueue = new Queue<((int X, int Y) pos, int d, List<(int X, int Y)> visited)>();
            ToCheckQueue.Enqueue((input.StartPos, 0, new List<(int X, int Y)>()));
            while (ToCheckQueue.Count > 0)
            {
                var ToCheck = ToCheckQueue.Dequeue();
                if (ToCheck.pos == input.EndPos)
                {
                    MaxTrip = Math.Max(ToCheck.d, MaxTrip);
                    continue;
                }
                var possibleDirections = distTree[ToCheck.pos];
                var VisitedCopy = new List<(int X, int Y)>(ToCheck.visited);
                VisitedCopy.Add(ToCheck.pos);
                foreach (var dir in possibleDirections)
                {
                    if (VisitedCopy.Contains(dir.pos)) continue;
                    ToCheckQueue.Enqueue((dir.pos, ToCheck.d + dir.distance, VisitedCopy));
                }
            }

            return MaxTrip;
        }

        public static int Day23_Part2(Day23_Input input)
        {
            var distTree = input.CalculateDistanceTreePart2();

            int MaxTrip = CalculateMaxPathWithDistTree(input, distTree);

            return MaxTrip;
        }


    }
    public class Day23_Test
    {
        [Theory]
        [InlineData("#.#####################\r\n#.......#########...###\r\n#######.#########.#.###\r\n###.....#.>.>.###.#.###\r\n###v#####.#v#.###.#.###\r\n###.>...#.#.#.....#...#\r\n###v###.#.#.#########.#\r\n###...#.#.#.......#...#\r\n#####.#.#.#######.#.###\r\n#.....#.#.#.......#...#\r\n#.#####.#.#.#########v#\r\n#.#...#...#...###...>.#\r\n#.#.#v#######v###.###v#\r\n#...#.>.#...>.>.#.###.#\r\n#####v#.#.###v#.#.###.#\r\n#.....#...#...#.#.#...#\r\n#.#########.###.#.#.###\r\n#...###...#...#...#.###\r\n###.###.#.###v#####v###\r\n#...#...#.#.>.>.#.>.###\r\n#.###.###.#.###.#.#v###\r\n#.....###...###...#...#\r\n#####################.#", 94)]
        public static void Day23Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day23.Day23_Part1(Day23.Day23_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("#.#####################\r\n#.......#########...###\r\n#######.#########.#.###\r\n###.....#.>.>.###.#.###\r\n###v#####.#v#.###.#.###\r\n###.>...#.#.#.....#...#\r\n###v###.#.#.#########.#\r\n###...#.#.#.......#...#\r\n#####.#.#.#######.#.###\r\n#.....#.#.#.......#...#\r\n#.#####.#.#.#########v#\r\n#.#...#...#...###...>.#\r\n#.#.#v#######v###.###v#\r\n#...#.>.#...>.>.#.###.#\r\n#####v#.#.###v#.#.###.#\r\n#.....#...#...#.#.#...#\r\n#.#########.###.#.#.###\r\n#...###...#...#...#.###\r\n###.###.#.###v#####v###\r\n#...#...#.#.>.>.#.>.###\r\n#.###.###.#.###.#.#v###\r\n#.....###...###...#...#\r\n#####################.#", 154)]
        public static void Day23Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day23.Day23_Part2(Day23.Day23_ReadInput(rawinput)));
        }
    }
}
