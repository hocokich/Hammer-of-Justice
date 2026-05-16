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

	private PlayerMotion movement;
	private MeleeAttack meleeAttack;
	private RangeAttack rangeAttack;

	protected override void Start()
	{
		base.Start();

		movement = GetComponent<PlayerMotion>();
		meleeAttack = GetComponent<MeleeAttack>();
		rangeAttack = GetComponent<RangeAttack>();

		// Подписываемся на событие смерти из базового класса
		OnDeath += Die;
	}

	public override void TakeDamage(int damage)
	{
		if (IsDead || isInvincible) return;

		base.TakeDamage(damage);

		if (!IsDead && damage > 0)
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
		DisablePlayerInput inputDisabler = GetComponent<DisablePlayerInput>();
		if (inputDisabler != null) inputDisabler.Disable();

		// Запускаем анимацию смерти
		Animator anim = GetComponent<Animator>();
		if (anim != null)
			anim.SetTrigger("Dead");

		// Замораживаем перемещение по X и даём небольшой подскок
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.linearVelocity = new Vector2(0f, 7.5f);  // подскок вверх
			rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		}

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