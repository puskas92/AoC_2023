using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day25
    {
        public class Day25_Input : Dictionary<string, List<string>> //Define input type
        {
        }
        public static void Day25_Main()
        {
            var input = Day25_ReadInput();
            Console.WriteLine($"Day25 Part1: {Day25_Part1(input)}");
        }

        public static Day25_Input Day25_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day25\\Day25_input.txt").ReadToEnd();
            }

            var result = new Day25_Input();

            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                var from = line.Split(':')[0].Trim();
                var tos = line.Split(':')[1].Trim().Split(' ').ToList();
                if(!result.ContainsKey(from)) result.Add(from, new List<string>());
                foreach(var to in tos)
                {
                    result[from].Add(to);
                    if (!result.ContainsKey(to)) result.Add(to, new List<string>());
                    result[to].Add(from);
                }
            }

            return result;
        }


        public static int Day25_Part1(Day25_Input input)
        {
            var ToCheckQueue = new Queue<(string currentPos, List<string> visited, int length)>();
            ToCheckQueue.Enqueue((input.First().Key, new List<string>(), 0));
            var resultList = new List<(List<string> visited, int length)>();
            while (ToCheckQueue.Count > 0)
            {
                var ToCheck = ToCheckQueue.Dequeue();
                if (input[ToCheck.currentPos].Count == 0)
                {
                    resultList.Add((new List<string>(ToCheck.visited), ToCheck.length));
                    continue;
                }
                foreach (var dest in input[ToCheck.currentPos])
                {
                    if (ToCheck.visited.Contains(dest))
                    {
                        resultList.Add((new List<string>(ToCheck.visited), ToCheck.length));
                        continue;
                    }
                    var newvisitied = new List<string>(ToCheck.visited);
                    newvisitied.Add(dest);
                    ToCheckQueue.Enqueue((dest, newvisitied, ToCheck.length + 1));
                }
            }

            var maxLength = resultList.Max(f => f.length);
            var numberOfUsage = new Dictionary<string, int>();
            //foreach (var line in input)
            //{
            //    numberOfUsage.Add(line.Key, 0);
            //}
            foreach (var resultline in resultList.Where(f=> f.length == maxLength))
            {
                //foreach(var part in resultline.visited)
                //{
                //    numberOfUsage[part]++;
                //}
                for (var i = 0; i < resultline.visited.Count - 1; i++)
                {
                    var key = resultline.visited[i] + ',' + resultline.visited[i+1];
                    if(!numberOfUsage.ContainsKey(key)) numberOfUsage.Add(key,0);
                    numberOfUsage[key]++;
                }
            }
            var OrderedNumberOfUsage = numberOfUsage.OrderBy(f=> f.Value).ToList();
            return 0;
        }



    }
    public class Day25_Test
    {
        [Theory]
        [InlineData("jqt: rhn xhk nvd\r\nrsh: frs pzl lsr\r\nxhk: hfx\r\ncmg: qnr nvd lhk bvb\r\nrhn: xhk bvb hfx\r\nbvb: xhk hfx\r\npzl: lsr hfx nvd\r\nqnr: nvd\r\nntq: jqt hfx bvb xhk\r\nnvd: lhk\r\nlsr: lhk\r\nrzs: qnr cmg lsr rsh\r\nfrs: qnr lhk lsr", 54)]
        public static void Day25Part1Test(string rawinput, int expectedValue)
        {
            Assert.Equal(expectedValue, Day25.Day25_Part1(Day25.Day25_ReadInput(rawinput)));
        }
    }
}
