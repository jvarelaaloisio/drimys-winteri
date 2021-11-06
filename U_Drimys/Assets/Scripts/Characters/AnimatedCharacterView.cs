using Core.Extensions;
using UnityEngine;

namespace Characters
{
	public class AnimatedCharacterView : CharacterView
	{
		[SerializeField]
		protected Animator animator;

		[SerializeField]
		private string jumpingParameter = "Jumping";

		[SerializeField]
		private string velocityParameter = "Velocity";
		[SerializeField]
		private string stopParameter = "Stop";
		[SerializeField]
		private string attackParameter = "Attack";

		private int _jumpingHash;
		private int _velocityHash;
		private int _velocityXHash;
		private int _velocityZHash;

		protected override void Awake()
		{
			_jumpingHash = Animator.StringToHash(jumpingParameter);
			_velocityHash = Animator.StringToHash(velocityParameter);
			_velocityXHash = Animator.StringToHash(velocityParameter + "X");
			_velocityZHash = Animator.StringToHash(velocityParameter + "Z");
			base.Awake();

			Rigidbody = GetComponent<Rigidbody>();

			Model.onJump += () => SetJumping(true);
			Model.onLand += () => SetJumping(false);
			Model.onStop += () => animator.SetTrigger( stopParameter);
			Model.onAttacking += _ => animator.SetTrigger(attackParameter);
		}

		protected override void Update()
		{
			base.Update();
			Vector3 velocity = Rigidbody.velocity.IgnoreY() / characterProperties.MaxSpeed;
			animator.SetFloat(_velocityHash, velocity.magnitude);
			if (Model.Flags.IsLocked)
			{
				velocity = transform.InverseTransformDirection(velocity);
				animator.SetFloat(_velocityXHash, velocity.x);
				animator.SetFloat(_velocityZHash, velocity.z);
			}
			else
			{
				animator.SetFloat(_velocityXHash, 0);
				animator.SetFloat(_velocityZHash, velocity.magnitude);
			}
		}

		private void SetJumping(bool value)
		{
			animator.SetBool(_jumpingHash, value);
		}
	}
}