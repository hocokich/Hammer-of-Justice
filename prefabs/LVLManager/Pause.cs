using UnityEngine;

public class Pause : MonoBehaviour
{
	void Start() => ResumeGame();
	// Поставить игру на паузу
	public void PauseGame()=> Time.timeScale = 0f;

	// Продолжить игру
	public void ResumeGame()=> Time.timeScale = 1f;
}
