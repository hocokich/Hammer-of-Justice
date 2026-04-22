using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : Health
{
	[Header("Неуязвимость")]
	[SerializeField] private float invincibilityDuration = 1f;
	private bool isInvincible = false;

	[Header("Визуал")]
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float blinkInterval = 0.1f;


	private motion movement;
	private Attack attack;

	protected override void Start()
	{
		base.Start();

		movement = GetComponent<motion>();
		attack = GetComponent<Attack>();

		// Подписываемся на событие смерти из базового класса
		OnDeath += Die;
	}

	public override void TakeDamage(int damage)
	{
		if (IsDead || isInvincible) return;

		base.TakeDamage(damage);

		if (!IsDead)
		{
			StartCoroutine(InvincibilityCoroutine());
		}
	}

	private IEnumerator InvincibilityCoroutine()
	{
		isInvincible = true;

		float timer = 0f;
		bool visible = true;

		while (timer < invincibilityDuration)
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.enabled = visible;
				visible = !visible;
			}

			yield return new WaitForSeconds(blinkInterval);
			timer += blinkInterval;
		}

		if (spriteRenderer != null)
			spriteRenderer.enabled = true;

		isInvincible = false;
	}

	private void Die()
	{
		// Отключаем управление
		if (movement != null) movement.enabled = false;
		if (attack != null) attack.enabled = false;

		// Перезагружаем сцену через 2 секунды
		Invoke(nameof(RestartLevel), 2f);
	}

	private void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnDestroy()
	{
		// Отписываемся от события при уничтожении
		OnDeath -= Die;
	}
}