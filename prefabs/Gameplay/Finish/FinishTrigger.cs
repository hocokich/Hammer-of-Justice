using UnityEngine;
using UnityEngine.Events;

public class FinishTrigger : MonoBehaviour
{
	[SerializeField] private UnityEvent onFinish;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			//if(LevelManager.Instance != null)
			//	LevelManager.Instance.CompleteLevel();

			onFinish?.Invoke();
		}
	}
}