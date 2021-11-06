using System;
using Core.Helpers;
using Events.UnityEvents;
using MVC;
using UnityEngine;
using UnityEngine.Events;

namespace Characters
{
	[RequireComponent(typeof(Rigidbody))]
	public class CharacterView : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField]
		protected CharacterProperties characterProperties;

		[SerializeField]
		protected bool shouldLogTransitions;

		protected BaseController Controller;
		protected Rigidbody Rigidbody;

		public UnityEvent onJump;
		public UnityEvent onLand;
		public TransformUnityEvent onLock;
		public UnityEvent onUnlock;

		protected CharacterModel Model;
		protected virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
			Model = BuildModel();
			Model.onJump += onJump.Invoke;
			Model.onLand += onLand.Invoke;
			Model.onLock += onLock.Invoke;
			Model.onUnlock += onUnlock.Invoke;
		}

		protected virtual CharacterModel BuildModel()
		{
			return new CharacterModel(transform,
									Rigidbody,
									characterProperties,
									this,
									shouldLogTransitions);
		}

		protected virtual void Update()
		{
			Model.Update(Time.deltaTime);
		}


		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(.7f, .1f, .1f, .25f);
			var position = transform.position;
			var up = transform.up;
			var groundCheckPosition = position - up * characterProperties.GroundDistanceCheck;
			Gizmos.DrawLine(position, groundCheckPosition);
			var radius = characterProperties.GroundedCheckSphereRadius;
			Gizmos.DrawSphere(groundCheckPosition, radius);
		}
	}
}