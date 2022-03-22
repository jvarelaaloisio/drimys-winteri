using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters
{
	//TODO:Make this class non static and add LUTs to optimize it. (This will be a component)
	public static class CharacterHelper
	{
		private const float SafeDistance = .1f;
		// private static readonly Color HitColor = new Color(.5f, .2f, .2f);
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
			if (Physics.Raycast(modelTransform.position + down * .5f,
								down,
								out RaycastHit floorHit,
								1.25f,
								properties.FloorLayer))
			{
				float slopeAngle = Vector3.Angle(floorHit.normal, modelTransform.up);
				float angleLerp = slopeAngle / properties.MaxSlopeAngle;
				float sin = Mathf.Asin(2 * angleLerp - 1) / Mathf.PI + .5f;
				Color angleDeltaColor = new Color(sin, 0, 1 - sin);
				Debug.DrawRay(floorHit.point, floorHit.normal * .5f, angleDeltaColor);
				Debug.DrawLine(floorHit.point + floorHit.normal * .5f,
								floorHit.point - down * .5f,
								angleDeltaColor);
				float maxSlopeAngle = properties.MaxSlopeAngle;
				if (slopeAngle > maxSlopeAngle && Vector3.Dot(floorHit.normal, direction) < 0)
				{
					Debug.Log("Angle too steep");
					onFinish();
					yield break;
				}

				float angleSine = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
				Debug.DrawRay(modelTransform.position, direction, new Color(.2f, .5f, .2f, .5f));
				bool goingUp = Physics.Raycast(floorHit.point + Vector3.up * .1f, direction);
				if (direction.magnitude > 0 && goingUp)
					direction.y += properties.SlopeCompensation.Evaluate(angleSine);
			}

			Debug.DrawRay(modelTransform.position, direction, new Color(0, 1, 0, .5f));

			var modelRigidbody = model.rigidbody;
			Vector3 velocity = modelRigidbody.velocity.IgnoreY();
			float clampedForce = Mathf.Lerp(0, speed, 1 - velocity.magnitude / maxSpeed);
			Debug.Log($"direction {modelTransform.TransformDirection(direction)} force {clampedForce}");
			modelRigidbody.AddForce(direction * clampedForce, ForceMode.Force);
			// modelRigidbody.AddForce(direction * speed, ForceMode.Force);
			// Vector3 currentVelocity = modelRigidbody.velocity;
			// Vector3 horVelocity
			// 	= Vector3.ClampMagnitude(currentVelocity.IgnoreY(),
			// 							maxSpeed);
			// modelRigidbody.velocity = horVelocity.ReplaceY(currentVelocity.y);
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

		public static bool IsInFrontOfStepDown(Vector3 positionLow,
												Vector3 positionHigh,
												Vector3 forward,
												float distance,
												LayerMask floor,
												out Vector3 stepPosition)
		{
			float downDistance = positionHigh.y - positionLow.y;
			bool rayForwardLow = Physics.Raycast(positionLow, forward, distance, floor);
			bool rayForwardHigh = Physics.Raycast(positionHigh, forward, distance, floor);
			bool rayHighToLow = Physics.Raycast(positionHigh + forward * distance,
												Vector3.down,
												downDistance,
												floor);
			bool rayDownFromLow = Physics.Raycast(positionLow + forward * distance + Vector3.down * .2f,
												Vector3.down,
												out var hit2,
												downDistance,
												floor);
			if (!rayForwardHigh
				&& !rayForwardLow
				&& !rayHighToLow
				&& rayDownFromLow)
			{
				stepPosition = hit2.point;
				var angle = Vector3.Angle(hit2.normal, Vector3.up);
				return angle < 2;
			}

			stepPosition = Vector3.zero;
			return false;
		}

		public static bool IsInFrontOfStep(Vector3 validationPositionLow,
											Vector3 validationPositionHigh,
											Vector3 forward,
											float distance,
											LayerMask floor,
											out Vector3 stepPosition)
		{
			bool rayForwardHigh = Physics.Raycast(validationPositionHigh,
												forward,
												out var hitA,
												distance,
												floor);
			bool rayForwardLow = Physics.Raycast(validationPositionLow,
												forward,
												out var hitB,
												distance,
												floor);
			float highToLowDistance = validationPositionHigh.y - validationPositionLow.y;
			bool rayHighToLow = Physics.Raycast(validationPositionHigh + forward * distance,
												Vector3.down,
												out var hitC,
												highToLowDistance,
												floor);
			bool rayDownLow = Physics.Raycast(validationPositionLow,
											Vector3.down,
											out var hitD,
											highToLowDistance,
											floor);
			bool rayForwardDownLow = Physics.Raycast(validationPositionLow + forward * distance,
													Vector3.down,
													out var hitE,
													highToLowDistance,
													floor);
			//Wall
			if (rayForwardHigh)
			{
				stepPosition = Vector3.zero;
				Debug.Log("Wall");
				return false;
			}

			//Start Slope or Step Up
			if (rayForwardLow
				&& rayHighToLow
				&& Vector3.Dot(hitD.normal, Vector3.up) > .99f
			)
			{
				stepPosition = hitC.point;
				Debug.Log("Step/Slope Up");
				return true;
			}
			
			//Step Down
			if (rayForwardDownLow
				&& hitE.distance - hitD.distance > SafeDistance
				&& Vector3.Dot(hitD.normal, Vector3.up) > .99f
				&& Vector3.Dot(hitE.normal, Vector3.up) > .99f
			)
			{
				stepPosition = hitE.point;
				Debug.Log("Step Down");
				return true;
			}

			stepPosition = Vector3.zero;
			return false;
		}
	}
}