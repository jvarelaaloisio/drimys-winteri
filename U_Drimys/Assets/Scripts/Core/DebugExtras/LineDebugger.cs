using UnityEngine;

namespace Core.DebugExtras
{
	public static class LineDebugger
	{
		public static void DrawLines(Vector3[] points, Color color, float duration = 0, bool depthTest = false)
		{
			for (int i = 0; i < points.Length - 1; i++)
				Debug.DrawLine(points[i], points[i + 1], color, duration, depthTest);
		}
	}
}