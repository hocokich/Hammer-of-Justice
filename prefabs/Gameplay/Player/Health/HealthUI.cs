using UnityEngine;

public class HealthUI : MonoBehaviour
{
	[SerializeField] private GameObject[] hearts;

	private PlayerHealth playerHealth;

	private void Start()
	{
		playerHealth = FindAnyObjectByType<PlayerHealth>();

		if (playerHealth != null)
		{
			playerHealth.OnHealthChanged += UpdateHearts;
			UpdateHearts(playerHealth.CurrentHealth, playerHealth.MaxHealth);
		}
	}

	private void UpdateHearts(int current, int max)
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			hearts[i].SetActive(i < current);
		}
	}

	private void OnDestroy()
	{
		if (playerHealth != null)
		{
			playerHealth.OnHealthChanged -= UpdateHearts;
		}
	}
}