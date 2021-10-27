using System.Collections;
using UnityEngine;

namespace MVC
{
	public interface IView
	{
		Transform Transform { get; }
		BaseController Controller { set; }
		Vector3 Velocity { get; set; }
		void Jump(float jumpForce);
		void Die(float time = 0);
		Coroutine StartCoroutine(IEnumerator routine);
		Coroutine StartCoroutine(string methodName);
		Coroutine StartCoroutine(string methodName, object value);
		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);
		void StopCoroutine(string methodName);
	}
}