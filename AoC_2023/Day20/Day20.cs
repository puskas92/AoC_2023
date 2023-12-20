using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day20
    {
        public class Day20_Input : Dictionary<string, Day20_Module> //Define input type
        {
        }
        public struct Day20_Module
        {

            public string name;
            public int type; //0 broadcaster, 1 flip-flop, 2-conjuction, 3-output
            public List<string> destinations;
        
            public Day20_Module(string raw)
            {
                var splitted = raw.Split("->")[0].Trim();
                name = splitted.Trim('%').Trim('&');
                if (splitted == "broadcaster") type = 0;
                else if (splitted.Contains("%")) type = 1;
                else if (splitted.Contains("&")) type = 2;
                else type = 3;
                destinations = raw.Split("->")[1].Trim().Split(',').Select(f => f.Trim()).ToList();
            }
        }

        public static void Day20_Main()
        {
            var input = Day20_ReadInput();
            Console.WriteLine($"Day20 Part1: {Day20_Part1(input)}");
            Console.WriteLine($"Day20 Part2: {Day20_Part2(input)}");
        }

        public static Day20_Input Day20_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day20\\Day20_input.txt").ReadToEnd();
            }

            var result = new Day20_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var module = new Day20_Module(line);
                result.Add(module.name, module);
            }

            return result;
        }


        public static long Day20_Part1(Day20_Input input)
        {
            Dictionary<string, bool> FlipFlopModuleState;
            Dictionary<string, Dictionary<string, bool>> ConjuctionModuleState;
            Dictionary<bool, long> PulseCount;
            InitStates(input, out FlipFlopModuleState, out ConjuctionModuleState, out PulseCount);

            for (var i = 1; i <= 1000; i++)
            {
                var instructionList = new Queue<(string from, string dest, bool pulseType)>();
                instructionList.Enqueue(("button", "broadcaster", false));
                PulseCount[false]++;

                while (instructionList.Count > 0)
                {
                    var inst = instructionList.Dequeue();
                    if (!input.ContainsKey(inst.dest)) continue;
                    var module = input[inst.dest];

                    switch (module.type)
                    {
                        case 0: //broadcaster
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, inst.pulseType));
                                PulseCount[inst.pulseType]++;
                            }
                            break;
                        case 1: //flip-flop
                            if (inst.pulseType == true) continue;
                            var newState = !FlipFlopModuleState[inst.dest];
                            FlipFlopModuleState[inst.dest] = newState;
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, newState));
                                PulseCount[newState]++;
                            }
                            break;
                        case 2: //conjuction
                            ConjuctionModuleState[inst.dest][inst.from] = inst.pulseType;
                            var sentPulseType = true;
                            if (ConjuctionModuleState[inst.dest].All(f => f.Value == true)) sentPulseType = false;
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, sentPulseType));
                                PulseCount[sentPulseType]++;
                            }
                            break;
                        default: //output
                            continue;
                    }
                }
            }

            return PulseCount[false] * PulseCount[true];
        }

        private static void InitStates(Day20_Input input, out Dictionary<string, bool> FlipFlopModuleState, out Dictionary<string, Dictionary<string, bool>> ConjuctionModuleState, out Dictionary<bool, long> PulseCount)
        {
            FlipFlopModuleState = new Dictionary<string, bool>();
            ConjuctionModuleState = new Dictionary<string, Dictionary<string, bool>>();
            foreach (var module in input)
            {
                if (module.Value.type == 1) FlipFlopModuleState.Add(module.Key, false);
                if (module.Value.type == 2) ConjuctionModuleState.Add(module.Key, new Dictionary<string, bool>());
            }
            foreach (var module in input)
            {
                foreach (var dest in module.Value.destinations)
                {
                    if (ConjuctionModuleState.ContainsKey(dest)) ConjuctionModuleState[dest].Add(module.Key, false);
                }
            }

            PulseCount = new Dictionary<bool, long>();
            PulseCount.Add(false, 0);
            PulseCount.Add(true, 0);
        }

        public static long Day20_Part2(Day20_Input input)
        {
            Dictionary<string, bool> FlipFlopModuleState;
            Dictionary<string, Dictionary<string, bool>> ConjuctionModuleState;
            Dictionary<bool, long> PulseCount;
            InitStates(input, out FlipFlopModuleState, out ConjuctionModuleState, out PulseCount);

            var rxInput = input.ToList().Find(f => f.Value.destinations.Contains("rx")).Key;
            var cyclesToCheck = new Dictionary<string, long>();
            foreach(var inp in ConjuctionModuleState[rxInput].Keys)
            {
                cyclesToCheck.Add(inp, 0);
            }

            long buttonPress = 0;

            do
            {
                buttonPress++;
                var instructionList = new Queue<(string from, string dest, bool pulseType)>();
                instructionList.Enqueue(("button", "broadcaster", false));
                PulseCount[false]++;

                while (instructionList.Count > 0)
                {
                    var inst = instructionList.Dequeue();
                    if (inst.dest == "rx" && inst.pulseType == false) return buttonPress;
                    if (!input.ContainsKey(inst.dest)) continue;
                    var module = input[inst.dest];

                    switch (module.type)
                    {
                        case 0: //broadcaster
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, inst.pulseType));
                                PulseCount[inst.pulseType]++;
                            }
                            break;
                        case 1: //flip-flop
                            if (inst.pulseType == true) continue;
                            var newState = !FlipFlopModuleState[inst.dest];
                            FlipFlopModuleState[inst.dest] = newState;
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, newState));
                                PulseCount[newState]++;
                            }
                            break;
                        case 2: //conjuction
                            ConjuctionModuleState[inst.dest][inst.from] = inst.pulseType;
                            var sentPulseType = true;

                            if (cyclesToCheck.ContainsKey(inst.dest))
                            {
                                if (ConjuctionModuleState[rxInput].Any(f => f.Value == true))
                                {
                                    foreach (var inp in ConjuctionModuleState[rxInput])
                                    {
                                        if (inp.Value == true)
                                        {
                                            if (cyclesToCheck[inp.Key] == 0) cyclesToCheck[inp.Key] = buttonPress;
                                        }
                                    }
                                }
                            }

                            if (ConjuctionModuleState[inst.dest].All(f => f.Value == true)) sentPulseType = false;
                            foreach (var dest in module.destinations)
                            {
                                instructionList.Enqueue((module.name, dest, sentPulseType));
                                PulseCount[sentPulseType]++;
                            }
                            break;
                        default: //output
                            continue;
                    }
                }
              
            } while (cyclesToCheck.Any(f=> f.Value == 0));

            long result = 1;
            foreach(var inp in cyclesToCheck)
            {
                result = CommonFunctions.lcm(result, inp.Value);
            }

            return result;
        }
    }
    public class Day20_Test
    {
        [Theory]
        [InlineData("broadcaster -> a, b, c\r\n%a -> b\r\n%b -> c\r\n%c -> inv\r\n&inv -> a", 32000000)]
        [InlineData("broadcaster -> a\r\n%a -> inv, con\r\n&inv -> b\r\n%b -> con\r\n&con -> output", 11687500)]
        public static void Day20Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day20.Day20_Part1(Day20.Day20_ReadInput(rawinput)));
        }
    }
}
