using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters
{
	[RequireComponent(typeof(Rigidbody))]
	public class Movement : MonoBehaviour
	{
		[SerializeField]
		private LayerMask floor;

		[SerializeField]
		private float maxSlopeAngle = 50;

		[SerializeField]
		private AnimationCurve slopeCompensation = AnimationCurve.Constant(0, 1, 1);
		
		[Header("Debug")]
		[SerializeField]
		private Color floorNormal = new Color(.5f, .2f, .2f);
		
		[SerializeField]
		[Tooltip("Original direction when calling Move")]
		private Color inputDirection = new Color(.2f, .5f, .2f);
		
		[SerializeField]
		[Tooltip("Final movement direction")]
		private Color moveDirection = Color.green;

		private Rigidbody _rigidbody;
		private bool _canMove = true;
		[SerializeField]
		private float forceTest;

		[SerializeField]
		private float speedLimitTest;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void RunTest(Vector2 dir)
		{
			Run(dir.HorizontalPlaneToVector3(), forceTest, speedLimitTest);
		}

		public void Run(Vector3 direction, float force, float speedLimit)
		{
			// Vector3 down = -transform.up;
			// if (Physics.Raycast(transform.position + down * .5f,
			// 					down,
			// 					out RaycastHit hit,
			// 					1.25f,
			// 					floor))
			// {
			// 	float slopeAngle = Vector3.Angle(hit.normal, transform.up);
			// 	Debug.DrawRay(hit.point, hit.normal, floorNormal);
			// 	float angleDelta = slopeAngle / maxSlopeAngle;
			// 	Debug.DrawLine(hit.point + hit.normal,
			// 					hit.point - down,
			// 					new Color(angleDelta, .2f, 1 - angleDelta));
			// 	if (slopeAngle > maxSlopeAngle)
			// 	{
			// 		Debug.Log("Angle too steep");
			// 		return;
			// 	}
			// 	
			// 	float angleSine = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
			// 	Debug.DrawRay(transform.position, direction * force / speedLimit, inputDirection);
			// 	if (direction.magnitude > 0 && Physics.Raycast(transform.position, direction))
			// 		direction.y += slopeCompensation.Evaluate(angleSine);
			// }

			float clampedForce = Mathf.Lerp(0, force, 1 - _rigidbody.velocity.IgnoreY().magnitude / speedLimit);
			Debug.DrawRay(transform.position, direction * clampedForce, moveDirection);
			_canMove = false;
			StartCoroutine(RunInternal(direction, clampedForce, () => _canMove = true));
		}
		
		private IEnumerator RunInternal(Vector3 direction,
										float force,
										Action onFinish)
		{
			yield return new WaitForFixedUpdate();
			_rigidbody.AddForce(direction * force, ForceMode.Force);
			onFinish();
		}
	}
}
