using System.Collections;
using Core.DebugExtras;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Throwables
{
	public class LerpThrowable : Throwable
	{
		private Transform _transform;

		protected override void Awake()
		{
			base.Awake();
			_transform = transform;
		}

		protected override IEnumerator FlyToPoint(Vector3 objective, float duration)
		{
			WaitForSeconds wait = new WaitForSeconds(positionUpdatePeriod);

			float start = Time.time;
			Vector3 pointA = _transform.position;

			_transform.rotation= Quaternion.LookRotation(objective - pointA);
			Vector3 bezierOffsetStartLocal = _transform.TransformDirection(bezierOffsetStart);
			Vector3 bezierOffsetEndLocal = _transform.TransformDirection(bezierOffsetEnd);

			Vector3 pointB = pointA + bezierOffsetStartLocal;
			Vector3 pointC = objective + bezierOffsetEndLocal;
			Vector3 pointD = objective;
			//TODO: Remove commented lines if DrawLines works
			LineDebugger.DrawLines(new[] { pointA, pointB, pointC }, Color.red, duration);
			
			//TODO:Remove commented lines if For works
			for (float present = start; present < start + duration; present = Time.time)
			{
				float t = (present - start) / duration;
				UpdatePosition(pointA, pointB, pointC, pointD, t);
				yield return wait;
			}
			UpdatePosition(pointA, pointB, pointC, pointD, 1);
		}

		private void UpdatePosition(Vector3 pointA,
									Vector3 pointB,
									Vector3 pointC,
									Vector3 pointD,
									float t)
		{
			var processedT = speedCurve.Evaluate(t);
			Vector3 AtoB = Vector3.Lerp(pointA, pointB, processedT);
			Vector3 BtoC = Vector3.Lerp(pointB, pointC, processedT);
			Vector3 CtoD = Vector3.Lerp(pointC, pointD, processedT);
			Vector3 ABtoBC = Vector3.Lerp(AtoB, BtoC, processedT);
			Vector3 BCtoCD = Vector3.Lerp(BtoC, CtoD, processedT);
			Vector3 ABBCtoBCCD = Vector3.Lerp(ABtoBC, BCtoCD, processedT);
			LineDebugger.DrawLines(new[] { AtoB, BtoC, CtoD }, Color.blue, positionUpdatePeriod);
			// Debug.DrawLine(AtoB, BtoC, Color.blue, positionUpdatePeriod);
			// Debug.DrawLine(BtoC, CtoD, Color.blue, positionUpdatePeriod);
			Debug.DrawLine(ABtoBC, BCtoCD, Color.black, positionUpdatePeriod);

			if ((BCtoCD - ABtoBC).magnitude > .1f)
				_transform.forward = (BCtoCD - ABtoBC).normalized;
			Vector3 newPosition = ABBCtoBCCD;

			_transform.position = newPosition;
		}

		protected override IEnumerator FlyToTarget(Transform target, float duration)
		{
			WaitForSeconds wait = new WaitForSeconds(positionUpdatePeriod);

			float start = Time.time;

			Vector3 bezierOffsetStartLocal = _transform.TransformDirection(bezierOffsetStart);
			Vector3 bezierOffsetEndLocal = _transform.TransformDirection(bezierOffsetEnd);

			Vector3 pointA = _transform.position;
			Vector3 pointB = pointA + bezierOffsetStartLocal;
			Vector3 pointC = new Vector3();
			Vector3 pointD = new Vector3();

			for (float present = start; present < start + duration; present = Time.time)
			{
				Vector3 targetPosition = target.position;
				pointC = targetPosition + bezierOffsetEndLocal;
				pointD = targetPosition;
				LineDebugger.DrawLines(new[] { pointA, pointB, pointC }, Color.red);
				
				float t = (present - start) / duration;
				UpdatePosition(pointA, pointB, pointC, pointD, t);
				yield return wait;
			}
			
			UpdatePosition(pointA, pointB, pointC, pointD, 1);
		}
	}
}