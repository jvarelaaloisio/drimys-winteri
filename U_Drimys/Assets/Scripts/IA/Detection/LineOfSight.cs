using UnityEngine;

namespace IA
{
	public static class LineOfSight
	{
		public static bool IsTargetOnSight(Transform searcher,
											Vector3 targetPosition,
											float viewDistance,
											float fieldOfView,
											int wallLayer)
		{
			Vector3 connection = targetPosition - searcher.position;
			if (connection.magnitude > viewDistance
				|| Vector3.Angle(searcher.forward, connection.normalized) > fieldOfView / 2)
				return false;
			if (Physics.Raycast(searcher.position,
								connection.normalized,
								out _,
								connection.magnitude,
								wallLayer))
			{
				Debug.DrawRay(searcher.position, connection, Color.magenta);
				return false;
			}
			return true;
		}
	}
}