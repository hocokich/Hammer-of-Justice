using UnityEngine;
using System.Collections;

public class DemoToast : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float showDuration = 1.5f;
	[SerializeField] private float fadeDuration = 0.5f;

	private CanvasGroup canvasGroup;
	private Coroutine currentRoutine;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
		canvasGroup.alpha = 0f;
		gameObject.SetActive(false);
	}

	public void Show()
	{
		// Сначала активируем объект, потом запускаем корутину
		gameObject.SetActive(true);
		if (currentRoutine != null) StopCoroutine(currentRoutine);
		currentRoutine = StartCoroutine(ShowRoutine());
	}

	private IEnumerator ShowRoutine()
	{
		float t = 0f;
		while (t < fadeDuration)
		{
			t += Time.unscaledDeltaTime;
			canvasGroup.alpha = t / fadeDuration;
			yield return null;
		}
		canvasGroup.alpha = 1f;

		yield return new WaitForSecondsRealtime(showDuration);

		t = 0f;
		while (t < fadeDuration)
		{
			t += Time.unscaledDeltaTime;
			canvasGroup.alpha = 1f - t / fadeDuration;
			yield return null;
		}
		canvasGroup.alpha = 0f;
		gameObject.SetActive(false);
	}
}