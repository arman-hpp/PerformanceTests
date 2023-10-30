namespace PerformanceTests.Tests
{
	public class PointClass
	{
		public int X { get; set; }
		public int Y { get; set; }
		public PointClass(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class PointClassFinalized : PointClass
	{
		public PointClassFinalized(int x, int y) : base(x, y)
		{
		}
    }

	public struct PointStruct
	{
		public int X { get; set; }
		public int Y { get; set; }
		public PointStruct(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	public class StructsTest : PerformanceTest
	{
		private const int DefaultIterations = 1_000_000;

		public StructsTest() : base("Structs", "A:finalized class, B:class, C:struct", DefaultIterations)
		{
		}

		protected override bool MeasureTestA()
		{
			// access array elements
			var list = new PointClassFinalized[Iterations];
			for (var i = 0; i < Iterations; i++)
			{
				list[i] = new PointClassFinalized(i, i);
			}
			return true;
		}

		protected override bool MeasureTestB()
		{
			// access array elements
			var list = new PointClass[Iterations];
			for (var i = 0; i < Iterations; i++)
			{
				list[i] = new PointClass(i, i);
			}
			return true;
		}

		protected override bool MeasureTestC()
		{
			// access array elements
			var list = new PointStruct[Iterations];
			for (var i = 0; i < Iterations; i++)
			{
				list[i] = new PointStruct(i, i);
			}
			return true;
		}
	}
}
