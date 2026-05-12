using UnityEngine;
using UnityEngine.UI;

public class WinPanelChest : MonoBehaviour
{
	[SerializeField] private Image chestImage;

	// Вызывается из LevelManager.CompleteLevel()
	public void UpdateWinPanelChest()
	{
		var openedChests = LevelManager.Instance.GetOpenedChestsForLevel();
		bool chestFound = openedChests != null && openedChests.Count > 0 && openedChests[0];
		if (chestImage != null)
			chestImage.enabled = chestFound;
	}
}