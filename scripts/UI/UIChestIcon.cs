using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIChestIcon : MonoBehaviour
{
	[SerializeField] private Image chestIcon;
	[SerializeField] private Sprite chestClosed;
	[SerializeField] private Sprite chestOpened;

	private void Start()
	{
		// ѕоказываем закрытый сундук по умолчанию
		if (chestIcon != null)
			chestIcon.sprite = chestClosed;

		// ≈сли уровень уже пройден и сундук был открыт Ц сразу показываем открытый
		if (LevelManager.Instance.IsLevelCompleted())
		{
			var openedChests = LevelManager.Instance.GetOpenedChestsForLevel();
			if (openedChests != null && openedChests.Count > 0 && openedChests[0])
			{
				if (chestIcon != null)
					chestIcon.sprite = chestOpened;
			}
		}

		// ѕодписываемс€ на событие открыти€ сундука в реальном времени
		ChestDrop.OnAnyChestOpened += OnChestOpened;
	}

	private void OnChestOpened()
	{
		if (chestIcon != null)
			chestIcon.sprite = chestOpened;
	}

	private void OnDestroy()
	{
		ChestDrop.OnAnyChestOpened -= OnChestOpened;
	}
}