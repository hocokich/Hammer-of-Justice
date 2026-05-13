using UnityEngine;
using UnityEngine.Events;

public class Detection : MonoBehaviour
{
	public UnityEvent OnPlayerDetected;
	public UnityEvent OnPlayerLost;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			OnPlayerDetected?.Invoke();
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
			OnPlayerLost?.Invoke();
	}
}