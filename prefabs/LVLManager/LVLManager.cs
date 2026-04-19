using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	[Header("»нформаци€ об уровне")]
	public string sceneName;
	public int actNumber = 1;
	public int levelNumber = 1;
	public int baseExpReward = 100;

	[HideInInspector] public bool isCompleted = false;

	void Awake()
	{
		Instance = this;
		sceneName = SceneManager.GetActiveScene().name;
	}

	public void CompleteLevel()
	{
		if (isCompleted) return;

		isCompleted = true;

		GetComponent<Pause>().PauseGame();

		// —обираем список bool спасЄнных жителей
		List<bool> rescued = new List<bool>();
		Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);

		foreach (Civilian c in civilians)
		{
			rescued.Add(c.isRescued);
		}

		int rescuedCount = 0;
		foreach (bool r in rescued) if (r) rescuedCount++;

		LevelSaveData data = new LevelSaveData
		{
			sceneName = sceneName,
			actNumber = actNumber,
			levelNumber = levelNumber,
			rescued = rescued,
			expEarned = baseExpReward + (rescuedCount * 20)
		};

		// ¬место RegisterCompletedLevel Ч используем метод, который обновит или добавит
		GameManager.Instance.SaveLevelData(data);
	}

	private void LoadNextLevel()
	{
		int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
		if (nextIndex < SceneManager.sceneCountInBuildSettings)
		{
			SceneManager.LoadScene(nextIndex);
		}
		else
		{
			SceneManager.LoadScene(0);
		}
	}
}