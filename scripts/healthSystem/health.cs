using UnityEngine;
using System;

public class Health : MonoBehaviour
{
	[SerializeField] protected int maxHealth = 100;
	protected int currentHealth;

	public event Action<int, int> OnHealthChanged;
	public event Action OnDeath;

	public bool IsDead { get; protected set; } = false;

	protected virtual void Start()
	{
		currentHealth = maxHealth;
		OnHealthChanged?.Invoke(currentHealth, maxHealth);
	}

	public virtual void TakeDamage(int damage)
	{
		if (IsDead) return;

		currentHealth -= damage;
		OnHealthChanged?.Invoke(currentHealth, maxHealth);

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			IsDead = true;
			OnDeath?.Invoke();
		}
	}

	public virtual void Heal(int amount)
	{
		if (IsDead) return;
		currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
		OnHealthChanged?.Invoke(currentHealth, maxHealth);
	}
}