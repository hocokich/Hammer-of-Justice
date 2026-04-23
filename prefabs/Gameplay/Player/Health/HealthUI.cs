using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
	[Header("Сердечки")]
	[SerializeField] private Image[] hearts;

	[Header("Спрайты")]
	[SerializeField] private Sprite fullHeart;
	[SerializeField] private Sprite emptyHeart;

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
			if (i < current)
				hearts[i].sprite = fullHeart;
			else
				hearts[i].sprite = emptyHeart;
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