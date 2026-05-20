using UnityEngine;
using System.Collections;
using System;

public class UIFader : MonoBehaviour
{
	public CanvasGroup CanvasGroup;
	[SerializeField] private float fadeDuration = 0.5f;

	private Coroutine fadeCoroutine;

	private void Awake()
	{
		if (CanvasGroup == null)
			CanvasGroup = GetComponent<CanvasGroup>();
		if (CanvasGroup == null)
			CanvasGroup = gameObject.AddComponent<CanvasGroup>();
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
		float startAlpha = CanvasGroup.alpha;
		float timer = 0f;

		// Если исчезаем — не выключаем интерактивность до конца
		if (targetAlpha > 0f)
		{
			CanvasGroup.interactable = true;
			CanvasGroup.blocksRaycasts = true;
		}

		while (timer < fadeDuration)
		{
			timer += Time.unscaledDeltaTime;
			CanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
			yield return null;
		}

		CanvasGroup.alpha = targetAlpha;

		// Когда полностью прозрачны — отключаем
		if (targetAlpha <= 0f)
		{
			CanvasGroup.interactable = false;
			CanvasGroup.blocksRaycasts = false;
		}

		onComplete?.Invoke();
	}

	public void SetAlpha(float alpha)
	{
		if (CanvasGroup != null)
			CanvasGroup.alpha = alpha;
	}
	public void SetInteractable(bool value)
	{
		if (CanvasGroup != null)
		{
			CanvasGroup.interactable = value;
			CanvasGroup.blocksRaycasts = value;
		}
	}

	public void FadeInFromHidden()
	{
		gameObject.SetActive(true);
		CanvasGroup.alpha = 0f;
		FadeIn();
	}

	//loadScreen
	public IEnumerator FadeInRoutine()
	{
		gameObject.SetActive(true);
		float elapsed = 0f;
		while (elapsed < fadeDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			CanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
			yield return null;
		}
		CanvasGroup.alpha = 1f;
		SetInteractable(true);
	}

	public IEnumerator FadeOutRoutine()
	{
		float elapsed = 0f;
		while (elapsed < fadeDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			CanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
			yield return null;
		}
		CanvasGroup.alpha = 0f;
		SetInteractable(false);
	}

}