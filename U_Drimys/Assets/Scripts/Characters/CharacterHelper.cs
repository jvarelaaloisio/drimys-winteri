using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters
{
	public static class CharacterHelper
	{
		private static readonly Color HitColor = new Color(.5f, .2f, .2f);
		private static readonly Color TriangleColor = new Color(.2f, .2f, .5f);

		public static IEnumerator MoveHorizontally(CharacterModel model,
													Vector3 direction,
													Action onFinish)
		{
			yield return new WaitForFixedUpdate();
			//TODO: Raycast to the floor and use the angle to modify the direction so it follows slopes
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
				var slopeAngle = Vector3.Angle(hit.normal, modelTransform.up);
				Debug.DrawRay(hit.point, hit.normal, HitColor);
				Debug.DrawLine(hit.point + hit.normal,
								hit.point - down,
								TriangleColor);
				var maxSlopeAngle = properties.MaxSlopeAngle;
				if (slopeAngle > maxSlopeAngle)
				{
					Debug.Log("Angle too steep");
					onFinish();
					yield break;
				}
			
				var angleCosine = Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
				// Debug.Log($"angle: {slopeAngle}");
			}

			Debug.DrawRay(modelTransform.position, direction, Color.green);

			var modelRigidbody = model.rigidbody;
			modelRigidbody.AddForce(direction * properties.Speed, ForceMode.Force);

			Vector3 currentVelocity = modelRigidbody.velocity;
			Vector3 horVelocity
				= Vector3.ClampMagnitude(currentVelocity.IgnoreY(),
										properties.MaxSpeed);
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
	}
}