using System;
using System.Collections.Generic;

namespace PerformanceTests.Tests
{
	public class ForForeachTest : PerformanceTest
	{
		private const int DefaultIterations = 100;
		private const int ListSize = 1_000_000;

		private readonly List<int> _list;

		public ForForeachTest() : base("For/Foreach", "A:foreach, B:for", DefaultIterations)
		{
			var random = new Random();
			_list = new List<int>(ListSize);
			for (var i = 0; i < ListSize; i++)
			{
				var number = random.Next(256);
				_list.Add(number);
			}
		}

		protected override bool MeasureTestA()
		{
			// walk through the array using a foreach loop
			for (var i = 0; i < Iterations; i++)
			{
				foreach (var _ in _list)
				{
					// do something with number
				}
			}
			return true;
		}

		protected override bool MeasureTestB()
		{
			// walk through the array using a for loop
			for (var i = 0; i < Iterations; i++)
			{
				for (var j = 0; j < ListSize; j++)
				{
					var _ = _list[j];
				}
			}
			return true;
		}
    }
}
