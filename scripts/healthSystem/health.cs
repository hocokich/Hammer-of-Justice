using UnityEngine;
using System;

public class Health : MonoBehaviour
{

	[SerializeField] public int MaxHealth = 3;
	public int CurrentHealth = 3;

	public event Action<int, int> OnHealthChanged;
	public event Action OnDeath;

	public bool IsDead { get; protected set; } = false;

	protected virtual void Start()
	{
		CurrentHealth = MaxHealth;
		OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
	}

	public virtual void TakeDamage(int damage)
	{
		if (IsDead) return;

		CurrentHealth -= damage;
		OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

		if (CurrentHealth <= 0)
		{
			CurrentHealth = 0;
			IsDead = true;
			OnDeath?.Invoke();
		}
	}

	public virtual void Heal(int amount)
	{
		if (IsDead) return;
		CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
		OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
	}
}