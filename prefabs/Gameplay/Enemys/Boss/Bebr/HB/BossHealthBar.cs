using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
	[SerializeField] private Image FillImage;   // ссылка на Image заливки (тип Filled)
	private Health health;

	private void Start()
	{
		// »щем босса по тегу и берЄм его Health
		GameObject boss = GameObject.FindWithTag("Boss");
		if (boss != null) health = boss.GetComponent<Health>();

		if (health != null)
		{
			health.OnHealthChanged += UpdateFill;
			UpdateFill(health.CurrentHealth, health.MaxHealth);
		}
	}

	private void UpdateFill(int current, int max)
	{
		if (FillImage != null)
			FillImage.fillAmount = (float)current / max;
	}

	private void OnDestroy()
	{
		if (health != null)
			health.OnHealthChanged -= UpdateFill;
	}
}