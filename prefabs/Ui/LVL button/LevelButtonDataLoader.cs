using UnityEngine;

public class LevelButtonDataLoader : MonoBehaviour
{
	[SerializeField] private string sceneName;               // им€ сцены уровн€ (Act1_Level2 и т.п.)
	[SerializeField] private LevelButtonIcons iconsView;     // ссылка на компонент иконок

	private void Start()
	{
		LoadAndDisplay();
	}

	private void LoadAndDisplay()
	{
		if (string.IsNullOrEmpty(sceneName) || iconsView == null) return;
		if (GameManager.Instance == null) return;

		LevelSaveData levelData = GameManager.Instance.GetLevelData(sceneName);

		if (levelData == null)
		{
			// ”ровень не пройден Ч иконки тусклые
			iconsView.UpdateIcons(new bool[3], false);
			return;
		}

		// ”ровень пройден: получаем спасЄнных жителей
		bool[] rescued = levelData.rescued?.ToArray() ?? new bool[0];
		// —ундук открыт, если список openedChests не пуст и первый элемент true
		bool chestOpened = levelData.openedChests != null && levelData.openedChests.Count > 0 && levelData.openedChests[0];

		iconsView.UpdateIcons(rescued, chestOpened);
	}
}