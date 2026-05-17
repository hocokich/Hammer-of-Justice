using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeathHandler : MonoBehaviour
{
	[Header("Ссылки")]
	[SerializeField] private GameObject winPanel;      // панель победы
	[SerializeField] private float returnDelay = 5f;   // задержка перед возвратом (даём время на сбор монет)

	private Health bossHealth;

	private void Start()
	{
		GameObject boss = GameObject.FindWithTag("Boss");
		if (boss != null)
		{
			bossHealth = boss.GetComponent<Health>();
			if (bossHealth != null)
				bossHealth.OnDeath += OnBossDeath;
		}
	}

	private void OnBossDeath()
	{
		if (winPanel != null)
			winPanel.SetActive(true);

		// Запускаем отложенное сохранение и возврат в меню
		Invoke(nameof(SaveAndReturn), returnDelay);
	}

	private void SaveAndReturn()
	{
		// Сохраняем прогресс (все монеты уже собраны)
		if (LevelManager.Instance != null)
			LevelManager.Instance.CompleteLevel();

		// Возвращаемся в главное меню
		SceneManager.LoadScene(0);
	}

	private void OnDestroy()
	{
		if (bossHealth != null)
			bossHealth.OnDeath -= OnBossDeath;
	}
}