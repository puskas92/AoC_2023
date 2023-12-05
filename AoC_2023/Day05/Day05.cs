using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day05
    {
        public class Day05_Input
        {
            public List<long> seeds = new List<long>();

            public Dictionary<string, Day05_Map> maps = new Dictionary<string, Day05_Map>();
        }
        public class Day05_Map
        {
            public string FromType;
            public string ToType;
            public List<(long destStart, long sourceStart, long length)> convertRanges;

            public Day05_Map(string from, string to)
            {
                FromType = from;
                ToType = to;
                convertRanges = new List<(long destStart, long sourceStart, long length)>();
            }
        }
        public static void Day05_Main()
        {
            var input = Day05_ReadInput();
            Console.WriteLine($"Day05 Part1: {Day05_Part1(input)}");
            Console.WriteLine($"Day05 Part2: {Day05_Part2(input)}");
        }

        public static Day05_Input Day05_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day05\\Day05_input.txt").ReadToEnd();
            }

            var result = new Day05_Input();

            var lines = rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()).ToList();
          
            result.seeds = lines.First().Split(':')[1].Trim().Split(' ').Where(f => f != "").Select(f => long.Parse(f)).ToList();

            var sourcetype = "";
            for(int i = 1; i< lines.Count; i++)
            {
                if (lines[i].Trim() == "")
                {
                    sourcetype = "";
                    continue;
                }
                if (sourcetype == "")
                {
                    sourcetype = lines[i].Split(' ')[0].Split('-')[0].Trim();
                    var destType = lines[i].Split(' ')[0].Split('-')[2].Trim();
                    result.maps.Add(sourcetype, new Day05_Map(sourcetype, destType));
                }
                else
                {
                    var numbers = lines[i].Trim().Split(' ').Where(f => f != "").Select(f => long.Parse(f)).ToList();
                    result.maps[sourcetype].convertRanges.Add(new(numbers[0], numbers[1], numbers[2]));
                }
            }

            return result;
        }


        public static long Day05_Part1(Day05_Input input)
        {
            var ConvertedSeeds = new List<long>();
            foreach (var seed in input.seeds)
            {
                var currentValue = seed;
                var currentType = "seed";
                do
                {
                    var ranges = input.maps[currentType].convertRanges;
                    foreach(var range in ranges)
                    {
                        if((currentValue >= range.sourceStart) && (currentValue <= (range.sourceStart + (range.length - 1))))
                        {
                            currentValue = range.destStart + (currentValue - range.sourceStart);
                            break;
                        }
                    }
                    currentType = input.maps[currentType].ToType;
                } while (currentType != "location");
                ConvertedSeeds.Add(currentValue);
            }


            return ConvertedSeeds.Min();
        }

        public static long Day05_Part2(Day05_Input input)
        {
            var minLocation = long.MaxValue;
            for (var i = 0; i < input.seeds.Count; i += 2)
            {
                var start = input.seeds[i];
                var end = start + input.seeds[i + 1] - 1;
                minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, start, end , "seed"));
            }
            return minLocation;
        }

        private static long Day05_RecursiveMinLocationMapOfRange(Day05_Input input, long currentRangeLow, long currentRangeHigh, string currentType)
        {
            if (currentType == "location") return currentRangeLow;
            var ranges = input.maps[currentType].convertRanges;
            var nextType = input.maps[currentType].ToType;
            var minLocation = long.MaxValue;

            foreach (var range in ranges)
            {
                var sourceLow = range.sourceStart;
                var sourceHigh = range.sourceStart + range.length - 1;

                if(currentRangeLow >= sourceLow && currentRangeHigh <= sourceHigh) //full cover
                {
                   var nextRangeLow = range.destStart + (currentRangeLow - range.sourceStart);
                   var nextRangeHigh= range.destStart + (currentRangeHigh - range.sourceStart);
                   
                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, nextRangeLow, nextRangeHigh, nextType));
                    
                    currentRangeHigh = -1;
                    currentRangeLow = -1;
                    break;
                }
                else if(currentRangeLow < sourceLow && currentRangeHigh > sourceHigh) // middle overlap
                {
                    var nextRangeLow = range.destStart;
                    var nextRangeHigh = range.destStart + range.length - 1;

                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, nextRangeLow, nextRangeHigh, nextType));

                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, currentRangeLow, sourceLow - 1, currentType));
                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, sourceHigh + 1, currentRangeHigh, currentType));

                    currentRangeHigh = -1;
                    currentRangeLow = -1;
                    break;
                }
                else if(currentRangeHigh >= sourceLow && currentRangeLow <= sourceLow) //lower overlap
                {
                    var nextRangeLow = range.destStart;
                    var nextRangeHigh = range.destStart + (currentRangeHigh - range.sourceStart);

                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, nextRangeLow, nextRangeHigh, nextType));

                    currentRangeHigh = sourceLow - 1; 
                }
                else if(currentRangeLow <= sourceHigh && currentRangeHigh >= sourceHigh) //upper overlap
                {
                    var nextRangeLow = range.destStart + (currentRangeLow - range.sourceStart);
                    var nextRangeHigh = range.destStart + range.length - 1;

                    minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, nextRangeLow, nextRangeHigh, nextType));

                    currentRangeLow = sourceHigh + 1;
                }
            }

            //no overlap remains
            if (currentRangeLow != -1 && currentRangeHigh != -1 && currentRangeHigh >= currentRangeLow)
            {
                minLocation = Math.Min(minLocation, Day05_RecursiveMinLocationMapOfRange(input, currentRangeLow, currentRangeHigh, nextType));
            }

            return minLocation;
        }
    }
    public class Day05_Test
    {
        [Theory]
        [InlineData("seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4", 35)]
        public static void Day05Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part1(Day05.Day05_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4", 46)]
        public static void Day05Part2Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day05.Day05_Part2(Day05.Day05_ReadInput(rawinput)));
        }
    }
}
