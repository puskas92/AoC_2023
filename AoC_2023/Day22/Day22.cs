using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day22
    {
        public class Day22_Input : List<Day22_Brick> //Define input type
        {
            public int safelyDisintegrateCount = 0;
            public int sumOfNumberOfOtherBricksWouldFall = 0;
            public void  FallDown()
            {
                var isStillFalling = true;
                var BrickSupport = new Dictionary<Day22_Brick, List<Day22_Brick>>();

                while (isStillFalling)
                {
                    isStillFalling = false;
                    var bottomDict = new Dictionary<int, Dictionary<(int X, int Y), Day22_Brick>>();
                    var topDict = new Dictionary<int, Dictionary<(int X, int Y), Day22_Brick>>();
                    foreach (var brick in this)
                    {
                        var bottom = brick.start.Z;
                        if (!bottomDict.ContainsKey(bottom)) bottomDict.Add(bottom, new Dictionary<(int X, int Y), Day22_Brick>());
                        var top = brick.end.Z;
                        if (!topDict.ContainsKey(top)) topDict.Add(top, new Dictionary<(int X, int Y), Day22_Brick>());
                        for (var X = brick.start.X; X <= brick.end.X; X++)
                        {
                            for (var Y = brick.start.Y; Y <= brick.end.Y; Y++)
                            {
                                bottomDict[bottom].Add((X, Y), brick);
                                topDict[top].Add((X, Y), brick);
                            }
                        }
                    }

                    BrickSupport = new Dictionary<Day22_Brick, List<Day22_Brick>>();

                    for (var i = bottomDict.Keys.Min(); i <= bottomDict.Keys.Max(); i++)
                    {
                        if (i <= 1) continue;
                        if (!bottomDict.ContainsKey(i)) continue;
                        foreach (var brickToFall in bottomDict[i])
                        {
                            if (topDict.ContainsKey(i - 1))
                            {
                                if (topDict[i - 1].ContainsKey((brickToFall.Key.X, brickToFall.Key.Y)))
                                {
                                    var clashBrick = topDict[i - 1][(brickToFall.Key.X, brickToFall.Key.Y)];
                                    if (!BrickSupport.ContainsKey(brickToFall.Value)) BrickSupport.Add(brickToFall.Value, new List<Day22_Brick>());
                                    if (!BrickSupport[brickToFall.Value].Contains(clashBrick)) BrickSupport[brickToFall.Value].Add(clashBrick);
                                }
                            }
                        }
                    }

                    for (var i = 0; i < this.Count(); i++)
                    {
                        var Brick = this[i];
                        if (!BrickSupport.ContainsKey(Brick) && Brick.start.Z != 1)
                        {
                            isStillFalling = true;
                            Brick.start = (Brick.start.X, Brick.start.Y, Brick.start.Z - 1);
                            Brick.end = (Brick.end.X, Brick.end.Y, Brick.end.Z - 1);
                        }
                        this[i] = Brick;
                    }
                }

                safelyDisintegrateCount = 0;
                foreach(var Brick in this)
                {
                    var onlySupporting = false;
                    foreach(var BrickToSupport in BrickSupport)
                    {
                        if (BrickToSupport.Value.Contains(Brick) && BrickToSupport.Value.Count() == 1 && BrickToSupport.Key.start.Z != 1)
                        {
                            onlySupporting = true;
                            break;
                        }
                    }
                    if (onlySupporting == false) safelyDisintegrateCount += 1;

                    sumOfNumberOfOtherBricksWouldFall += CalculateNumberOfFallsIfBricksDisintegrate(BrickSupport, new List<Day22_Brick>() { Brick });
                }


               
            }
            private int CalculateNumberOfFallsIfBricksDisintegrate(Dictionary<Day22_Brick, List<Day22_Brick>> brickSupport, List<Day22_Brick> disBricks)
            {
                var isDisIntegrateChanged = false;
                do
                {
                    isDisIntegrateChanged = false;
                    foreach (var BrickToSupport in brickSupport)
                    {
                        if (BrickToSupport.Key.start.Z != 1 && !BrickToSupport.Value.Any(f => !disBricks.Contains(f)))
                        {
                            if (!disBricks.Contains(BrickToSupport.Key))
                            {
                                disBricks.Add(BrickToSupport.Key);
                                isDisIntegrateChanged = true;
                            }
                        }
                    }
                } while (isDisIntegrateChanged);
               
                return disBricks.Count -1 ;
            }
        }
        public struct Day22_Brick
        {
            public (int X, int Y, int Z) start;
            public (int X, int Y, int Z) end;

            public Day22_Brick(string raw)
            {
                var splitted = raw.Split('~');
                var startList = splitted[0].Split(',').Select(f => int.Parse(f.Trim())).ToList();
                start = (startList[0], startList[1], startList[2]);
                var endList = splitted[1].Split(',').Select(f => int.Parse(f.Trim())).ToList();
                end = (endList[0], endList[1], endList[2]);
                Debug.Assert(start.X <= end.X);
                Debug.Assert(start.Y <= end.Y);
                Debug.Assert(start.Z <= end.Z);

            }
        }
        public static void Day22_Main()
        {
            var input = Day22_ReadInput();
            Console.WriteLine($"Day22 Part1: {Day22_Part1(input)}");
            Console.WriteLine($"Day22 Part2: {Day22_Part2(input)}");
        }

        public static Day22_Input Day22_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day22\\Day22_input.txt").ReadToEnd();
            }

            var result = new Day22_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day22_Brick(line));
            }
            result.FallDown();

            return result;
        }


        public static int Day22_Part1(Day22_Input input)
        {

            return input.safelyDisintegrateCount;
        }

        public static int Day22_Part2(Day22_Input input)
        {
            return input.sumOfNumberOfOtherBricksWouldFall;
        }


    }
    public class Day22_Test
    {
        [Theory]
        [InlineData("1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9", 5)]
        public static void Day22Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day22.Day22_Part1(Day22.Day22_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9", 7)]
        public static void Day22Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day22.Day22_Part2(Day22.Day22_ReadInput(rawinput)));
        }
    }
}
