using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Properties/Thrower", fileName = "ThrowerProperties", order = 1)]
	public class ThrowerProperties : CharacterProperties
	{
		[SerializeField]
		private float aimDelay;

		[SerializeField]
		private float throwableSpeed;

		public float AimDelay => aimDelay;
		public float ThrowableSpeed => throwableSpeed;
	}
}