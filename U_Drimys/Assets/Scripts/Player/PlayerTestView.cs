using Characters;
using Events.UnityEvents;
using MVC;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerTestView : MonoBehaviour, IView
	{
		[SerializeField]
		private CharacterProperties properties;

		private CharacterModel _model;
		private BaseController _controller;

		public Vector2UnityEvent onMove;
		
		public Transform Transform => transform;
		
		public Rigidbody Rigidbody { get; private set; }

		public BaseController Controller { get; set; }

		public BaseModel Model => _model;

		private void Awake()
		{
			Rigidbody = GetComponent<Rigidbody>();
		}

		public void Setup(BaseController controller)
		{
			_controller = controller;
			_model = new CharacterModel(this,
										properties);
			_model.onMove += onMove.Invoke;
		}

		public void Die(float time = 0)
		{
			Destroy(gameObject, time);
		}

		private void Update()
		{
			_model.Update(Time.deltaTime);
		}

		private void OnCollisionEnter(Collision other)
		{
			_model.Land();
		}

		private void OnCollisionExit(Collision other)
		{
			
		}
	}
}