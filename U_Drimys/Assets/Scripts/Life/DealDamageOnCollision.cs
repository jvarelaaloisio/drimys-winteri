using UnityEngine;

namespace Life
{
	public class DealDamageOnCollision : MonoBehaviour
	{
		[SerializeField]
		private int damage;

		private void OnCollisionEnter(Collision other)
		{
			if (other.gameObject.TryGetComponent(out LifeHandler hpHAndler))
				hpHAndler.TakeDamage(damage);
		}
	}
}