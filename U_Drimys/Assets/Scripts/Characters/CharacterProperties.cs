using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Properties/Character", fileName = "CharacterProperties", order = 0)]
	public class CharacterProperties : ScriptableObject
	{
		[SerializeField]
		private float jumpForce;

		[SerializeField]
		private float speed;

		[SerializeField]
		private float maxSpeed;

		[SerializeField]
		private float turnSpeed;

		[SerializeField]
		private LayerMask floorLayer;

		[SerializeField]
		private float maxSlopeAngle;

		public float JumpForce => jumpForce;
		public float Speed => speed;
		public float MaxSpeed => maxSpeed;
		public float TurnSpeed => turnSpeed;
		public LayerMask FloorLayer => floorLayer;
		public float MaxSlopeAngle => maxSlopeAngle;
	}
}