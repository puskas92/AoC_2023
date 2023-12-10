using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day10
    {
        public class Day10_Input
        {
            public Dictionary<int, Dictionary<int, char>> map;
            public (int X, int Y) startPos;
            public int LoopLength = 0;
            public List<(int X, int Y)> PipeParts;
            public HashSet<(int X, int Y)> OneSideRegionSeeds;

            public void DoALoop()
            {
                PipeParts = new List<(int X, int Y)>();
                OneSideRegionSeeds = new HashSet<(int X, int Y)>();

                var currentPos = startPos;
                var currentDirFirst = Day10_PipeTypes[map[currentPos.X][currentPos.Y]].First();
                (int X, int Y) fromDir = new(currentDirFirst.X * -1, currentDirFirst.Y * -1);
                LoopLength = 0;
                do
                {
                    PipeParts.Add(currentPos);
                    LoopLength++;
                    var CurrentDirs = Day10_PipeTypes[map[currentPos.X][currentPos.Y]];
                    var nextDir = CurrentDirs.Find(f => f.X != fromDir.X * -1 || f.Y != fromDir.Y * -1);

                    (int X, int Y) sideDir1 = new(nextDir.Y*-1, nextDir.X );
                    (int X, int Y) sideDir2 = new(fromDir.Y*-1, fromDir.X );
                    OneSideRegionSeeds.Add(new(currentPos.X + sideDir1.X, currentPos.Y + sideDir1.Y));
                    OneSideRegionSeeds.Add(new(currentPos.X + sideDir2.X, currentPos.Y + sideDir2.Y));


                    currentPos = new(currentPos.X + nextDir.X, currentPos.Y + nextDir.Y);


                    fromDir = nextDir;

                } while (currentPos.X != startPos.X || currentPos.Y != startPos.Y);

                foreach(var pos in PipeParts)
                {
                    if (OneSideRegionSeeds.Contains(pos)) OneSideRegionSeeds.Remove(pos);
                }
            }
        }
        public static void Day10_Main()
        {
            var input = Day10_ReadInput();
            Console.WriteLine($"Day10 Part1: {Day10_Part1(input)}");
            Console.WriteLine($"Day10 Part2: {Day10_Part2(input)}");
        }

        public static Dictionary<char, List<(int X, int Y)>> Day10_PipeTypes = new Dictionary<char, List<(int X, int Y)>>
        {
            {'L', new List<(int X, int Y)>() { new (-1,0), new (0,1)} },
            {'J', new List<(int X, int Y)>() { new (-1,0), new (0,-1)} },
            {'7', new List<(int X, int Y)>() { new (1,0), new (0,-1)} },
            {'F', new List<(int X, int Y)>() { new (1,0), new (0,1)} },
            {'|', new List<(int X, int Y)>() { new (-1,0), new (1,0)} },
            {'-', new List<(int X, int Y)>() { new (0,-1), new (0,1)} },
        };
        public static Day10_Input Day10_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day10\\Day10_input.txt").ReadToEnd();
            }

            var result = new Day10_Input();
            result.map = new Dictionary<int, Dictionary<int, char>>();
            var i = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.map.Add(i, new Dictionary<int, char>());
                var j = 0;
                foreach(var character in line)
                {
                    result.map[i].Add(j, character);
                    if (character == 'S') result.startPos = new(i, j);
                    j++;
                }
                i++;
            }

            var currPossibleDirs = new List<(int X, int Y)>();
            foreach (var dir in CommonFunctions.CrossDirections)
            {
                var X = result.startPos.X + dir.X;
                var Y = result.startPos.Y + dir.Y;
                if (result.map.ContainsKey(X) && result.map[X].ContainsKey(Y))
                {
                    var neighChar = result.map[X][Y];
                    if (Day10_PipeTypes.ContainsKey(neighChar) && Day10_PipeTypes[neighChar].Contains(new(-1 * dir.X, -1 * dir.Y))) currPossibleDirs.Add(new(dir.X, dir.Y));
                }
            }
            var startPosChar = Day10_PipeTypes.ToList().Find(f => f.Value.Contains(currPossibleDirs.First()) && f.Value.Contains(currPossibleDirs.Last())).Key;
            result.map[result.startPos.X][result.startPos.Y] = startPosChar;


            return result;
        }


        public static int Day10_Part1(Day10_Input input)
        {
            if (input.LoopLength == 0) input.DoALoop();
            return (int)Math.Ceiling(input.LoopLength / 2.0);
        }

        public static int Day10_Part2(Day10_Input input)
        {
            if (input.LoopLength == 0) input.DoALoop();

            var RegionMap = new HashSet<(int X, int Y)>();
            var ToVisitQueue = new Queue<(int X, int Y)>();
            foreach(var seed in input.OneSideRegionSeeds)
            {
                ToVisitQueue.Enqueue(seed);
            }

            while(ToVisitQueue.Count > 0)
            {
                var ToCheck = ToVisitQueue.Dequeue();
                if (!input.map.ContainsKey(ToCheck.X) || !input.map[ToCheck.X].ContainsKey(ToCheck.Y)) continue;
                if (input.PipeParts.Contains(ToCheck)) continue;
                if (RegionMap.Contains(ToCheck)) continue;
                RegionMap.Add(ToCheck);
                foreach(var dir in CommonFunctions.CrossDirections)
                {
                    var X = ToCheck.X + dir.X;
                    var Y = ToCheck.Y + dir.Y;
                    if (input.map.ContainsKey(X) && input.map[X].ContainsKey(Y)) ToVisitQueue.Enqueue(new(X, Y));
                }
            }

            //Visualize the output for debugging
            //for(var i = 0;i<= input.map.Keys.Max(); i++)
            //{
            //    var s = "";
            //    for(var j = 0; j<= input.map[i].Keys.Max(); j++)
            //    {
            //        (int X, int Y) coord = new(i, j);
            //        if (input.PipeParts.Contains(coord)) s += input.map[i][j];
            //        else if (RegionMap.Contains(coord)) s += 'O';
            //        else s += '.';
            //    }
            //    Console.WriteLine(s);
            //}


            if(RegionMap.Contains(new (0,0)))
            {
                var MapSize = (input.map.Keys.Max() + 1) * (input.map[0].Keys.Max() + 1);
                return MapSize - RegionMap.Count - input.PipeParts.Count;
            }
            else
            {
                return RegionMap.Count;
            }
           
        }


    }
    public class Day10_Test
    {
        [Theory]
        [InlineData(".....\r\n.S-7.\r\n.|.|.\r\n.L-J.\r\n.....", 4)]
        [InlineData("-L|F7\r\n7S-7|\r\nL|7||\r\n-L-J|\r\nL|-JF", 4)]
        [InlineData("7-F7-\r\n.FJ|7\r\nSJLL7\r\n|F--J\r\nLJ.LJ", 8)]
        public static void Day10Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part1(Day10.Day10_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("...........\r\n.S-------7.\r\n.|F-----7|.\r\n.||.....||.\r\n.||.....||.\r\n.|L-7.F-J|.\r\n.|..|.|..|.\r\n.L--J.L--J.\r\n...........", 4)]
        [InlineData(".F----7F7F7F7F-7....\r\n.|F--7||||||||FJ....\r\n.||.FJ||||||||L7....\r\nFJL7L7LJLJ||LJ.L-7..\r\nL--J.L7...LJS7F-7L7.\r\n....F-J..F7FJ|L7L7L7\r\n....L7.F7||L7|.L7L7|\r\n.....|FJLJ|FJ|F7|.LJ\r\n....FJL-7.||.||||...\r\n....L---J.LJ.LJLJ...", 8)]
        [InlineData("FF7FSF7F7F7F7F7F---7\r\nL|LJ||||||||||||F--J\r\nFL-7LJLJ||||||LJL-77\r\nF--JF--7||LJLJ7F7FJ-\r\nL---JF-JLJ.||-FJLJJ7\r\n|F|F-JF---7F7-L7L|7|\r\n|FFJF7L7F-JF7|JL---7\r\n7-L-JL7||F7|L7F-7F7|\r\nL.L7LFJ|||||FJL7||LJ\r\nL7JLJL-JLJLJL--JLJ.L", 10)]
        public static void Day10Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day10.Day10_Part2(Day10.Day10_ReadInput(rawinput)));
        }
    }
}
