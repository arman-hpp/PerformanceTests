using System.Diagnostics;

namespace PerformanceTests
{
    public class PerformanceTest : IPerformanceTest
    {
        private const int DefaultRepetitions = 10;

        public string Name { get; }

        public string Description { get; }

        public int Iterations { get; set; }

        public bool RunBaseline { get; set; }

        protected virtual bool MeasureTestA()
        {
            return false;
        }

        protected virtual bool MeasureTestB()
        {
            return false;
        }

        protected virtual bool MeasureTestC()
        {
            return false;
        }

        protected virtual bool MeasureTestD()
        {
            return false;
        }

        public PerformanceTest(string name, string description, int interactions)
        {
            Name = name;
            Description = description;
            Iterations = interactions;
        }

        public (int, int, int, int) Measure()
        {
            long totalA = 0, totalB = 0, totalC = 0, totalD = 0;

            var stopwatch = new Stopwatch();

            // run baseline tests
            if (RunBaseline)
            {
                for (long i = 0; i < DefaultRepetitions; i++)
                {
                    stopwatch.Restart();
                    var implemented = MeasureTestA();
                    stopwatch.Stop();
                    if (implemented)
                        totalA += stopwatch.ElapsedMilliseconds;
                }
            }

            // run optimized test B
            for (long i = 0; i < DefaultRepetitions; i++)
            {
                stopwatch.Restart();
                var implemented = MeasureTestB();
                stopwatch.Stop();
                if (implemented)
                    totalB += stopwatch.ElapsedMilliseconds;
            }

            // run optimized tests C
            for (long i = 0; i < DefaultRepetitions; i++)
            {
                stopwatch.Restart();
                var implemented = MeasureTestC();
                stopwatch.Stop();
                if (implemented)
                    totalC += stopwatch.ElapsedMilliseconds;
            }

            // run optimized tests D
            for (long i = 0; i < DefaultRepetitions; i++)
            {
                stopwatch.Restart();
                var implemented = MeasureTestD();
                stopwatch.Stop();
                if (implemented)
                    totalD += stopwatch.ElapsedMilliseconds;
            }

            // return results
            return (
                (int)(totalA / DefaultRepetitions),
                (int)(totalB / DefaultRepetitions),
                (int)(totalC / DefaultRepetitions),
                (int)(totalD / DefaultRepetitions));
        }
    }
}
