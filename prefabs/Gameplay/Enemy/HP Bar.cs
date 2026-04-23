using UnityEngine;
using System.Collections;

public class EnemyHealthBar : MonoBehaviour
{
	[SerializeField] private Transform fillBar;
	[SerializeField] private float fadeDuration = 0.5f;

	private Health health;
	private Vector3 fullScale;
	private SpriteRenderer[] sprites;
	private Coroutine fadeCoroutine;

	private void Start()
	{
		health = GetComponentInParent<Health>();
		fullScale = fillBar.localScale;
		sprites = GetComponentsInChildren<SpriteRenderer>();

		health.OnHealthChanged += (cur, max) =>
		{
			// Меняем ширину заливки
			fillBar.localScale = new Vector3(fullScale.x * cur / max, fullScale.y, fullScale.z);

			// Перезапускаем затухание
			if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
			fadeCoroutine = StartCoroutine(Fade());
		};
	}

	private IEnumerator Fade()
	{
		// Показываем
		foreach (var s in sprites)
			s.color = new Color(s.color.r, s.color.g, s.color.b, 1f);

		yield return new WaitForSeconds(1f);

		// Плавно скрываем
		float t = 0f;
		while (t < fadeDuration)
		{
			t += Time.deltaTime;
			foreach (var s in sprites)
				s.color = new Color(s.color.r, s.color.g, s.color.b, 1f - t / fadeDuration);
			yield return null;
		}
	}
}