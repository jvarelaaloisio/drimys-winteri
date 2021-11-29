using System.Collections;
using Core.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

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

		[SerializeField]
		private int upperBodyLayer = 2;

		[FormerlySerializedAs("layerWeightTransitionDuration")]
		[SerializeField]
		private float turnOnUpperLayerDuration = 1;
		
		[SerializeField]
		private float turnOffUpperLayerDuration = 1;

		[SerializeField]
		[Range(1, 60)]
		private float layerWeightRefreshFrequency = 40;

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
			Model.onFall += () => SetJumping(true);
			Model.onLand += () => SetJumping(false);
			Model.onStop += () => animator.SetTrigger( stopParameter);
			Model.onAttacking += _ => animator.SetTrigger(attackParameter);
			// Model.onAttacking += _ => TurnOnUpperBodyAnimatorLayer();
			// Model.onAttacked += _ => TurnOffUpperBodyAnimatorLayer();
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

		protected void TurnOnUpperBodyAnimatorLayer()
		{
			StopCoroutine(nameof(SmoothLayerWeightTransition));
			StartCoroutine(SmoothLayerWeightTransition(
														upperBodyLayer,
														1,
														turnOnUpperLayerDuration));
		}

		protected void TurnOffUpperBodyAnimatorLayer()
		{
			StopCoroutine(nameof(SmoothLayerWeightTransition));
			StartCoroutine(SmoothLayerWeightTransition(
														upperBodyLayer,
														0,
														turnOffUpperLayerDuration));
		}

		private IEnumerator SmoothLayerWeightTransition(int layer, float newWeight, float duration)
		{
			float oldWeight = animator.GetLayerWeight(layer);
			float layerWeightRefreshPeriod = 1 / layerWeightRefreshFrequency;
			var waitTillNextPeriod = new WaitForSeconds(layerWeightRefreshPeriod);
			for (float i = 0; i < duration; i += layerWeightRefreshPeriod)
			{
				animator.SetLayerWeight(layer, Mathf.Lerp(oldWeight, newWeight, i / duration));
				yield return waitTillNextPeriod;
			}

			animator.SetLayerWeight(layer, newWeight);
		}

		private void SetJumping(bool value)
		{
			animator.SetBool(_jumpingHash, value);
		}
	}
}