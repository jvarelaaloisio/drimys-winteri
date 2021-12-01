using UnityEngine;

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
		private float stepDistanceCheck;

		[SerializeField]
		private float stepUpTime;
		
		[SerializeField]
		private float stepDownTime;
		
		[SerializeField]
		private float groundedCheckSphereRadius;

		[SerializeField]
		private float maxSlopeAngle;

		[SerializeField]
		private LayerMask floorLayer;

		[SerializeField]
		private AnimationCurve slopeCompensation;

		[SerializeField]
		private float landDistance;

		[SerializeField]
		private float landingForce;

		public float JumpForce => jumpForce;
		public float GroundSpeed => groundSpeed;
		public float AirSpeed => airSpeed;
		public float MaxSpeed => maxSpeed;
		public float TurnSpeed => turnSpeed;
		public LayerMask FloorLayer => floorLayer;
		public float MaxSlopeAngle => maxSlopeAngle;
		public float GroundDistanceCheck => groundDistanceCheck;
		public float StepDistanceCheck => stepDistanceCheck;
		public float GroundedCheckSphereRadius => groundedCheckSphereRadius;
		public AnimationCurve SlopeCompensation => slopeCompensation;
		public float LandDistance => landDistance;
		public float LandingForce => landingForce;
		public float StepUpTime => stepUpTime;
		public float StepDownTime => stepDownTime;
	}
}