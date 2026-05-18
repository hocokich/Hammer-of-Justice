using UnityEngine;
using System.Collections;

public class HintTrigger : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private CanvasGroup hintCanvasGroup;   // ссылка на CanvasGroup подсказки
	[SerializeField] private float fadeDuration = 0.5f;      // длительность появления/исчезновения

	private Coroutine currentFade;

	private void Start()
	{
		if (hintCanvasGroup == null)
			hintCanvasGroup = GetComponentInChildren<CanvasGroup>();
		if (hintCanvasGroup == null)
		{
			// Если нет CanvasGroup, создаём на дочернем Canvas
			Canvas canvas = GetComponentInChildren<Canvas>();
			if (canvas != null)
				hintCanvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
		}
		if (hintCanvasGroup != null)
			hintCanvasGroup.alpha = 0f;   // изначально невидима
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;
		StartFade(1f);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;
		StartFade(0f);
	}

	private void StartFade(float targetAlpha)
	{
		if (hintCanvasGroup == null) return;
		if (currentFade != null) StopCoroutine(currentFade);
		currentFade = StartCoroutine(FadeRoutine(targetAlpha));
	}

	private IEnumerator FadeRoutine(float targetAlpha)
	{
		float startAlpha = hintCanvasGroup.alpha;
		float elapsed = 0f;

		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			hintCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
			yield return null;
		}
		hintCanvasGroup.alpha = targetAlpha;
	}
}