using System.Text;

namespace PerformanceTests.Tests
{
    public class StringsTest : PerformanceTest
    {
        private const int DefaultIterations = 50_000;

        public StringsTest() : base("Strings", "A:string, B:StringBuilder, C:char pointer", DefaultIterations)
        {
        }

        protected override bool MeasureTestA()
        {
            // string additions using regular string type
            var result = string.Empty;
            for (var i = 0; i < Iterations; i++)
            {
                result = result + '*';
            }
            return true;
        }

        protected override bool MeasureTestB()
        {
            // string additions using StringBuilder
            var result = new StringBuilder();
            for (var i = 0; i < Iterations; i++)
            {
                result.Append('*');
            }
            return true;
        }

        protected override unsafe bool MeasureTestC()
        {
            // fill string by using pointer operations
            var result = new char[Iterations];
            fixed (char* fixedPointer = result)
            {
                var pointer = fixedPointer;
                for (var i = 0; i < Iterations; i++)
                {
                    *(pointer++) = '*';
                }
            }
            return true;
        }
    }
}
