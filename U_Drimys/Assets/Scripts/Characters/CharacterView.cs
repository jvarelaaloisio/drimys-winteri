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
		protected CharacterProperties properties;
		[SerializeField]
		protected bool shouldLogTransitions;

		protected CharacterModel Model;
		protected BaseController Controller;
		protected Rigidbody Rigidbody;

		public UnityEvent onJump;
		public UnityEvent onLand;
		public TransformUnityEvent onLock;
		public UnityEvent onUnlock;
		protected virtual void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
			Model = new CharacterModel(transform,
										Rigidbody,
										properties,
										this,
										shouldLogTransitions);
			Model.onJump += onJump.Invoke;
			Model.onLand += onLand.Invoke;
			Model.onLock += onLock.Invoke;
			Model.onUnlock += onUnlock.Invoke;
		}

		protected virtual void Update()
		{
			Model.Update(Time.deltaTime);
		}

		protected virtual void OnCollisionEnter(Collision other)
		{
			//TODO:This is a workaround
			Model.Land();
		}
	}
}
