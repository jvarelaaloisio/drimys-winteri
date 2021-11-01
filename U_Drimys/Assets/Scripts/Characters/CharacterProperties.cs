using UnityEngine;

namespace Characters
{
	[CreateAssetMenu(menuName = "Characters/Properties", fileName = "CharacterProperties", order = 0)]
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

		public float JumpForce => jumpForce;

		public float Speed => speed;
		
		public float MaxSpeed => maxSpeed;

		public float TurnSpeed => turnSpeed;
	}
}