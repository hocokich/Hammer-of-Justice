using UnityEngine;

public class CoinRandomStart : MonoBehaviour
{
	private void Start()
	{
		Animator anim = GetComponent<Animator>();
		if (anim != null)
		{
			// Выбираем случайное время от 0 до длины первого клипа анимации
			float randomTime = Random.Range(0f, 1f);
			anim.Play(0, 0, randomTime); // 0 – слой, 0 – имя состояния (по умолчанию), randomTime – нормализованное время
		}
	}
}