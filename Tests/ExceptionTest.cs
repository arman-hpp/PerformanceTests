using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTests.Tests
{
	public class ExceptionTest : PerformanceTest
	{
		// constants
		private const int DefaultIterations = 100;
		private const int ListSize = 1000;
		private const int NumberSize = 5;

		// fields
		private readonly char[] _digitArray = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'X' };
		private readonly List<string> _numbers = new();

		public ExceptionTest() : base("Exceptions", "A:Parse, B:TryParse", DefaultIterations)
		{
			var random = new Random();
			for (var i = 0; i < ListSize; i++)
			{
				var sb = new StringBuilder();
				for (var d = 0; d < NumberSize; d++)
				{
					var index = random.Next(_digitArray.Length);
					sb.Append(_digitArray[index]);
				}
				_numbers.Add(sb.ToString());
			}
		}

		protected override bool MeasureTestA()
		{
			// parse numbers using Parse
			for (var i = 0; i < Iterations; i++)
			{
				for (var j = 0; j < ListSize; j++)
				{
					try
					{
						var _ =int.Parse(_numbers[j]);
					}
					catch (FormatException)
					{

					}
				}
			}
			return true;
		}

		protected override bool MeasureTestB()
		{
			// parse numbers using TryParse
			for (var i = 0; i < Iterations; i++)
			{
				for (var j = 0; j < ListSize; j++)
				{
					var _ = int.TryParse(_numbers[j], out var _);
                }
			}
			return true;
		}
    }
}
