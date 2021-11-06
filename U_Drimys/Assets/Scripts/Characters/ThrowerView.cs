using System;
using System.Collections;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters
{
	public class ThrowerView : AnimatedCharacterView
	{
		protected new ThrowerModel Model;

		protected ThrowerProperties ThrowerProperties;

		[SerializeField]
		private string aimingParameter = "Aiming";

		[SerializeField]
		private Throwable throwablePrefab;

		[SerializeField]
		private Transform hand;

		[SerializeField]
		private int upperBodyLayer = 2;

		[SerializeField]
		private float layerWeightTransitionDuration = 1;

		[SerializeField]
		[Range(1, 60)]
		private float layerWeightRefreshFrequency = 20;

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
	}
}