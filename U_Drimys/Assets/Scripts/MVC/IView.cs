using UnityEngine;

namespace MVC
{
	public interface IView
	{
		Transform Transform { get; }
		BaseController Controller { set; }
		void SetVelocity(Vector3 newVelocity);
		Vector3 GetVelocity();
		void AddForce(Vector3 force, ForceMode forceMode);
		void Die(float time = 0);
	}
}