global using Xunit;
using AoC_2023;
using System.Diagnostics;

Console.WriteLine("AoC 2023");
var sw = new Stopwatch();
sw.Start();
//Day01.Day01_Main();
//Day02.Day02_Main();
//Day03.Day03_Main();
//Day04.Day04_Main();
//Day05.Day05_Main();
//Day06.Day06_Main();
//Day07.Day07_Main();
//Day08.Day08_Main();
//Day09.Day09_Main();
//Day10.Day10_Main(); // could be faster
//Day11.Day11_Main(); // could be faster
//Day12.Day12_Main(); // need to solve
//Day13.Day13_Main();
//Day14.Day14_Main(); //could be faster
//Day15.Day15_Main();
//Day16.Day16_Main(); //could be faster
//Day17.Day17_Main();
//Day18.Day18_Main();
//Day19.Day19_Main();
//Day20.Day20_Main();
//Day21.Day21_Main();
//Day22.Day22_Main(); //could be faster, but it works in reasonable time (~80s)
//Day23.Day23_Main(); //Part 2 could be faster
Day24.Day24_Main();
sw.Stop();

Console.WriteLine($"Code run under {sw.ElapsedMilliseconds}ms");
Console.ReadLine();