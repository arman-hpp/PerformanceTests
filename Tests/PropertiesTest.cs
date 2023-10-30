using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace PerformanceTests.Tests
{
	public class PropertiesTest : PerformanceTest
	{
		private const int DefaultIterations = 5_000_000;

		public delegate object PropertyGetDelegate(object obj);

		public delegate void PropertySetDelegate(object obj, object value);

		public PropertiesTest() : base("Property access", "A:reflection, B:dynamic CIL, C:compile-time", DefaultIterations)
		{
		}

		protected PropertyGetDelegate GetPropertyGetter(string typeName, string propertyName)
		{
			// get the property get method
			var t = Type.GetType(typeName);
            if (t == null)
                return null;
            
            var pi = t.GetProperty(propertyName);
            if (pi == null)
                return null;

			var getter = pi.GetGetMethod();
            if (getter == null)
                return null;

			// create a new dynamic method that calls the property getter
			var dm = new DynamicMethod("GetValue", typeof(object), new[] { typeof(object) }, typeof(object), true);
			var ilGenerator = dm.GetILGenerator();

            // emit CIL
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, getter);

			if (getter.ReturnType.GetTypeInfo().IsValueType)
			{
                ilGenerator.Emit(OpCodes.Box, getter.ReturnType);
			}

            ilGenerator.Emit(OpCodes.Ret);

			return dm.CreateDelegate(typeof(PropertyGetDelegate)) as PropertyGetDelegate;
		}

		protected PropertySetDelegate GetPropertySetter(string typeName, string propertyName)
		{
            // get the property get method
            var t = Type.GetType(typeName);
            if (t == null)
                return null;

            var pi = t.GetProperty(propertyName);
            if (pi == null)
                return null;

            var setter = pi.GetSetMethod(false);
            if (setter == null)
                return null;

            // create a new dynamic method that calls the property setter
            var dm = new DynamicMethod("SetValue", typeof(void), new[] { typeof(object), typeof(object) }, typeof(object), true);
			var ilGenerator = dm.GetILGenerator();

            // emit CIL
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);

			var parameterType = setter.GetParameters()[0].ParameterType;

			if (parameterType.GetTypeInfo().IsValueType)
			{
                ilGenerator.Emit(OpCodes.Unbox_Any, parameterType);
			}

            ilGenerator.Emit(OpCodes.Call, setter);
            ilGenerator.Emit(OpCodes.Ret);

			return dm.CreateDelegate(typeof(PropertySetDelegate)) as PropertySetDelegate;
		}

		protected override bool MeasureTestA()
		{
			// get property using reflection
			var sb = new StringBuilder("Arman Hasanpour Pazevari");
			var pi = sb.GetType().GetProperty("Length");
            if (pi == null)
                return false;
            for (var i = 0; i < Iterations; i++)
			{
				var length = pi.GetValue(sb);
				if (!21.Equals(length))
					throw new InvalidOperationException($"Invalid length {length} returned");
			}
			return true;
		}

		protected override bool MeasureTestB()
		{
			// get property using dynamic cil
			var sb = new StringBuilder("Arman Hasanpour Pazevari");
			var getter = GetPropertyGetter("System.Text.StringBuilder", "Length");
			for (var i = 0; i < Iterations; i++)
			{
				var length = getter(sb);
				if (!21.Equals(length))
					throw new InvalidOperationException($"Invalid length {length} returned");
			}
			return true;
		}

		protected override bool MeasureTestC()
		{
			// get property using compiled code
			var sb = new StringBuilder("Arman Hasanpour Pazevari");
			for (var i = 0; i < Iterations; i++)
			{
				var length = sb.Length;
				if (!21.Equals(length))
					throw new InvalidOperationException($"Invalid length {length} returned");
			}
			return true;
		}

	}
}
