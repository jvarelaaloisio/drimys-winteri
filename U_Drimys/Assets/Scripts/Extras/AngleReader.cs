using System;
using UnityEngine;

namespace Extras
{
	public class AngleReader : MonoBehaviour
	{
		[SerializeField]
		[Range(0, 1)]
		private float rayAlpha;

		[SerializeField]
		[Range(0, 180)]
		private int maxAngle;

		private void OnDrawGizmos()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100))
			{
				var dot = Vector3.Dot(transform.up, hit.normal);
				Gizmos.color = new Color(Mathf.Abs(dot), .1f, .1f, rayAlpha);
				Gizmos.DrawLine(transform.position, hit.point);
				float angle = Vector3.Angle(transform.up, hit.normal);
				float lerp = angle / maxAngle;
				Gizmos.color = lerp > 1
									? Color.red
									: new Color(lerp, 1 - lerp, rayAlpha);
				Gizmos.DrawRay(hit.point, hit.normal);
			}
		}
	}
}