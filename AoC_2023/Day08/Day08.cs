using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day08
    {
        public class Day08_Input //Define input type
        {
            public List<char> Instructions;

            public Dictionary<string, Day08_MapNode> Map;
        }

        public class Day08_MapNode
        {
            public string name;

            public Day08_MapNode left;

            public Day08_MapNode right;

            public Day08_MapNode()
            {
                name = string.Empty;
                left = this;
                right = this;
            }
        }
        public static void Day08_Main()
        {
            var input = Day08_ReadInput();
            Console.WriteLine($"Day08 Part1: {Day08_Part1(input)}");
            Console.WriteLine($"Day08 Part2: {Day08_Part2(input)}");
        }

        public static Day08_Input Day08_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day08\\Day08_input.txt").ReadToEnd();
            }

            var result = new Day08_Input();

            var lines = rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToList();
            result.Instructions = lines.First().ToList();

            var nodes = new Dictionary<string, (string left, string right)>();

            for(var i = 2; i<lines.Count(); i++)
            {
                var name = lines[i].Split('=')[0].Trim();
                var left = lines[i].Split('=')[1].Split(',')[0].Trim().Trim('(');
                var right = lines[i].Split('=')[1].Split(',')[1].Trim().Trim(')');
                nodes.Add(name, new(left, right));
                //result.Add(line);
            }

            result.Map = new Dictionary<string, Day08_MapNode>();
            foreach(var node in nodes)
            {
                result.Map.Add(node.Key, new Day08_MapNode());
                result.Map[node.Key].name = node.Key;
            }

            foreach (var node in nodes)
            {
                result.Map[node.Key].left = result.Map[node.Value.left];
                result.Map[node.Key].right = result.Map[node.Value.right];
            }

            return result;
        }


        public static int Day08_Part1(Day08_Input input)
        {
            return Day08_IterateOnMapUntilZ(input, input.Map["AAA"], false);
        }

        private static int Day08_IterateOnMapUntilZ(Day08_Input input, Day08_MapNode currentPosNode, bool part2)
        {
            var currentSteps = 0;
            var instLength = input.Instructions.Count();
            while ((!part2 && (currentPosNode.name != "ZZZ")) || (part2 && (!currentPosNode.name.EndsWith('Z'))))
            {
                var currentInstruction = input.Instructions[currentSteps % instLength];
                currentSteps++;
                if (currentInstruction == 'L')
                {
                    currentPosNode = currentPosNode.left;
                }
                else
                {
                    currentPosNode = currentPosNode.right;
                }

            }
            return currentSteps;
        }

        public static long Day08_Part2(Day08_Input input)
        {
            var nodesEndingWithA = input.Map.Where(f => f.Key.EndsWith('A'));
            var resultNumbers = new List<int>();
            foreach (var node in nodesEndingWithA)
            {
                resultNumbers.Add(Day08_IterateOnMapUntilZ(input, node.Value, true));
            }

            long result = 1;
            for(var i = 0; i<resultNumbers.Count; i++)
            {
                result = CommonFunctions.lcm(result, resultNumbers[i]);
            }

            return result;
        }



    }
    public class Day08_Test
    {
        [Theory]
        [InlineData("RL\r\n\r\nAAA = (BBB, CCC)\r\nBBB = (DDD, EEE)\r\nCCC = (ZZZ, GGG)\r\nDDD = (DDD, DDD)\r\nEEE = (EEE, EEE)\r\nGGG = (GGG, GGG)\r\nZZZ = (ZZZ, ZZZ)", 2)]
        [InlineData("LLR\r\n\r\nAAA = (BBB, BBB)\r\nBBB = (AAA, ZZZ)\r\nZZZ = (ZZZ, ZZZ)",6 )]
        public static void Day08Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part1(Day08.Day08_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)", 6)]
        public static void Day08Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day08.Day08_Part2(Day08.Day08_ReadInput(rawinput)));
        }
    }
}
