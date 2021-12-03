using System;
using System.Collections;

using UnityEngine;
namespace Throwables
{
	public class FireArch : LerpThrowable
	{
		[SerializeField]
		private Vector3 endScale;

		private Vector3 _originScale;

		protected override void Awake()
		{
			base.Awake();
			_originScale = transform.localScale;
		}

		public override void FlyTo(Vector3 objective, float speed)
		{
			Vector3 fromMeToObjective = objective - transform.position;
			float time = fromMeToObjective.magnitude / speed;
			StartCoroutine(FlyToPoint(objective, time));
			StartCoroutine(Scale(lifeTime));
		}

		public override void FlyTargeted(Transform target,
									float speed,
									Action onFinish = null)
		{
			FlyTo(target.position, speed);
		}

		private IEnumerator Scale(float duration)
		{
			float start = Time.time;

			for (float present = start; present < start + duration; present = Time.time)
			{
				transform.localScale = Vector3.Lerp(_originScale,
													endScale,
													speedCurve.Evaluate((present - start) / duration));
				yield return null;
			}
		}
	}
}