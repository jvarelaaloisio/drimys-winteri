﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactions.Throwables
{
	//TODO: Figure out who brings the hit effect bc the reuse ability may need to change it (maybe an OnHit event).
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Throwable : MonoBehaviour
	{
		[SerializeField]
		protected Vector3 bezierOffsetStart;

		[SerializeField]
		protected Vector3 bezierOffsetEnd;

		[SerializeField]
		protected AnimationCurve speedCurve = AnimationCurve.Constant(0, 1, 1);

		[SerializeField]
		protected float positionUpdatePeriod = .05f;

		[SerializeField]
		protected float lifeTime = 5f;

		[SerializeField]
		protected UnityEvent onDeath;

		protected Rigidbody Rigidbody;

		public virtual void Throw(Vector3 throwForce)
		{
			StopAllCoroutines();
			Rigidbody.isKinematic = false;
			Rigidbody.AddForce(transform.TransformDirection(throwForce),
								ForceMode.Impulse);
		}

		public virtual void Throw(Vector3 objective, float speed)
		{
			Vector3 fromMeToObjective = objective - transform.position;
			float time = fromMeToObjective.magnitude / speed;
			StopAllCoroutines();
			StartCoroutine(FlyToPoint(objective, time));
		}

		public virtual void Throw(Transform target,
								float speed,
								Action onFinish = null)
		{
			Vector3 thisToTarget = target.position - transform.position;
			float duration = thisToTarget.magnitude / speed;
			Debug.Log($"{speed} => {duration}");
			StopAllCoroutines();
			StartCoroutine(FlyToTarget(target,
										duration,
										onFinish));
		}

		public void Freeze()
		{
			Rigidbody.isKinematic = true;
		}

		public void StopDeath()
		{
			CancelInvoke(nameof(Die));
		}

		protected abstract IEnumerator FlyToPoint(Vector3 objective, float duration);

		protected abstract IEnumerator FlyToTarget(Transform target,
													float duration,
													Action onFinish = null);

		protected virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
			Invoke(nameof(Die), lifeTime);
		}

		protected virtual void Die()
		{
			onDeath.Invoke();
			Destroy(gameObject);
		}

		#region Editor

#if UNITY_EDITOR
		[SerializeField]
		private float speedTest = 1;

		[ContextMenu("Shoot test")]
		private void ShootTest()
		{
			StopAllCoroutines();
			transform.position = Vector3.zero;
			Throw(Vector3.forward * 10, speedTest);
		}
#endif

		#endregion
	}
}