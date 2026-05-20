using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
	[SerializeField] private UIFader fader;
	[SerializeField] private float minLoadTime = 1.5f;

	private void Awake()
	{
		if (fader == null) fader = GetComponent<UIFader>();
		// Объект всегда активен, но прозрачен и не блокирует лучи
		gameObject.SetActive(true);
		fader?.SetAlpha(0f);
		fader?.SetInteractable(false);
	}

	/// <summary>
	/// Загружает сцену с плавным затемнением и последующим проявлением.
	/// Если skipFadeIn = true, затемнение появляется мгновенно (для стартовой загрузки).
	/// </summary>
	public void LoadScene(string sceneName, bool skipFadeIn = false)
	{
		StartCoroutine(LoadRoutine(sceneName, skipFadeIn));
	}

	private IEnumerator LoadRoutine(string sceneName, bool skipFadeIn)
	{
		if (fader == null) yield break;

		// Показываем затемнение (мгновенно или плавно)
		if (skipFadeIn)
		{
			fader.SetAlpha(1f);
			fader.SetInteractable(true);
		}
		else
		{
			yield return fader.FadeInRoutine();
		}

		// Начинаем асинхронную загрузку сцены
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		if (asyncLoad == null)
		{
			Debug.LogError($"Сцена '{sceneName}' не найдена в Build Settings.");
			yield return fader.FadeOutRoutine();
			yield break;
		}
		asyncLoad.allowSceneActivation = false;

		// Ждём завершения загрузки и минимальное время
		float elapsed = 0f;
		while (asyncLoad.progress < 0.9f || elapsed < minLoadTime)
		{
			elapsed += Time.unscaledDeltaTime;
			yield return null;
		}

		// ** Активируем сцену **
		asyncLoad.allowSceneActivation = true;

		// Ждём кадр, чтобы сцена гарантированно отобразилась
		// Пропускаем 3 кадра, чтобы новая сцена "продышалась"
		//yield return new WaitForSecondsRealtime(0.2f);

		fader.SetAlpha(1f);

		// Плавно убираем затемнение
		yield return fader.FadeOutRoutine();
	}
}