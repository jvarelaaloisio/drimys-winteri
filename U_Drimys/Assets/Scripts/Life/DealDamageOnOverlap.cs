using System;
using UnityEngine;

namespace Life
{
	public class DealDamageOnOverlap : MonoBehaviour
	{
		[SerializeField]
		private int damage;
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out LifeHandler hpHAndler))
				hpHAndler.TakeDamage(damage);
		}
	}
}
