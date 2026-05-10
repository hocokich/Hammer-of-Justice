using UnityEngine;
using TMPro; // Если используешь TextMeshPro
			 // using UnityEngine.UI; // Раскомментируй, если используешь обычный Text

public class LevelTitleSetter : MonoBehaviour
{
	[Header("Текстовые поля")]
	[SerializeField] private TextMeshProUGUI winTitleText;   // для "Уровень ?"
	[SerializeField] private TextMeshProUGUI pauseTitleText; // для "- УРОВЕНЬ ? -"

	private void Start()
	{
		SetLevelTitles();
	}

	private void SetLevelTitles()
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		// Проверяем, что сцена начинается с "lvl" (регистронезависимо)
		if (sceneName.StartsWith("lvl", System.StringComparison.OrdinalIgnoreCase))
		{
			// Извлекаем часть после "lvl"
			string numberPart = sceneName.Substring(3);

			if (int.TryParse(numberPart, out int levelNumber))
			{
				// Формируем строки
				string winTitle = $"Уровень {levelNumber}";
				string pauseTitle = $"- УРОВЕНЬ {levelNumber} -";

				// Присваиваем текстовым полям
				if (winTitleText != null) winTitleText.text = winTitle;
				if (pauseTitleText != null) pauseTitleText.text = pauseTitle;
			}
		}
	}
}