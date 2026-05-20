using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingDots : MonoBehaviour
{
	[SerializeField] private TMP_Text loadingText;          // если не назначишь, найдёт сам
	[SerializeField] private float interval = 0.4f;         // скорость смены точек
	[SerializeField] private string baseText = "Загрузка";  // без точек

	private Coroutine dotsRoutine;

	private void OnEnable()
	{
		if (loadingText == null) loadingText = GetComponent<TMP_Text>();
		dotsRoutine = StartCoroutine(AnimateDots());
	}

	private void OnDisable()
	{
		if (dotsRoutine != null) StopCoroutine(dotsRoutine);
	}

	private IEnumerator AnimateDots()
	{
		int dotCount = 1;
		int direction = 1;   // 1 = увеличиваем, -1 = уменьшаем
		while (true)
		{
			loadingText.text = baseText + new string('.', dotCount);
			yield return new WaitForSecondsRealtime(interval);

			dotCount += direction;
			if (dotCount == 3 || dotCount == 1)
				direction *= -1;   // разворачиваем направление на краях
		}
	}
}