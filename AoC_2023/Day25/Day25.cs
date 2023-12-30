using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            //var ToCheckQueue = new Queue<(string currentPos, List<string> visited, int length)>();
            //ToCheckQueue.Enqueue((input.First().Key, new List<string>(), 0));
            //var resultList = new List<(List<string> visited, int length)>();
            //while (ToCheckQueue.Count > 0)
            //{
            //    var ToCheck = ToCheckQueue.Dequeue();
            //    if (input[ToCheck.currentPos].Count == 0)
            //    {
            //        resultList.Add((new List<string>(ToCheck.visited), ToCheck.length));
            //        continue;
            //    }
            //    foreach (var dest in input[ToCheck.currentPos])
            //    {
            //        if (ToCheck.visited.Contains(dest))
            //        {
            //            resultList.Add((new List<string>(ToCheck.visited), ToCheck.length));
            //            continue;
            //        }
            //        var newvisitied = new List<string>(ToCheck.visited);
            //        newvisitied.Add(dest);
            //        ToCheckQueue.Enqueue((dest, newvisitied, ToCheck.length + 1));
            //    }
            //}

            //var maxLength = resultList.Max(f => f.length);
            //var numberOfUsage = new Dictionary<string, int>();
            ////foreach (var line in input)
            ////{
            ////    numberOfUsage.Add(line.Key, 0);
            ////}
            //foreach (var resultline in resultList.Where(f=> f.length == maxLength))
            //{
            //    //foreach(var part in resultline.visited)
            //    //{
            //    //    numberOfUsage[part]++;
            //    //}
            //    for (var i = 0; i < resultline.visited.Count - 1; i++)
            //    {
            //        var key = resultline.visited[i] + ',' + resultline.visited[i+1];
            //        if(!numberOfUsage.ContainsKey(key)) numberOfUsage.Add(key,0);
            //        numberOfUsage[key]++;
            //    }
            //}
            //var OrderedNumberOfUsage = numberOfUsage.OrderBy(f=> f.Value).ToList();
            //return 0;

            Day25_Input contractedGraph;
            do
            {
                contractedGraph = new Day25_Input();
                foreach (var node in input)
                {
                    contractedGraph.Add(node.Key, new List<string>(node.Value));
                }

                contractedGraph = fastMinCut(contractedGraph);
            } while (contractedGraph.First().Value.Count > 3);

            var first = contractedGraph.First().Key.Split(',');
            var second = contractedGraph.Last().Key.Split(',');

            return first.Count() * second.Count();
        }

        private static Day25_Input fastMinCut(Day25_Input graph)
        {
            if(graph.Count <= 6)
            {
                return contract(graph, 2);
            }
            else
            {
                var t = (1 + (Math.Abs(graph.Count) / Math.Sqrt(2)));

                var graph1 = new Day25_Input();
                foreach (var node in graph)
                {
                    graph1.Add(node.Key, new List<string>(node.Value));
                }
                graph1 =  contract(graph1, t);

                var graph2 = new Day25_Input();
                foreach (var node in graph)
                {
                    graph2.Add(node.Key, new List<string>(node.Value));
                }
                graph2 = contract(graph2, t);

                graph1 = fastMinCut(graph1);
                graph2 = fastMinCut(graph2);

                return (graph1.First().Value.Count <= graph2.First().Value.Count) ? graph1 : graph2;
            }
        }
        private static Day25_Input contract(Day25_Input graph, double untilSize)
        {


            while (graph.Count > untilSize)
            {
                var listOfEdges = new List<(string, string)>(); //will contain all edges twice, with both direction, but this should not be a problem
                foreach (var node in graph)
                {
                    foreach (var con in node.Value)
                    {
                        listOfEdges.Add((node.Key, con));
                    }
                }

                //randomly select an edge
                var rnd = Random.Shared.Next(listOfEdges.Count);
                var removeEdge = listOfEdges[rnd];

                //create new contracted edge
                var newEdge = removeEdge.Item1 + ',' + removeEdge.Item2;
                graph.Add(newEdge, new List<string>());
                foreach (var node in graph[removeEdge.Item1])
                {
                    graph[newEdge].Add(node);
                }
                foreach (var node in graph[removeEdge.Item2])
                {
                    graph[newEdge].Add(node);
                }
                //delete old edge
                var result = graph.Remove(removeEdge.Item1);
                Debug.Assert(result);
                result = graph.Remove(removeEdge.Item2);
                Debug.Assert(result);

                //update all the other nodes with newedge
                foreach (var node in graph)
                {
                    for (var i = 0; i < node.Value.Count; i++)
                    {
                        if (node.Value[i] == removeEdge.Item1) node.Value[i] = newEdge;
                        if (node.Value[i] == removeEdge.Item2) node.Value[i] = newEdge;
                    }
                    //remove self edge
                    node.Value.RemoveAll(f => f == node.Key);
                }

            }

            return graph;
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
