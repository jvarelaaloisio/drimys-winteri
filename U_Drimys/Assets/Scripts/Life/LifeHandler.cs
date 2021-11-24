using Events.UnityEvents;
using LS;
using UnityEngine;
using UnityEngine.Events;

namespace Life
{
	public sealed class LifeHandler : MonoBehaviour
	{
		public UnityEvent onDeath;
		public IntUnityEvent onHealthPointsChanged;
		
		[SerializeField]
		private int totalLifePoints;

		[SerializeField]
		private bool allowOverflow;

		private Damageable damageable;
		
		[Header("Debug")]
		[SerializeField]
		private bool isLoggingDamage;

		public int TotalLifePoints => damageable.MaxLifePoints;

		public int CurrentLifePoints => damageable.LifePoints;

		public bool AllowOverflow
		{
			get => allowOverflow;
			set => allowOverflow = value;
		}

		private void Awake()
		{
			damageable = new Damageable(totalLifePoints,
										totalLifePoints,
										allowOverflow);
			damageable.OnDeath += Die;
		}
		
		public void TakeDamage(int damagePoints)
		{
			if(isLoggingDamage)
				Debug.Log($"{name}: took {damagePoints} points of damage");
			damageable.TakeDamage(damagePoints);
			onHealthPointsChanged.Invoke(damageable.LifePoints);
		}

		private void Die()
		{
			onDeath.Invoke();
		}

#if UNITY_EDITOR
		[ContextMenu("test Take Damage (1HP)")]
		private void TakeDamageTest1Hp()
		{
			TakeDamage(1);
		}
		
		[ContextMenu("test Take Damage (10HP)")]
		private void TakeDamageTest10Hp()
		{
			TakeDamage(10);
		}
#endif
	}
}