using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters
{
	//TODO:Make this class non static and add LUTs to optimize it.
	public static class CharacterHelper
	{
		private static readonly Color HitColor = new Color(.5f, .2f, .2f);
		private static readonly Color TriangleColor = new Color(.2f, .2f, .5f);

		public static IEnumerator MoveHorizontally(CharacterModel model,
													Vector3 direction,
													Action onFinish,
													float speed,
													float maxSpeed)
		{
			yield return new WaitForFixedUpdate();
			Transform modelTransform = model.transform;
			Vector3 down = -modelTransform.up;
			var properties = model.Properties;
			//TODO:Remove if not needed by 1/12
			if (Physics.Raycast(modelTransform.position + down * .5f,
								down,
								out RaycastHit hit,
								1.25f,
								properties.FloorLayer))
			{
				float slopeAngle = Vector3.Angle(hit.normal, modelTransform.up);
				Debug.DrawRay(hit.point, hit.normal, HitColor);
				float angleDelta = slopeAngle / properties.MaxSlopeAngle;
				Debug.DrawLine(hit.point + hit.normal,
								hit.point - down,
								new Color(angleDelta, .2f, 1 - angleDelta));
				float maxSlopeAngle = properties.MaxSlopeAngle;
				if (slopeAngle > maxSlopeAngle)
				{
					Debug.Log("Angle too steep");
					onFinish();
					yield break;
				}

				float angleCosine = Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
				float angleSine = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
				Debug.DrawRay(modelTransform.position, direction, new Color(.2f, .5f, .2f));
				if (direction.magnitude > 0 && Physics.Raycast(modelTransform.position, direction))
					direction.y += properties.SlopeCompensation.Evaluate(angleSine);
			}

			Debug.DrawRay(modelTransform.position, direction, Color.green);

			var modelRigidbody = model.rigidbody;
			modelRigidbody.AddForce(direction * speed, ForceMode.Force);

			Vector3 currentVelocity = modelRigidbody.velocity;
			Vector3 horVelocity
				= Vector3.ClampMagnitude(currentVelocity.IgnoreY(),
										maxSpeed);
			modelRigidbody.velocity = horVelocity.ReplaceY(currentVelocity.y);
			onFinish();
		}

		public static IEnumerator AddForce(Rigidbody rigidbody,
											Vector3 force,
											ForceMode forceMode = ForceMode.Force)
		{
			yield return new WaitForFixedUpdate();
			rigidbody.AddForce(force, forceMode);
		}

		//TODO:Add transform.HasChanged validation when this class is no longer static.
		public static bool IsGrounded(Vector3 feetPosition,
									CharacterProperties properties)
		{
			var value = Physics.OverlapSphereNonAlloc(feetPosition,
													properties.GroundedCheckSphereRadius,
													new Collider[1],
													properties.FloorLayer);
			return value
					> 0;
		}

		public static IEnumerator GoOverStep(Transform transform,
											Vector3 newPosition,
											float duration,
											Action onFinish = null)
		{
			Vector3 origin = transform.position;
			float start = Time.time;
			while (Time.time < start + duration)
			{
				transform.position = Vector3.Lerp(origin, newPosition, (Time.time - start) / duration);
				yield return null;
			}

			transform.position = Vector3.Lerp(origin, newPosition, 1);

			onFinish?.Invoke();
		}

		public static bool IsInFrontOfStepUp(Vector3 validationPositionLow,
											Vector3 validationPositionHigh,
											Vector3 forward,
											float distance,
											LayerMask floor,
											out Vector3 stepPosition)
		{
			float downDistance = validationPositionHigh.y - validationPositionLow.y;
			bool rayForwardLow = Physics.Raycast(validationPositionLow, forward, distance, floor);
			bool rayForwardHigh = Physics.Raycast(validationPositionHigh, forward, distance, floor);
			bool rayBetweenHighAndLow = Physics.Raycast(validationPositionHigh + forward * distance,
														Vector3.down,
														out var hit,
														downDistance,
														floor);
			if (rayForwardLow
				&& !rayForwardHigh
				&& rayBetweenHighAndLow
			)
			{
				stepPosition = hit.point;
				var angle = Vector3.Angle(hit.normal, Vector3.up);
				return angle < 2;
			}
			stepPosition = Vector3.zero;
			return false;
		}
		public static bool IsInFrontOfStepDown(Vector3 validationPositionLow,
											Vector3 validationPositionHigh,
											Vector3 forward,
											float distance,
											LayerMask floor,
											out Vector3 stepPosition)
		{
			float downDistance = validationPositionHigh.y - validationPositionLow.y;
			bool rayForwardLow = Physics.Raycast(validationPositionLow, forward, distance, floor);
			bool rayForwardHigh = Physics.Raycast(validationPositionHigh, forward, distance, floor);
			bool rayBetweenHighAndLow = Physics.Raycast(validationPositionHigh + forward * distance,
														Vector3.down,
														downDistance,
														floor);
			bool rayDownFromLow = Physics.Raycast(validationPositionLow + forward * distance + Vector3.down * .2f,
												Vector3.down,
												out var hit2,
												downDistance,
												floor);
			if (!rayForwardHigh
				&& !rayForwardLow
				&& !rayBetweenHighAndLow
				&& rayDownFromLow)
			{
				stepPosition = hit2.point;
				var angle = Vector3.Angle(hit2.normal, Vector3.up);
				return angle < 2;
			}

			stepPosition = Vector3.zero;
			return false;
		}
	}
}