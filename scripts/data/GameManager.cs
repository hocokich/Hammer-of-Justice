using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public int totalExp = 0;
	public int totalRescued = 0;
	public List<LevelSaveData> completedLevels = new List<LevelSaveData>();

	private string saveFilePath;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
		saveFilePath = Path.Combine(Application.persistentDataPath, "hammer_of_justice_save.json");
	}

	private void Start()
	{
		LoadGame();
		Debug.Log($"Загружено: Опыт={totalExp}, Спасено={totalRescued}, Уровней={completedLevels.Count}");
	}

	// Вызывается из LevelManager при завершении уровня

	public bool IsLevelCompleted(string sceneName)
	{
		return completedLevels.Exists(l => l.sceneName == sceneName);
	}

	// Нужно при перепрохождении уровня
	public void SaveLevelData(LevelSaveData levelData)
	{
		LevelSaveData existing = completedLevels.Find(l => l.sceneName == levelData.sceneName);

		if (existing != null)
		{
			// Считаем, сколько было спасено раньше
			int oldRescued = 0;
			if (existing.rescued != null)
			{
				foreach (bool r in existing.rescued) if (r) oldRescued++;
			}

			// Считаем, сколько спасено сейчас
			int newRescued = 0;
			foreach (bool r in levelData.rescued) if (r) newRescued++;

			// Начисляем опыт только за НОВЫХ спасённых
			int extraRescued = newRescued - oldRescued;
			if (extraRescued > 0)
			{
				int extraExp = extraRescued * 20;
				totalExp += extraExp;
				levelData.expEarned = existing.expEarned + extraExp;
				Debug.Log($"Дополнительно спасено {extraRescued} жителей! +{extraExp} опыта");
			}
			else
			{
				levelData.expEarned = existing.expEarned;
			}

			// Обновляем данные
			existing.rescued = levelData.rescued;
			existing.expEarned = levelData.expEarned;

			Debug.Log($"Уровень {levelData.sceneName} обновлён. Спасено: {newRescued}");
		}
		else
		{
			// Первое прохождение
			int rescuedCount = 0;
			foreach (bool r in levelData.rescued) if (r) rescuedCount++;

			completedLevels.Add(levelData);
			totalExp += levelData.expEarned;

			Debug.Log($"Уровень {levelData.sceneName} пройден впервые! Спасено: {rescuedCount}, Опыт: +{levelData.expEarned}");
		}

		RecalculateTotalRescued();
		SaveGame();
	}

	private void RecalculateTotalRescued()
	{
		totalRescued = 0;
		foreach (var level in completedLevels)
		{
			if (level.rescued != null)
			{
				foreach (bool r in level.rescued)
				{
					if (r) totalRescued++;
				}
			}
		}
	}

	// При загрузке уровня берет информацию о спасенных жителях
	public List<bool> GetRescuedForLevel(string sceneName)
	{
		LevelSaveData data = completedLevels.Find(l => l.sceneName == sceneName);
		return data?.rescued ?? new List<bool>();
	}

	public void SaveGame()
	{
		SaveData data = new SaveData
		{
			totalExp = this.totalExp,
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

			totalExp = data.totalExp;
			totalRescued = data.totalRescued;
			completedLevels = data.completedLevels ?? new List<LevelSaveData>();
		}
	}

	public void ResetProgress()
	{
		// Удаляем файл
		if (File.Exists(saveFilePath))
			File.Delete(saveFilePath);

		// Сбрасываем данные в ТЕКУЩЕМ GameManager
		totalExp = 0;
		totalRescued = 0;
		completedLevels.Clear();

		// Сохраняем пустое состояние (создаст новый файл с нулями)
		SaveGame();

		// Загружаем сцену 0
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void OnApplicationQuit() => SaveGame();

	[System.Serializable]
	private class SaveData
	{
		public int totalExp;
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
	public int expEarned;
}