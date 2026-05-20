using System.Collections;
using UnityEngine;

public class StartupLoader : MonoBehaviour
{
	IEnumerator Start()
	{
		yield return null;
		// Загружаем меню, экран сразу становится чёрным (без fade‑in)
		LoadingService.Instance.LoadScene("menu", skipFadeIn: true);
	}
}