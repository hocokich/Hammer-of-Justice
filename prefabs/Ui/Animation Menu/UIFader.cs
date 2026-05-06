using UnityEngine;
using System.Collections;
using System;

public class UIFader : MonoBehaviour
{
	private CanvasGroup canvasGroup;
	[SerializeField] private float fadeDuration = 0.5f;

	private Coroutine fadeCoroutine;

	private void Awake()
	{
		if (canvasGroup == null)
			canvasGroup = GetComponent<CanvasGroup>();
		if (canvasGroup == null)
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
	}

	public void FadeIn(Action onComplete = null)
	{
		StartFade(1f, onComplete);
	}

	public void FadeOut(Action onComplete = null)
	{
		StartFade(0f, onComplete);
	}

	private void StartFade(float targetAlpha, Action onComplete)
	{
		if (fadeCoroutine != null)
			StopCoroutine(fadeCoroutine);

		fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, onComplete));
	}

	private IEnumerator FadeRoutine(float targetAlpha, Action onComplete)
	{
		float startAlpha = canvasGroup.alpha;
		float timer = 0f;

		// Если исчезаем — не выключаем интерактивность до конца
		if (targetAlpha > 0f)
		{
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}

		while (timer < fadeDuration)
		{
			timer += Time.unscaledDeltaTime;
			canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
			yield return null;
		}

		canvasGroup.alpha = targetAlpha;

		// Когда полностью прозрачны — отключаем
		if (targetAlpha <= 0f)
		{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		onComplete?.Invoke();
	}

	public void SetAlpha(float alpha)
	{
		if (canvasGroup != null)
			canvasGroup.alpha = alpha;
	}

	public void FadeInFromHidden()
	{
		gameObject.SetActive(true);
		canvasGroup.alpha = 0f;
		FadeIn();
	}
}