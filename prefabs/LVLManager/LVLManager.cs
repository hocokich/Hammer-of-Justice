using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cainos.PixelArtPlatformer_VillageProps;

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
		//GetComponent<Pause>().PauseGame();

		if (isCompleted) return;
		isCompleted = true;

		List<bool> rescued = new List<bool>();
		Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);
		foreach (Civilian c in civilians)
			rescued.Add(c.isRescued);

		// Собираем открытые сундуки
		ChestDrop[] chests = FindObjectsByType<ChestDrop>(FindObjectsSortMode.None);
		System.Array.Sort(chests, (a, b) => a.GetChestID().CompareTo(b.GetChestID()));
		List<bool> openedChests = new List<bool>();
		foreach (ChestDrop chest in chests)
			openedChests.Add(chest.GetComponent<Chest>().IsOpened);

		LevelSaveData data = new LevelSaveData
		{
			sceneName = sceneName,
			actNumber = actNumber,
			levelNumber = levelNumber,
			rescued = rescued,
			openedChests = openedChests,
			coinsEarned = CoinManager.Instance != null ? CoinManager.Instance.coinsCollected : 0
		};

		CoinManager.Instance.UpdateCoinsPerLvl();
		GameManager.Instance.SaveLevelData(data);
	}
}