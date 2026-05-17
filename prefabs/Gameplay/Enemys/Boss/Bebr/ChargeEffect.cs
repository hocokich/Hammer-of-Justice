using UnityEngine;
using System.Collections;

public class ChargeEffect : MonoBehaviour
{
	[SerializeField] private float duration = 0.8f;   // длительность зарядки
	[SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);   // кривая размера

	private Coroutine currentRoutine;

	public void Play()
	{
		if (currentRoutine != null) StopCoroutine(currentRoutine);
		currentRoutine = StartCoroutine(ChargeRoutine());
	}

	private IEnumerator ChargeRoutine()
	{
		transform.localScale = Vector3.zero;
		float timer = 0f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			float t = timer / duration;
			transform.localScale = Vector3.one * scaleCurve.Evaluate(t);
			yield return null;
		}
		transform.localScale = Vector3.zero; // исчезает после зарядки
	}
}