namespace PerformanceTests.Tests
{
	public class MemoryTest : PerformanceTest
	{
		private const int DefaultIterations = 500;
		private const int BufferSize = 1_000_000;

		private readonly byte[] _buffer1;
		private readonly byte[] _buffer2;

		public MemoryTest() : base("Byte array copy", "A:direct, B:with pointers, C:CopyTo", DefaultIterations)
		{
			_buffer1 = new byte[BufferSize];

			_buffer2 = new byte[BufferSize];
		}

		protected override bool MeasureTestA()
		{
			// copy buffer using a simple loop
			for (var i = 0; i < Iterations; i++)
			{
				for (var j = 0; j < BufferSize; j++)
				{
					_buffer2[j] = _buffer1[j];
				}
			}
			return true;
		}

		protected override unsafe bool MeasureTestB()
		{
			// copy buffer using pointers
			fixed (byte* fixed1 = &_buffer1[0])
			fixed (byte* fixed2 = &_buffer2[0])
			{
				for (var i = 0; i < Iterations; i++)
				{
					var source = fixed1;
					var dest = fixed2;
					for (var j = 0; j < BufferSize; j++)
					{
						*(dest++) = *(source++);
					}
				}
			}
			return true;
		}

		protected override bool MeasureTestC()
		{
			// copy buffer using the CopyTo method
			for (var i = 0; i < Iterations; i++)
			{
				_buffer1.CopyTo(_buffer2, 0);
			}
			return true;
		}

	}
}
