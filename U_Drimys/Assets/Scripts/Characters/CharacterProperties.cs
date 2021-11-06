using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
	[CreateAssetMenu(menuName = "Properties/Character", fileName = "CharacterProperties", order = 0)]
	public class CharacterProperties : ScriptableObject
	{
		[SerializeField]
		private float jumpForce;

		[SerializeField]
		private float groundSpeed;
		
		[SerializeField]
		private float airSpeed;

		[SerializeField]
		private float maxSpeed;

		[SerializeField]
		private float turnSpeed;
		
		[SerializeField]
		private float groundDistanceCheck;
		
		[SerializeField]
		private float groundedCheckSphereRadius;

		[SerializeField]
		private float maxSlopeAngle;

		[SerializeField]
		private LayerMask floorLayer;

		public float JumpForce => jumpForce;
		public float GroundSpeed => groundSpeed;
		public float AirSpeed => airSpeed;
		public float MaxSpeed => maxSpeed;
		public float TurnSpeed => turnSpeed;
		public LayerMask FloorLayer => floorLayer;
		public float MaxSlopeAngle => maxSlopeAngle;
		public float GroundDistanceCheck => groundDistanceCheck;
		public float GroundedCheckSphereRadius => groundedCheckSphereRadius;
	}
}