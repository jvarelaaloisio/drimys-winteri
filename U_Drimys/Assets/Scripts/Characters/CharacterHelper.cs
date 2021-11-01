using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters
{
	public static class CharacterHelper
	{
		public static IEnumerator MoveHorizontally(Rigidbody rigidbody,
													Vector3 direction,
													float speed,
													float maxSpeed)
		{
			yield return new WaitForFixedUpdate();
			rigidbody.AddForce(direction.IgnoreY() * speed, ForceMode.Force);
			var currentVelocity = rigidbody.velocity;
			Vector3 horVelocity = Vector3.ClampMagnitude(currentVelocity.IgnoreY(), maxSpeed);
			rigidbody.velocity = horVelocity.ReplaceY(currentVelocity.y);
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