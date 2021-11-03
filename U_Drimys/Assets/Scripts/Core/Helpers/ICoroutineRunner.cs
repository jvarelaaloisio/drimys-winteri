using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Core.Helpers
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(string methodName);
		Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value);
		Coroutine StartCoroutine(IEnumerator routine);
		void StopCoroutine(IEnumerator routine);
		void StopCoroutine(Coroutine routine);
		void StopCoroutine(string methodName);
		void StopAllCoroutines();
	}
}
