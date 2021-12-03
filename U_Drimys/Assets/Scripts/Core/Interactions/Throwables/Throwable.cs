using System;
using System.Collections;
using Events.UnityEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactions.Throwables
{
	//TODO: Figure out who brings the hit effect bc the reuse ability may need to change it (maybe an OnHit event).
	[RequireComponent(typeof(Rigidbody))]
	public abstract class Throwable : MonoBehaviour
	{
		public FloatUnityEvent onStun;
		
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


		#region Combat

		[SerializeField]
		private float stunRadius;

		[SerializeField]
		private float stunDuration;

		[SerializeField]
		private int damage;

		#endregion

		protected Rigidbody Rigidbody;

		public virtual void Throw(Vector3 throwForce)
		{
			StopAllCoroutines();
			Rigidbody.isKinematic = false;
			Rigidbody.AddForce(transform.TransformDirection(throwForce),
								ForceMode.Impulse);
			Debug.Log($"{name} => thrown with force", gameObject);
		}

		public virtual void FlyTo(Vector3 objective, float speed)
		{
			Debug.Log($"{name} => thrown to objective", gameObject);
			Vector3 fromMeToObjective = objective - transform.position;
			float time = fromMeToObjective.magnitude / speed;
			StopAllCoroutines();
			StartCoroutine(FlyToPoint(objective, time));
		}

		public virtual void FlyTargeted(Transform target,
								float speed,
								Action onFinish = null)
		{
			Debug.Log($"{name} => thrown to target", gameObject);
			Vector3 thisToTarget = target.position - transform.position;
			float duration = thisToTarget.magnitude / speed;
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
		
		public void Stun()
		{
			var stunVictims = new Collider[10];
			var victimQty
				= Physics.OverlapSphereNonAlloc(transform.position, stunRadius, stunVictims,
												LayerMask.GetMask("Enemy"));

			if (victimQty > 0)
			{
				for (int i = 0; i < victimQty; i++)
					if (stunVictims[i].TryGetComponent(out IStunnable stunnable))
						stunnable.GetStunned(stunDuration);

				Debug.Log($"stunned {victimQty} enemies");
				onStun.Invoke(stunRadius);
			}

			// DieWithEffect(effectSpherePrefab, stunRadius, new Color(1, 0, 0, .5f));
			Destroy(gameObject);
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

		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(1, 0, 0, .25f);
			Gizmos.DrawWireSphere(transform.position, stunRadius);
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
			FlyTo(Vector3.forward * 10, speedTest);
		}
#endif

		#endregion
	}
}