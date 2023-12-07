global using Xunit;
using AoC_2023;
using System.Diagnostics;

Console.WriteLine("AoC 2023");
var sw = new Stopwatch();
sw.Start();
Day01.Day01_Main();
Day02.Day02_Main();
Day03.Day03_Main();
Day04.Day04_Main();
Day05.Day05_Main();
Day06.Day06_Main();
Day07.Day07_Main();
Day08.Day08_Main();
sw.Stop();

Console.WriteLine($"Code run under {sw.ElapsedMilliseconds}ms");
Console.ReadLine();