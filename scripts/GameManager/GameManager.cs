using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public int totalCoins = 0;
	public int totalRescued = 0;
	public List<LevelSaveData> completedLevels = new List<LevelSaveData>();

	private string saveFilePath;

	void Awake()
	{
		Application.targetFrameRate = 60;

		if (Instance != null)
			Destroy(gameObject);

		Instance = this;
		DontDestroyOnLoad(gameObject);
		saveFilePath = Path.Combine(Application.persistentDataPath, "hammer_of_justice_save.json");
	}

	private void Start() => LoadGame();

	// Потратить монеты (для прокачки)
	public bool SpendCoins(int amount)
	{
		if (totalCoins >= amount)
		{
			totalCoins -= amount;
			SaveGame();
			return true;
		}
		return false;
	}

	// Сохранить данные уровня при завершении
	public void SaveLevelData(LevelSaveData levelData)
	{
		LevelSaveData existing = completedLevels.Find(l => l.sceneName == levelData.sceneName);

		if (existing != null)
		{
			existing.rescued = levelData.rescued;
			existing.openedChests = levelData.openedChests;   // ← добавить
			existing.coinsEarned = levelData.coinsEarned;
		}
		else
		{
			completedLevels.Add(levelData);
		}

		// Переносим монеты уровня в общий счёт
		totalCoins += levelData.coinsEarned;

		RecalculateTotalRescued();
		SaveGame();
	}

	private void RecalculateTotalRescued()
	{
		totalRescued = 0;
		foreach (var level in completedLevels)
		{
			if (level.rescued != null)
				foreach (bool r in level.rescued)
					if (r) totalRescued++;
		}
	}

	public LevelSaveData GetLevelData(string sceneName) =>
		completedLevels.Find(l => l.sceneName == sceneName);

	public void SaveGame()
	{
		SaveData data = new SaveData
		{
			totalCoins = this.totalCoins,
			totalRescued = this.totalRescued,
			completedLevels = this.completedLevels
		};

		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(saveFilePath, json);
	}

	public void LoadGame()
	{
		if (File.Exists(saveFilePath))
		{
			string json = File.ReadAllText(saveFilePath);
			SaveData data = JsonUtility.FromJson<SaveData>(json);

			totalCoins = data.totalCoins;
			totalRescued = data.totalRescued;
			completedLevels = data.completedLevels ?? new List<LevelSaveData>();
		}
	}

	public void ResetProgress()
	{
		if (File.Exists(saveFilePath))
			File.Delete(saveFilePath);

		totalCoins = 0;
		totalRescued = 0;
		completedLevels.Clear();

		SaveGame();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void OnApplicationQuit() => SaveGame();

	[System.Serializable]
	private class SaveData
	{
		public int totalCoins;
		public int totalRescued;
		public List<LevelSaveData> completedLevels;
	}
}

[System.Serializable]
public class LevelSaveData
{
	public string sceneName;
	public int actNumber;
	public int levelNumber;
	public List<bool> rescued;
	public List<bool> openedChests;
	public int coinsEarned; //Монет на уровне
}