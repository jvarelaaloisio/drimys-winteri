using Core.Extensions;
using UnityEngine;

namespace Characters
{
	public class AnimatedCharacterView : CharacterView
	{
		[SerializeField]
		protected Animator animator;

		[SerializeField]
		private string jumping = "Jumping";

		[SerializeField]
		private string velocity = "Velocity";

		private int _jumpingParameter;
		private int _velocityParameter;
		private int _velocityXParameter;
		private int _velocityZParameter;

		protected override void Awake()
		{
			_jumpingParameter = Animator.StringToHash(jumping);
			_velocityParameter = Animator.StringToHash(velocity);
			_velocityXParameter = Animator.StringToHash(velocity + "X");
			_velocityZParameter = Animator.StringToHash(velocity + "Z");
			base.Awake();

			Rigidbody = GetComponent<Rigidbody>();

			Model.onJump += () => SetJumping(true);
			Model.onLand += () => SetJumping(false);
		}

		protected override void Update()
		{
			base.Update();
			Vector3 velocity = Rigidbody.velocity.IgnoreY() / properties.MaxSpeed;
			animator.SetFloat(_velocityParameter, velocity.magnitude);
			if (Model.Flags.IsLocked)
			{
				velocity = transform.InverseTransformDirection(velocity);
				animator.SetFloat(_velocityXParameter, velocity.x);
				animator.SetFloat(_velocityZParameter, velocity.z);
			}
			else
			{
				animator.SetFloat(_velocityXParameter, 0);
				animator.SetFloat(_velocityZParameter, velocity.magnitude);
			}
			//TODO:Add these booleans
			//Also "IsAimPressed" can be the same as "Attacking" in the caraya.
			// animator.SetBool("Aiming", player.IsAimPressed);
		}

		private void SetJumping(bool value)
		{
			animator.SetBool(_jumpingParameter, value);
		}
	}
}