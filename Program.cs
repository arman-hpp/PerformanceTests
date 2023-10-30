using System;
using System.Collections.Generic;
using PerformanceTests.Tests;

namespace PerformanceTests
{
    internal class Program
    {
        private static readonly List<IPerformanceTest> Tests = new();

        private static void ShowAllTests()
        {
            Console.WriteLine("Available tests:");
            for (var i = 0; i < Tests.Count; i++)
            {
                Console.WriteLine($"{i}: {Tests[i].Name}");
            }
            Console.WriteLine();
        }

        private static IPerformanceTest AskForTest()
        {
            Console.Write("Please select a test: ");
            if (!int.TryParse(Console.ReadLine(), out var index))
                return null;
            else
                return index >= 0 && index < Tests.Count ? Tests[index] : null;
        }

        private static void ConfigureTest(IPerformanceTest test)
        {
            Console.Write($"How many iterations ({test.Iterations}): ");
            if (int.TryParse(Console.ReadLine(), out var iterations))
            {
                test.Iterations = iterations;
            }
            Console.Write("Run baseline test? (yes): ");
            var input = Console.ReadLine();
            test.RunBaseline = (string.IsNullOrEmpty(input) || input.ToLower().StartsWith("y"));
        }

        private static void ShowGraph((int, int, int, int) result)
        {
            const int numStars = 50;

            // normalize results
            var max = Math.Max(result.Item1, Math.Max(result.Item2, result.Item3));
            var barA = max > 0 ? result.Item1 * numStars / max : 0;
            var barB = max > 0 ? result.Item2 * numStars / max : 0;
            var barC = max > 0 ? result.Item3 * numStars / max : 0;
            var barD = max > 0 ? result.Item4 * numStars / max : 0;

            // show bar graph
            Console.WriteLine($"A |{new string('\x25A0', barA)}");
            Console.WriteLine($"B |{new string('\x25A0', barB)}");
            Console.WriteLine($"C |{new string('\x25A0', barC)}");
            Console.WriteLine($"D |{new string('\x25A0', barD)}");
            Console.WriteLine("  +--------------------------------------------------");
            Console.WriteLine($"    A: {result.Item1}ms, B: {result.Item2}ms, C: {result.Item3}ms, D: {result.Item4}ms");
            Console.WriteLine();
        }

        private static void Main()
        {
            // initialize test list
            Tests.Add(new ExceptionTest());
            Tests.Add(new StringsTest());
            Tests.Add(new ArraysTest());
            Tests.Add(new ForForeachTest());
            Tests.Add(new StructsTest());
            Tests.Add(new MemoryTest());
            Tests.Add(new InstantiationTest());
            Tests.Add(new PropertiesTest());

            while (true)
            {
                // show test menu
                ShowAllTests();

                // get test
                var test = AskForTest();
                if (test == null)
                    return;

                // configure test
                ConfigureTest(test);

                // run test
                Console.WriteLine();
                Console.WriteLine($"Running '{test.Name}' with {test.Iterations} iterations...");
                Console.WriteLine(test.Description);
                var result = test.Measure();
                Console.WriteLine();

                // show results
                ShowGraph(result);
            }
        }
    }
}
