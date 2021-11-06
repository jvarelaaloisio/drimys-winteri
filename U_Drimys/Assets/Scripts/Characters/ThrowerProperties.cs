using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Properties/Thrower", fileName = "ThrowerProperties", order = 1)]
	public class ThrowerProperties : CharacterProperties
	{
		[SerializeField]
		private float aimDelay;

		public float AimDelay => aimDelay;
	}
}