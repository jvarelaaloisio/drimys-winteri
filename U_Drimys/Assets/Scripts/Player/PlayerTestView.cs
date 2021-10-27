using MVC;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerTestView : MonoBehaviour, IView
	{
		private PhysicalStatus _physicalStatus;
		private Rigidbody _rigidbody;

		public Transform Transform => transform;
		public BaseController Controller { get; set; }

		public Vector3 Velocity
		{
			get => _rigidbody.velocity;
			set => _physicalStatus.MovementVector = value;
		}

		private void Awake()
		{
			_physicalStatus = new PhysicalStatus()
					{
						ShouldJump = false,
						MovementVector = Vector3.zero
					};
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Jump(float jumpForce)
		{
			_physicalStatus.ShouldJump = true;
			_physicalStatus.NextJumpForce = jumpForce;
		}

		public void Die(float time = 0)
		{
			Destroy(gameObject, time);
		}

		private void FixedUpdate()
		{
			_rigidbody.velocity = _physicalStatus.MovementVector;
			if (_physicalStatus.ShouldJump)
				_rigidbody.AddForce(Vector3.up * _physicalStatus.NextJumpForce, ForceMode.Impulse);
		}

		private struct PhysicalStatus
		{
			public bool ShouldJump;
			public float NextJumpForce;
			public Vector3 MovementVector;
		}
	}
}