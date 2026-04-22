using UnityEngine;
using System;

public class Mana : MonoBehaviour
{
	[Header("Настройки маны")]
	[SerializeField] private int maxMana = 3;
	private int currentMana;

	public event Action<int, int> OnManaChanged;

	public int CurrentMana => currentMana;
	public int MaxMana => maxMana;

	private void Start()
	{
		currentMana = maxMana;
		OnManaChanged?.Invoke(currentMana, maxMana);
	}

	public bool UseMana(int amount)
	{
		if (currentMana >= amount)
		{
			currentMana -= amount;
			OnManaChanged?.Invoke(currentMana, maxMana);
			return true;
		}
		return false;
	}

	public void RestoreMana(int amount)
	{
		currentMana = Mathf.Min(currentMana + amount, maxMana);
		OnManaChanged?.Invoke(currentMana, maxMana);
	}
}