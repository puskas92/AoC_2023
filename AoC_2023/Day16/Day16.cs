using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;
using Xunit.Sdk;

namespace AoC_2023
{
    public static class Day16
    {
        public class Day16_Input : Dictionary<int, Dictionary<int, Day16_Tile>> //Define input type
        {
            public void Null()
            {
                for(var i = 0; i<= this.Keys.Max(); i++)
                {
                    for(var j= 0; j<= this[i].Keys.Max(); j++)
                    {
                        var value = this[i][j];
                        value.energizedH = false;
                        value.energizedV = false;
                        this[i][j] = value;
                    }
                }
            }
            public void Shine(Point start, Point dir)
            {
                var ToCheckQueue = new Queue<(Point, Point)>();
                ToCheckQueue.Enqueue(new(start, dir));
                while (ToCheckQueue.Count > 0)
                {
                    (start, dir) = ToCheckQueue.Dequeue();
                    while (true)
                    {
                        if (!this.ContainsKey(start.X) || !this.ContainsKey(start.Y)) break;

                        var state = this[start.X][start.Y];
                        if (dir.X == 0)
                        {
                            if (state.energizedH == true && (state.tileType != '\\' && state.tileType != '/')) break;
                            state.energizedH = true;
                        }
                        else
                        {
                            if (state.energizedV == true && (state.tileType != '\\' && state.tileType != '/')) break;
                            state.energizedV = true;
                        }

                        this[start.X][start.Y] = state;

                        var shouldStop = false;
                        switch (state.tileType)
                        {
                            case '.':
                                //do nothing
                                break;
                            case '/':
                                if (dir.X == 1 && dir.Y == 0) dir = new Point(0, -1);
                                else if (dir.X == 0 && dir.Y == 1) dir = new Point(-1, 0);
                                else if (dir.X == -1 && dir.Y == 0) dir = new Point(0, 1);
                                else if (dir.X == 0 && dir.Y == -1) dir = new Point(1, 0);
                                break;
                            case '\\':
                                if (dir.X == 1 && dir.Y == 0) dir = new Point(0, 1);
                                else if (dir.X == 0 && dir.Y == -1) dir = new Point(-1, 0);
                                else if (dir.X == -1 && dir.Y == 0) dir = new Point(0, -1);
                                else if (dir.X == 0 && dir.Y == 1) dir = new Point(1, 0);
                                break;
                            case '|':
                                if (dir.X == 0)
                                {
                                    shouldStop = true;
                                    var newdir1 = new Point(1, 0);
                                    var newstart1 = new Point(start.X + newdir1.X, start.Y + newdir1.Y);
                                    //this.Shine(newstart1, newdir1);
                                    ToCheckQueue.Enqueue(new(newstart1, newdir1));

                                    var newdir2 = new Point(-1, 0);
                                    var newstart2 = new Point(start.X + newdir2.X, start.Y + newdir2.Y);
                                    //this.Shine(newstart2, newdir2);
                                    ToCheckQueue.Enqueue(new(newstart2, newdir2));
                                }
                                break;
                            case '-':
                                if (dir.Y == 0)
                                {
                                    shouldStop = true;
                                    var newdir1 = new Point(0, 1);
                                    var newstart1 = new Point(start.X + newdir1.X, start.Y + newdir1.Y);
                                    //this.Shine(newstart1, newdir1);
                                    ToCheckQueue.Enqueue(new(newstart1, newdir1));

                                    var newdir2 = new Point(0, -1);
                                    var newstart2 = new Point(start.X + newdir2.X, start.Y + newdir2.Y);
                                    //this.Shine(newstart2, newdir2);
                                    ToCheckQueue.Enqueue(new(newstart2, newdir2));
                                }
                                break;
                        }
                        if (shouldStop == true) break;

                        start = new Point(start.X + dir.X, start.Y + dir.Y);
                    }
                }
            }
        }
        public struct Day16_Tile
        {
            public char tileType;
            public bool energizedH;
            public bool energizedV;


            public Day16_Tile(char type)
            {
                tileType = type;
                energizedH = false;
                energizedV = false;
            }

        }
        public static void Day16_Main()
        {
            var input = Day16_ReadInput();
            Console.WriteLine($"Day16 Part1: {Day16_Part1(input)}");
            Console.WriteLine($"Day16 Part2: {Day16_Part2(input)}");
        }

        public static Day16_Input Day16_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day16\\Day16_input.txt").ReadToEnd();
            }

            var result = new Day16_Input();

            var row = 0;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(row, new Dictionary<int, Day16_Tile>());
                var column = 0;
                foreach(var character in line)
                {
                    result[row].Add(column, new Day16_Tile(character));
                    column++; 
                }
                row++;
            }

            return result;
        }


        public static int Day16_Part1(Day16_Input input)
        {
            input.Shine(new Point(0,0), new Point(0,1));
            return input.Sum(f=>f.Value.Count(g=> g.Value.energizedH || g.Value.energizedV));
        }

        public static int Day16_Part2(Day16_Input input)
        {
            int max = 0;
            for (int i = 0; i <= input.Keys.Max(); i++)
            {
                input.Null();
                input.Shine(new Point(i, 0), new Point(0, 1));
                max = Math.Max(max, input.Sum(f => f.Value.Count(g => g.Value.energizedH || g.Value.energizedV)));

                input.Null();
                input.Shine(new Point(i, input[i].Keys.Max()), new Point(0, -1));
                max = Math.Max(max, input.Sum(f => f.Value.Count(g => g.Value.energizedH || g.Value.energizedV)));
            }

            for (int i = 0; i <= input[0].Keys.Max(); i++)
            {
                input.Null();
                input.Shine(new Point(0, i), new Point(1, 0));
                max = Math.Max(max, input.Sum(f => f.Value.Count(g => g.Value.energizedH || g.Value.energizedV)));

                input.Null();
                input.Shine(new Point(input.Keys.Max(), i), new Point(-1, 0));
                max = Math.Max(max, input.Sum(f => f.Value.Count(g => g.Value.energizedH || g.Value.energizedV)));
            }
            return max;
        }


    }
    public class Day16_Test
    {
        [Theory]
        [InlineData(".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....", 46)]
        public static void Day16Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day16.Day16_Part1(Day16.Day16_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData(".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....", 51)]
        public static void Day16Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day16.Day16_Part2(Day16.Day16_ReadInput(rawinput)));
        }
    }
}
