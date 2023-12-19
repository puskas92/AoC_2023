using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml.XPath;
using Xunit;

namespace AoC_2023
{
    public static class Day19
    {
        public class Day19_Input //Define input type
        {
            public Dictionary<string, Day19_Workflow> workflows = new Dictionary<string, Day19_Workflow>();
            public List<Day19_PartRating> parts = new List<Day19_PartRating>();

            public bool EvaluateRule(string ruleName, Day19_PartRating partRating)
            {
                foreach (var rule in workflows[ruleName].rules)
                {
                    var outcome = rule.CalculateOutcome(partRating);
                    switch (outcome)
                    {
                        case "A": return true;
                        case "R": return false;
                        case "": continue;
                        default: return EvaluateRule(outcome, partRating);
                    }
                }
                throw new UnreachableException();
            }

            public long EvaluateRuleWithRanges(string ruleName, Dictionary<char, (int lower, int upper)> ranges)
            {
                long result = 0;
                var modifiedMainRange = new Dictionary<char, (int lower, int upper)>(ranges);
                foreach (var rule in workflows[ruleName].rules)
                {
                    if(rule.operation == 'n')
                    {
                        if (rule.yesOutcome == "A") result += CalculateNumberOfAccepted(modifiedMainRange);
                        else if (rule.yesOutcome == "R") continue;
                        else result += EvaluateRuleWithRanges(rule.yesOutcome, modifiedMainRange);
                        continue;
                    }

                    var modifiedSubRange = new Dictionary<char, (int lower, int upper)>(modifiedMainRange);
                   
                    if(rule.operation == '<')
                    {
                        var upper =  Math.Min(modifiedSubRange[rule.conditionParam].upper, rule.toCompareNum - 1);
                        modifiedSubRange[rule.conditionParam] = (modifiedSubRange[rule.conditionParam].lower, upper);
                        modifiedMainRange[rule.conditionParam] = (upper+1 ,modifiedMainRange[rule.conditionParam].upper);

                    }
                    else
                    {
                        var lower = Math.Max(modifiedSubRange[rule.conditionParam].lower, rule.toCompareNum +1);
                        modifiedSubRange[rule.conditionParam] = (lower, modifiedSubRange[rule.conditionParam].upper);
                        modifiedMainRange[rule.conditionParam] = (modifiedMainRange[rule.conditionParam].lower, lower -1); 
                    }

                    if (rule.yesOutcome == "A") result += CalculateNumberOfAccepted(modifiedSubRange);
                    else if (rule.yesOutcome == "R") continue;
                    else result += EvaluateRuleWithRanges(rule.yesOutcome, modifiedSubRange);
                }

                return result;
            }

            private static long CalculateNumberOfAccepted(Dictionary<char, (int lower, int upper)> Ranges)
            {
                long result = 1;
                foreach(var range in Ranges)
                {
                    result *= (range.Value.upper - range.Value.lower + 1);
                }
                return result;
            }
        }
        public class Day19_Workflow
        {
            public string name;
            public List<Day19_Rule> rules;
            public Day19_Workflow(string raw)
            {
                name = raw.Split('{')[0];
                rules = raw.Split('{')[1].Trim('}').Split(',').Select(f => new Day19_Rule(f)).ToList();
            }
        }
        public class Day19_Rule
        {
            public char conditionParam;
            public char operation;
            public int toCompareNum;
            public string yesOutcome;

            public Day19_Rule(string raw)
            {
                if (raw.Contains('<'))
                {
                    operation = '<';
                    conditionParam = raw.Split('<')[0].First();
                    toCompareNum = int.Parse(raw.Split('<')[1].Split(':')[0]);
                    yesOutcome = raw.Split(':')[1];
                }
                else if (raw.Contains('>'))
                {
                    operation = '>';
                    conditionParam = raw.Split('>')[0].First();
                    toCompareNum = int.Parse(raw.Split('>')[1].Split(':')[0]);
                    yesOutcome = raw.Split(':')[1];
                }
                else
                {
                    operation = 'n';
                    yesOutcome = raw;
                }
            }

            public string CalculateOutcome(Day19_PartRating partRating)
            {
                if (operation == 'n') return yesOutcome;
                var ToCompare = conditionParam switch
                {
                    'x' => partRating.x,
                    'm' => partRating.m,
                    'a' => partRating.a,
                    's' => partRating.s,
                    _ => 0
                };
                if (operation == '>')
                {
                    return (ToCompare > toCompareNum ? yesOutcome : string.Empty);
                }
                else
                {
                    return (ToCompare < toCompareNum ? yesOutcome : string.Empty);
                }

            }
        }
        public record struct Day19_PartRating(int x, int m, int a, int s)
        {
            public int SumOfRatings()
            {
                return x + m + a + s;
            }
        };
        public static void Day19_Main()
        {
            var input = Day19_ReadInput();
            Console.WriteLine($"Day19 Part1: {Day19_Part1(input)}");
            Console.WriteLine($"Day19 Part2: {Day19_Part2(input)}");
        }

        public static Day19_Input Day19_ReadInput(string rawinput = "")
        {
            if (rawinput == "")
            {
                rawinput = new StreamReader("Day19\\Day19_input.txt").ReadToEnd();
            }

            var result = new Day19_Input();

            var isWorkflows = true;
            foreach (string line in rawinput.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim()))
            {
                if (line == "")
                {
                    isWorkflows = false;
                    continue;
                }

                if (isWorkflows)
                {
                    var workflow = new Day19_Workflow(line);
                    result.workflows.Add(workflow.name, workflow);
                }
                else
                {
                    var splitted = line.Trim('{').Trim('}').Split(',');
                    var x = int.Parse(splitted[0].Split('=')[1]);
                    var m = int.Parse(splitted[1].Split('=')[1]);
                    var a = int.Parse(splitted[2].Split('=')[1]);
                    var s = int.Parse(splitted[3].Split('=')[1]);
                    result.parts.Add(new Day19_PartRating(x, m, a, s));
                }

            }

            return result;
        }


        public static long Day19_Part1(Day19_Input input)
        {
            long result = 0;
            foreach (var part in input.parts)
            {
                var ruleResult = input.EvaluateRule("in", part);
                result += (ruleResult ? part.SumOfRatings() : 0);
            }
            return result;
        }

        public static long Day19_Part2(Day19_Input input) //works but very slow
        {
            var Ranges = new Dictionary<char, (int lower, int upper)>() {
                { 'x', (1, 4000)},
                { 'm', (1, 4000)},
                { 'a', (1, 4000)},
                { 's', (1, 4000)}};

             long result = input.EvaluateRuleWithRanges("in", Ranges);
            return result;
        }


    }
    public class Day19_Test
    {
        [Theory]
        [InlineData("px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}", 19114)]
        public static void Day19Part1Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day19.Day19_Part1(Day19.Day19_ReadInput(rawinput)));
        }

        [Theory]
        [InlineData("px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}", 167409079868000)]
        public static void Day19Part2Test(string rawinput, long expectedValue)
        {
            Assert.Equal(expectedValue, Day19.Day19_Part2(Day19.Day19_ReadInput(rawinput)));
        }
    }
}
