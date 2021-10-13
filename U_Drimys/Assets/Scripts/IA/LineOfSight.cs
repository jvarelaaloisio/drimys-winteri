using UnityEngine;

namespace Core.IA
{
	public static class LineOfSight
	{
		public static bool IsObjectiveOnSight(Transform searcher,
											Vector3 objectivePosition,
											float viewDistance,
											float fieldOfView,
											int wallLayer)
		{
			Vector3 connection = objectivePosition - searcher.position;
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