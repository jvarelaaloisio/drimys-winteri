using System;
using UnityEngine;

namespace Extras
{
	public class TimeDilator : MonoBehaviour
	{
		[SerializeField]
		[Range(0, 1)]
		private float timeScale;

		private void Update()
		{
			Time.timeScale = timeScale;
		}
	}
}
