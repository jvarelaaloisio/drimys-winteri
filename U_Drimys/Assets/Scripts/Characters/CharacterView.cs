using Core.Helpers;
using Core.Interactions;
using Events.UnityEvents;
using MVC;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
	[RequireComponent(typeof(Rigidbody))]
	public class CharacterView : MonoBehaviour, ICoroutineRunner, IStunnable
	{
		public UnityEvent onJump;
		public UnityEvent onLand;
		public TransformUnityEvent onLock;
		public UnityEvent onUnlock;
		public TransformUnityEvent onAttacking;
		public TransformUnityEvent onAttacked;
		public UnityEvent onStunned;
		public UnityEvent onRecovered;

		[SerializeField]
		protected CharacterProperties characterProperties;

		[SerializeField]
		protected Transform stepValidationLow;

		[SerializeField]
		protected Transform stepValidationHigh;

		[SerializeField]
		protected bool shouldLogTransitions;

		protected BaseController Controller;
		protected Rigidbody Rigidbody;

		protected CharacterModel Model;
		
		protected virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
			Model = BuildModel();
			Model.onJump += onJump.Invoke;
			Model.onLand += onLand.Invoke;
			Model.onLock += onLock.Invoke;
			Model.onUnlock += onUnlock.Invoke;
			Model.onAttacking += onAttacking.Invoke;
			Model.onAttacked += onAttacked.Invoke;
			Model.onStunned += onStunned.Invoke;
			Model.onRecovered += onRecovered.Invoke;
		}

		protected virtual CharacterModel BuildModel()
		{
			return new CharacterModel(transform,
									Rigidbody,
									characterProperties,
									this,
									stepValidationLow,
									stepValidationHigh,
									shouldLogTransitions);
		}

		protected virtual void Update()
			=> Model.Update(Time.deltaTime);

		protected virtual void OnDrawGizmos()
		{
			if(!characterProperties)
				return;
			Gizmos.color = new Color(.7f, .1f, .1f, .25f);
			var position = transform.position;
			var up = transform.up;
			var groundCheckPosition = position - up * characterProperties.GroundDistanceCheck;
			Gizmos.DrawLine(position, groundCheckPosition);
			var radius = characterProperties.GroundedCheckSphereRadius;
			Gizmos.DrawSphere(groundCheckPosition, radius);
			Gizmos.color = new Color(.5f, .5f, .5f, .5f);
			var landingCheckPosition = position - up * characterProperties.LandDistance;
			Gizmos.DrawLine(position, landingCheckPosition);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(stepValidationLow.position,
							transform.forward * characterProperties.StepDistanceCheck);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(stepValidationHigh.position,
							transform.forward * characterProperties.StepDistanceCheck);
			Gizmos.color = new Color(.5f, 0, .5f);
			Gizmos.DrawLine(stepValidationHigh.position + transform.forward * characterProperties.StepDistanceCheck,
							stepValidationLow.position + transform.forward * characterProperties.StepDistanceCheck);
		}

		public void GetStunned(float seconds)
		{
			Model.GetStunned(seconds);
		}
	}
}