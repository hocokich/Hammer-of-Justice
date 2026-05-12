using UnityEngine;
using System.Collections;

public class EffectArea : MonoBehaviour
{
	[SerializeField] private Effect effectAsset;
	[SerializeField] private float exitDelay = 0f;
	private Coroutine removeRoutine;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;
		if (removeRoutine != null) { StopCoroutine(removeRoutine); removeRoutine = null; }
		other.GetComponent<EffectManager>()?.ApplyEffect(effectAsset);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;
		if (exitDelay <= 0f) other.GetComponent<EffectManager>()?.RemoveEffect(effectAsset);
		else removeRoutine = StartCoroutine(DelayedRemove(other));
	}

	private IEnumerator DelayedRemove(Collider2D other)
	{
		yield return new WaitForSeconds(exitDelay);
		other.GetComponent<EffectManager>()?.RemoveEffect(effectAsset);
		removeRoutine = null;
	}
}