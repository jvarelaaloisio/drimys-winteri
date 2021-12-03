using UnityEngine;

namespace Throwables
{
	public class Nut : LerpThrowable
	{
		public override void FlyTo(Vector3 objective, float speed)
		{
			Vector3 forward = objective - transform.position;
			transform.rotation = Quaternion.LookRotation(forward);
			Throw(new Vector3(0,
							bezierOffsetStart.y,
							forward.magnitude)
				);
		}
	}
}