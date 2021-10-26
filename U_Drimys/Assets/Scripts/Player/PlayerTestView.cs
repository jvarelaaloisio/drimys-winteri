using MVC;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerTestView : MonoBehaviour, IView
	{
		private PhysicsFlags _flags;
		private Rigidbody _rigidbody;

		[SerializeField]
		private float jumpForce;

		public Transform Transform => transform;
		public BaseController Controller { get; set; }

		public Vector3 Velocity
		{
			get => _rigidbody.velocity;
			set => _flags.MovementVector = value;
		}

		private void Awake()
		{
			_flags = new PhysicsFlags()
					{
						ShouldJump = false,
						MovementVector = Vector3.zero
					};
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Jump()
		{
			_flags.ShouldJump = true;
		}

		public void Die(float time = 0)
		{
			Destroy(gameObject, time);
		}

		private void FixedUpdate()
		{
			_rigidbody.velocity = _flags.MovementVector;
			if (_flags.ShouldJump)
				_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}

		private struct PhysicsFlags
		{
			public bool ShouldJump;
			public Vector3 MovementVector;
		}
	}
}