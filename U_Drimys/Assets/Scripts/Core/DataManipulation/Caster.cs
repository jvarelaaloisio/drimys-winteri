using System;

namespace Core.DataManipulation
{
	public static class Caster
	{
		public static void TryCast<TIn, TOut>(TIn original, out TOut result)
		{
			var temp = (TOut)Convert.ChangeType(original, typeof(TOut));
			result = temp != null
						? temp
						: throw new ArgumentException("Properties field in a thrower class" +
													" should be of type ThrowerProperties");
		}
	}
}
