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

		if (!GameManager.Instance.IsLevelCompleted(sceneName))
		{
			// ”ровень не пройден Ц всЄ тусклое
			iconsView.UpdateIcons(new bool[3] { false, false, false }, false);
			return;
		}

		// ѕройден Ц т€нем данные
		var rescuedList = GameManager.Instance.GetRescuedForLevel(sceneName);
		bool[] rescued = rescuedList != null ? rescuedList.ToArray() : new bool[0];

		var chestsList = GameManager.Instance.GetOpenedChestsForLevel(sceneName);
		bool chestOpened = (chestsList != null && chestsList.Count > 0) ? chestsList[0] : false;

		iconsView.UpdateIcons(rescued, chestOpened);
	}
}