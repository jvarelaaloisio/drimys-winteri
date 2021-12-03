using System;
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

			_transform.rotation = Quaternion.LookRotation(objective - pointA);
			Vector3 bezierOffsetStartLocal = _transform.TransformDirection(bezierOffsetStart);
			Vector3 bezierOffsetEndLocal = _transform.TransformDirection(bezierOffsetEnd);

			Vector3 pointB = pointA + bezierOffsetStartLocal;
			Vector3 pointC = objective + bezierOffsetEndLocal;
			Vector3 pointD = objective;
			LineDebugger.DrawLines(new[] { pointA, pointB, pointC, pointD }, Color.red, duration);

			for (float present = start; present < start + duration; present = Time.time)
			{
				float t = (present - start) / duration;
				UpdatePosition(pointA, pointB, pointC, pointD, t);
				yield return wait;
			}

			UpdatePosition(pointA, pointB, pointC, pointD, 1);
		}

		protected override IEnumerator FlyToTarget(Transform target,
													float duration,
													Action onFinish = null)
		{
			WaitForSeconds wait = new WaitForSeconds(positionUpdatePeriod);
			
			float start = Time.time;

			Vector3 pointA = _transform.position;

			_transform.forward = target.position - pointA;
			Vector3 bezierOffsetStartLocal = _transform.TransformDirection(bezierOffsetStart);
			Vector3 bezierOffsetEndLocal = _transform.TransformDirection(bezierOffsetEnd);

			Vector3 pointB = pointA + bezierOffsetStartLocal;
			UpdatePointsCandD(target,
							bezierOffsetEndLocal,
							out Vector3 pointC,
							out Vector3 pointD);

			for (float present = start; present < start + duration; present = Time.time)
			{
				if (target.hasChanged)
				{
					UpdatePointsCandD(target,
									bezierOffsetEndLocal,
									out pointC,
									out pointD);
				}

				LineDebugger.DrawLines(new[] { pointA, pointB, pointC, pointD },
										Color.red,
										positionUpdatePeriod,
										true);

				float t = (present - start) / duration;
				UpdatePosition(pointA, pointB, pointC, pointD, t);
				yield return wait;
			}

			UpdatePosition(pointA, pointB, pointC, pointD, 1);
			onFinish?.Invoke();
		}

		private static void UpdatePointsCandD(Transform target,
											Vector3 bezierOffsetEndLocal,
											out Vector3 pointC,
											out Vector3 pointD)
		{
			Vector3 objective = target.position;
			pointC = objective + bezierOffsetEndLocal;
			pointD = objective;
		}

		private void UpdatePosition(Vector3 pointA,
									Vector3 pointB,
									Vector3 pointC,
									Vector3 pointD,
									float t)
		{
			var processedT = speedCurve.Evaluate(t);
			//NOTE:Points at t
			Vector3 AB = Vector3.Lerp(pointA, pointB, processedT);
			Vector3 BC = Vector3.Lerp(pointB, pointC, processedT);
			Vector3 CD = Vector3.Lerp(pointC, pointD, processedT);
			Vector3 ABtoBC = Vector3.Lerp(AB, BC, processedT);
			Vector3 BCtoCD = Vector3.Lerp(BC, CD, processedT);
			Vector3 ABBCtoBCCD = Vector3.Lerp(ABtoBC, BCtoCD, processedT);
			LineDebugger.DrawLines(new[] { AB, BC, CD }, Color.blue, positionUpdatePeriod, true);
			Debug.DrawLine(ABtoBC, BCtoCD, Color.black, positionUpdatePeriod, true);

			if ((BCtoCD - ABtoBC).magnitude > .1f)
				_transform.forward = (BCtoCD - ABtoBC).normalized;
			Vector3 newPosition = ABBCtoBCCD;

			_transform.position = newPosition;
		}
	}
}