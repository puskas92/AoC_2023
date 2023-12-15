using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day15
    {
        public class Day15_Input : List<Day15_Command> //Define input type
        {
        }
        public class Day15_Command
        {
            public string value;
            public byte fullhash;
            public byte labelhash;
            public char command;
            public string label;
            public int focalLength;

            public Day15_Command(string value  )
            {
                this.value = value;
                int inthash = 0;
                foreach (var character in value)
                {
                    inthash += character;
                    inthash *= 17;
                    inthash = inthash % 256;
                }
                fullhash = (byte)inthash;

                if (value.Contains('='))
                {
                    var split = value.Split('=');
                    label = split[0];
                    focalLength = int.Parse(split[1]);
                    command = '=';
                }
                else
                {
                    var split = value.Split('-');
                    label = split[0];
                    focalLength = 0;
                    command = '-';
                }

                inthash = 0;
                foreach (var character in label)
                {
                    inthash += character;
                    inthash *= 17;
                    inthash = inthash % 256;
                }
                labelhash = (byte)inthash;
            }
        }
        public static void Day15_Main()
        {
            var input = Day15_ReadInput();
            Console.WriteLine($"Day15 Part1: {Day15_Part1(input)}");
            Console.WriteLine($"Day15 Part2: {Day15_Part2(input)}");
        }

        public static Day15_Input Day15_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day15\\Day15_input.txt").ReadToEnd();
            }

            var result = new Day15_Input();

           
            foreach (string part in rawinput.Split(',').Select(s => s.Trim()))
            {
                result.Add(new Day15_Command( part));
            }

            return result;
        }


        public static int Day15_Part1(Day15_Input input)
        {

            return input.Sum(f => f.fullhash) ;
        }

        public static int Day15_Part2(Day15_Input input)
        {
            var boxes = new Dictionary<int, LinkedList<string>>();
            var focalLengths = new Dictionary<string, int>();

            for(var i = 0; i<=255; i++)
            {
                boxes.Add(i, new LinkedList<string>());
            }

            foreach(var step in input)
            {
                var boxNumber = step.labelhash;
                if(step.command == '-')
                {
                    if (boxes[boxNumber].Contains(step.label))
                    {
                        boxes[boxNumber].Remove(step.label);
                    }
                }
                else
                {
                    if (focalLengths.ContainsKey(step.label))
                    {
                        focalLengths[step.label] = step.focalLength;
                    }
                    else
                    {
                        focalLengths.Add(step.label, step.focalLength);
                    }
                    
                    if (boxes[boxNumber].Contains(step.label))
                    {
                       //??
                    }
                    else
                    {
                        boxes[boxNumber].AddLast(step.label);
                    }

                }

            }

            var lenses = new Dictionary<string, int>();
            for(var i = 0; i<=255; i++)
            {
                var j = 1;
                foreach(var lens in boxes[i])
                {
                    if (!lenses.ContainsKey(lens)) lenses.Add(lens, 0);
                    lenses[lens] = (i + 1) * j * focalLengths[lens];
                    j++;
                }
            }

            return lenses.Sum(f=> f.Value);
        }


    }
    public class Day15_Test
    {
        [Theory]
        [InlineData("HASH", 52)]
        [InlineData("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", 1320)]
        public static void Day15Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day15.Day15_Part1(Day15.Day15_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", 145)]
        public static void Day15Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day15.Day15_Part2(Day15.Day15_ReadInput(rawinput)));
        }
    }
}
