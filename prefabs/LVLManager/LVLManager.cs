using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	[Header("Информация об уровне")]
	public string sceneName;
	public int actNumber = 1;
	public int levelNumber = 1;

	[HideInInspector] public bool isCompleted = false;

	void Awake()
	{
		Instance = this;
		sceneName = SceneManager.GetActiveScene().name;
	}

	public void CompleteLevel()
	{
		GetComponent<Pause>().PauseGame();

		if (isCompleted) return;
		isCompleted = true;

		List<bool> rescued = new List<bool>();
		Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);
		foreach (Civilian c in civilians)
			rescued.Add(c.isRescued);

		LevelSaveData data = new LevelSaveData
		{
			sceneName = sceneName,
			actNumber = actNumber,
			levelNumber = levelNumber,
			rescued = rescued,
			coinsEarned = CoinManager.Instance != null ? CoinManager.Instance.coinsCollected : 0
		};

		GameManager.Instance.SaveLevelData(data);
	}
}