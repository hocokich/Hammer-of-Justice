using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIChestIcon : MonoBehaviour
{
	[SerializeField] private Image chestIcon;

	private void Start()
	{
		if (chestIcon != null)
			chestIcon.enabled = false;

		// Проверяем, был ли сундук открыт ранее (при перезаходе на уровень)
		string sceneName = SceneManager.GetActiveScene().name;
		if (GameManager.Instance.IsLevelCompleted(sceneName))
		{
			var openedChests = GameManager.Instance.GetOpenedChestsForLevel(sceneName);
			if (openedChests != null && openedChests.Count > 0 && openedChests[0])
			{
				ShowChestIcon();
			}
		}

		// Подписываемся на событие открытия в реальном времени
		ChestDrop.OnAnyChestOpened += ShowChestIcon;
	}

	private void ShowChestIcon()
	{
		if (chestIcon != null)
			chestIcon.enabled = true;
	}

	private void OnDestroy()
	{
		ChestDrop.OnAnyChestOpened -= ShowChestIcon;
	}
}