using UnityEngine;

public class Pause : MonoBehaviour
{
	void Start() => ResumeGame();
	// Поставить игру на паузу
	public void PauseGame()
	{
		Time.timeScale = 0f; // Останавливаем игровое время
		Debug.Log("Игра на паузе");
	}

	// Продолжить игру
	public void ResumeGame()
	{
		Time.timeScale = 1f; // Возвращаем нормальную скорость времени
		Debug.Log("Игра продолжается");
	}
}
