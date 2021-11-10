using System;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters
{
	public class ThrowerView : AnimatedCharacterView
	{
		[SerializeField]
		protected string aimingParameter = "Aiming";

		[SerializeField]
		private Throwable throwablePrefab;

		[SerializeField]
		private Transform hand;

		protected new ThrowerModel Model;
		protected ThrowerProperties ThrowerProperties;

		protected override void Awake()
		{
			var properties = characterProperties as ThrowerProperties;
			ThrowerProperties = properties
									? properties
									: throw new ArgumentException("Properties field in a thrower class" +
																" should be of type ThrowerProperties");
			base.Awake();
			//NOTE: The new Model hides the original, but doesn't replace it, so it has to be copied to the new one.
			Model = (ThrowerModel)base.Model;
			Model.onAim += () => SetAiming(true);
			Model.onAimCanceled += () => SetAiming(false);
			Model.onThrow += () => SetAiming(false);
		}

		protected override CharacterModel BuildModel()
		{
			return new ThrowerModel(transform,
									Rigidbody,
									ThrowerProperties,
									throwablePrefab,
									hand,
									this,
									shouldLogTransitions);
		}

		private void SetAiming(bool value)
		{
			animator.SetBool(aimingParameter, value);
			if (value)
				TurnOnUpperBodyAnimatorLayer();
			else
				TurnOffUpperBodyAnimatorLayer();
		}

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if(Model == null)
				return;
			Gizmos.color = Model.Flags.IsAiming
								? Model.Flags.CanThrow
									? Color.green
									: Color.red
								: Color.black;
			Gizmos.DrawSphere(hand.position, .05f);
		}
	}
}