using UnityEngine;
using UnityEngine.Events;

public class EnemyDetection : MonoBehaviour
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