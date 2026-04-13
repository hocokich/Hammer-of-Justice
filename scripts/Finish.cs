using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
	public UnityEvent onPlayerWin; // Создаем UnityEvent

	void OnTriggerEnter2D(Collider2D other)
	{
		//Условие прохождения уровня
		if (other.CompareTag("Player"))
		{
			onPlayerWin.Invoke();

			string currentScene = SceneManager.GetActiveScene().name;
			GameManager.Instance.CompleteLevel(currentScene);
		}
	}
}