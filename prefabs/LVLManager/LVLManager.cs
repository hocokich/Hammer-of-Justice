using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cainos.PixelArtPlatformer_VillageProps;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	 public string SceneName;
	 public int ActNumber = 1;
	 public int LevelNumber;

	 public bool isCompleted = false;

	void Awake()
	{
		Instance = this;
		SceneName = SceneManager.GetActiveScene().name;
		TryGetLevelNumberFromSceneName();
	}

	public void CompleteLevel()
	{
		if (isCompleted) return;
		isCompleted = true;

		List<bool> rescued = new List<bool>();
		Civilian[] civilians = FindObjectsByType<Civilian>(FindObjectsSortMode.None);
		foreach (Civilian c in civilians)
			rescued.Add(c.isRescued);

		ChestDrop[] chests = FindObjectsByType<ChestDrop>(FindObjectsSortMode.None);
		System.Array.Sort(chests, (a, b) => a.GetChestID().CompareTo(b.GetChestID()));
		List<bool> openedChests = new List<bool>();
		foreach (ChestDrop chest in chests)
			openedChests.Add(chest.GetComponent<Chest>().IsOpened);

		LevelSaveData data = new LevelSaveData
		{
			sceneName = SceneName,
			actNumber = ActNumber,
			levelNumber = LevelNumber,
			rescued = rescued,
			openedChests = openedChests,
			coinsEarned = CoinManager.Instance != null ? CoinManager.Instance.coinsCollected : 0
		};

		CoinManager.Instance.UpdateCoinsPerLvl();

		GameManager.Instance.SaveLevelData(data);
	}

	private void TryGetLevelNumberFromSceneName()
	{
		if (SceneName.StartsWith("lvl", System.StringComparison.OrdinalIgnoreCase))
		{
			string numberPart = SceneName.Substring(3);
			if (int.TryParse(numberPart, out int parsedNumber))
				LevelNumber = parsedNumber;
		}
	}

	public bool IsLevelCompleted()
	{
		return GameManager.Instance != null && GameManager.Instance.GetLevelData(SceneName) != null;
	}

	public List<bool> GetRescuedForLevel()
	{
		var data = GameManager.Instance?.GetLevelData(SceneName);
		return data?.rescued ?? new List<bool>();
	}

	public List<bool> GetOpenedChestsForLevel()
	{
		var data = GameManager.Instance?.GetLevelData(SceneName);
		return data?.openedChests ?? new List<bool>();
	}
}