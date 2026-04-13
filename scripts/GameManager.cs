using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	// Сколько уровней прошёл игрок
	public int countLvls = 0;
	// Сколько экспы у игрока
	public int exp = 0;
	// Список пройденных лвлов
	public List<string> completedLvls = new List<string>(); 


	// Путь к файлу сохранения
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

		// Определяем путь один раз при старте
		saveFilePath = Path.Combine(Application.persistentDataPath, "hammer_of_justice_save.json");
	}

	private void Start()
	{
		LoadGame();
		Debug.Log("пройдено лвл-ов: " + countLvls);
		Debug.Log("экспа: " + exp);
	}
	public void CompleteLevel(string sceneName)
	{
		if (completedLvls.Contains(sceneName)) return; // ← Уже проходили — выход

		completedLvls.Add(sceneName);
		countLvls = completedLvls.Count;
		exp += 10;

		SaveGame();
	}

	public void SaveGame()
	{
		SaveData data = new SaveData
		{
			countLvls = this.countLvls,
			exp = this.exp,
			completedLvls = this.completedLvls
		};

		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(saveFilePath, json);

		Debug.Log($"Игра сохранена в: {saveFilePath}");
	}

	public void LoadGame()
	{
		if (File.Exists(saveFilePath))
		{
			string json = File.ReadAllText(saveFilePath);
			SaveData data = JsonUtility.FromJson<SaveData>(json);

			countLvls = data.countLvls;
			exp = data.exp;
			completedLvls = data.completedLvls ?? new List<string>();

		}
	}

	// Сброс прогресса (пригодится для меню "Новая игра")
	public void ResetProgress()
	{
		countLvls = 0;
		exp = 0;
		SaveGame();
		Debug.Log("Прогресс сброшен");
	}

	private void OnApplicationQuit() => SaveGame();

	// Вспомогательный класс для сериализации
	[System.Serializable]
	private class SaveData
	{
		public int countLvls;
		public int exp;
		public List<string> completedLvls;
	}
}