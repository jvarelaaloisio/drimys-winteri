using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Extras
{
	public class KnockBackOnOverlap : MonoBehaviour
	{
		[SerializeField]
		private Vector3 force;

		private List<GameObject> victims = new List<GameObject>(20);
		private void OnTriggerEnter(Collider other)
		{
			if(victims.Contains(other.gameObject))
				return;
			victims.Add(other.gameObject);
			other.GetComponent<CharacterView>().KnockBack(force);
		}
	}
}
