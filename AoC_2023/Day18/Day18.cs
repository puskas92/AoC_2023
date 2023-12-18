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
    public static class Day18
    {
        public class Day18_Input : List<Day18_Command> //Define input type
        {
        }
        public class Day18_Command
        {
            public (int X, int Y) direction;
            public int length;
            public Color color;

            public Day18_Command(string raw)
            {
                var splitted = raw.Split(' ');
                var dirChar = splitted[0];
                switch (dirChar.First())
                {
                    case 'U':
                        direction = new(-1, 0); break;
                    case 'D':
                        direction = new(1, 0); break;
                    case 'L':
                        direction = new(0, -1); break;
                    case 'R':
                        direction = new(0, 1); break;
                }
                    
                    
                    length = int.Parse(splitted[1]);
                int colorNum = Convert.ToInt32(splitted[2].Trim('(').Trim(')').Trim('#'), 16);
                color = Color.FromArgb(colorNum);
            }
        }
        public static void Day18_Main()
        {
            var input = Day18_ReadInput();
            Console.WriteLine($"Day18 Part1: {Day18_Part1(input)}");
            Console.WriteLine($"Day18 Part2: {Day18_Part2(input)}");
        }

        public static Day18_Input Day18_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day18\\Day18_input.txt").ReadToEnd();
            }

            var result = new Day18_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                result.Add(new Day18_Command(line));
            }

            return result;
        }


        public static int Day18_Part1(Day18_Input input)
        {
            (int X, int Y) pos = new(0, 0);

            //var diggedColumnsInRow = new Dictionary<int, List<(int column, Color color)>>();
            //diggedColumnsInRow.Add(0, new List<(int, Color)>());


            //foreach (var command in input)
            //{
            //    if(command.direction.X == 0)
            //    {
            //        if (!diggedColumnsInRow.ContainsKey(pos.X)) diggedColumnsInRow.Add(pos.X, new List<(int,Color)>());
            //        diggedColumnsInRow[pos.X].Add(new (pos.Y, command.color));
            //        pos = new(pos.X + (command.direction.X * command.length), pos.Y + (command.direction.Y*command.length));
            //        if (!diggedColumnsInRow.ContainsKey(pos.X)) diggedColumnsInRow.Add(pos.X, new List<(int,Color)>());
            //        diggedColumnsInRow[pos.X].Add(new (pos.Y, command.color));
            //    }
            //    else
            //    {
            //        for (var i= 1; i <= command.length; i++)
            //        {
            //            if(i !=0) pos = new(pos.X + command.direction.X, pos.Y + command.direction.Y);
            //            if (i != command.length) {
            //                if (!diggedColumnsInRow.ContainsKey(pos.X)) diggedColumnsInRow.Add(pos.X, new List<(int, Color)>());
            //                diggedColumnsInRow[pos.X].Add(new(pos.Y, command.color));
            //                    }
            //        }
            //    }

            //}

            //var area = 0;
            //foreach (var row in diggedColumnsInRow)
            //{
            //   var orderdRow =  row.Value.OrderBy(f => f.column).ToList();
            //    var outside = true;
            //    var i =0;
            //    for ( i = 0; i < orderdRow.Count - 1; i++)
            //    {
            //        if (orderdRow[i].color == orderdRow[i + 1].color)
            //        {
            //            area += orderdRow[i + 1].column - orderdRow[i].column +1;
            //        }
            //        else
            //        {
            //            if (outside)
            //            {
            //                area += orderdRow[i + 1].column - orderdRow[i].column + 1;
            //            }
            //            outside = !outside;
            //        }
            //    }
            //    area -= (i-1);

            //}
            //return area;


            var diggedPoints = new List<(int X, int Y)>();
            diggedPoints.Add(pos);
            foreach (var command in input)
            {
                for (var i = 1; i <= command.length; i++)
                {
                      pos = new(pos.X + command.direction.X, pos.Y + command.direction.Y);
                      diggedPoints.Add(pos);
                }
            }

            diggedPoints = diggedPoints.Distinct().ToList();
            var ToCheckList = new Queue<(int X, int Y)>();
            ToCheckList.Enqueue((1, 1));
            while (ToCheckList.Count > 0)
            {
                var ToCheck = ToCheckList.Dequeue();
                if (diggedPoints.Contains(ToCheck)) continue;
                diggedPoints.Add(ToCheck);
                foreach(var dir in CommonFunctions.CrossDirections)
                {
                    (int X, int Y) nextPos = (ToCheck.X + dir.X, ToCheck.Y + dir.Y);
                    if (diggedPoints.Contains(nextPos)) continue;
                    ToCheckList.Enqueue(nextPos);
                }
            }

            return diggedPoints.Count;
        }

        public static int Day18_Part2(Day18_Input input)
        {
            return 0;
        }


    }
    public class Day18_Test
    {
        [Theory]
        [InlineData("R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", 62)]
        public static void Day18Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part1(Day18.Day18_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", 952408144115)]
        public static void Day18Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day18.Day18_Part2(Day18.Day18_ReadInput(rawinput)));
        }
    }
}
