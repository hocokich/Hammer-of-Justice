using System;
using System.Collections;
using UnityEngine;

public class Cage : MonoBehaviour
{
	private Health health;
	private Civilian сivilian;

	private SpriteRenderer spriteRenderer;
	private Coroutine fadeCoroutine;

	private void Start()
	{
		сivilian = GetComponentInParent<Civilian>();

		if (сivilian != null && сivilian.isRescued)
			Destroy();

		spriteRenderer = GetComponent<SpriteRenderer>();

		if (TryGetComponent<Health>(out health))
		{
			health.OnDeath += Destroy;
			health.OnHealthChanged += Fade;
		}

	}

	public void Fade(int current, int max)
	{
		if (spriteRenderer == null) return;

		// Прерываем предыдущее затухание
		if (fadeCoroutine != null)
			StopCoroutine(fadeCoroutine);

		// Запоминаем исходные тон и насыщенность
		Color.RGBToHSV(spriteRenderer.color, out float h, out float s, out float originalV);

		// Вспышка (яркость = 1)
		spriteRenderer.color = Color.HSVToRGB(h, s, 1f);

		// Запускаем возврат яркости, передаём originalV
		fadeCoroutine = StartCoroutine(ReturnBrightness(h, s, originalV));
	}

	private IEnumerator ReturnBrightness(float h, float s, float originalV)
	{
		float duration = 0.5f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float v = Mathf.Lerp(1f, originalV, elapsed / duration);
			spriteRenderer.color = Color.HSVToRGB(h, s, v);
			yield return null;
		}

		spriteRenderer.color = Color.HSVToRGB(h, s, originalV);
	}

	public void Destroy()
	{
		сivilian.Rescue();
		Destroy(gameObject);
		return;
	}
}
