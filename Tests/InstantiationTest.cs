using System;
using System.Reflection.Emit;

namespace PerformanceTests.Tests
{
	public class InstantiationTest : PerformanceTest
	{
		private const int DefaultIterations = 1_000_000;

		public delegate object ConstructorDelegate();

		public InstantiationTest() : base("Instantiation", "A:reflection, B:dynamic CIL, C:compile-time", DefaultIterations)
		{
		}

		protected ConstructorDelegate GetConstructor(string typeName)
		{
			// get the default constructor of the type
			var t = Type.GetType(typeName);
            if (t == null) return null;
            
            var ctor = t.GetConstructor(Type.EmptyTypes);
			if(ctor == null) return null;

            // create a new dynamic method that constructs and returns the type
            var methodName = t.Name + "Ctor";
            var dm = new DynamicMethod(methodName, t, Type.EmptyTypes, typeof(Activator));
            var ilGenerator = dm.GetILGenerator();
            ilGenerator.Emit(OpCodes.Newobj, ctor);
            ilGenerator.Emit(OpCodes.Ret);

            // add delegate to dictionary and return
            var creator = (ConstructorDelegate)dm.CreateDelegate(typeof(ConstructorDelegate));

            // return a delegate to the method
            return creator;
        }

		protected override bool MeasureTestA()
		{
			// instantiate StringBuilder using reflection
			var type = Type.GetType("System.Text.StringBuilder");
			for (var i = 0; i < Iterations; i++)
            {
                if (type == null) continue;

                var obj = Activator.CreateInstance(type);
                if (obj != null && obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("Constructed object is not a StringBuilder");
            }
			return true;
		}

        protected override bool MeasureTestB()
        {
            // instantiate StringBuilder using reflection
            var type = typeof(System.Text.StringBuilder);
            for (var i = 0; i < Iterations; i++)
            {
                var obj = Activator.CreateInstance(type);
                if (obj != null && obj.GetType() != typeof(System.Text.StringBuilder))
                    throw new InvalidOperationException("Constructed object is not a StringBuilder");
            }
            return true;
        }

        protected override bool MeasureTestC()
		{
            // instantiate StringBuilder using dynamic CIL
            var constructor = GetConstructor("System.Text.StringBuilder");
			for (var i = 0; i < Iterations; i++)
			{
				var obj = constructor();
				if (obj.GetType() != typeof(System.Text.StringBuilder))
					throw new InvalidOperationException("Constructed object is not a StringBuilder");
			}
			return true;
		}

		protected override bool MeasureTestD()
		{
            // instantiate StringBuilder directly
            for (var i = 0; i < Iterations; i++)
			{
				var obj = new System.Text.StringBuilder();
				if (obj.GetType() != typeof(System.Text.StringBuilder))
					throw new InvalidOperationException("Constructed object is not a StringBuilder");
			}
			return true;
		}

	}
}
