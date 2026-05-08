using UnityEngine;
using UnityEngine.UI;

public class WinPanelChest : MonoBehaviour
{
	[SerializeField] private Image chestImage;

	// Вызывается из LevelManager.CompleteLevel()
	public void UpdateWinPanelChest()
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		var openedChests = GameManager.Instance.GetOpenedChestsForLevel(sceneName);
		bool chestFound = openedChests != null && openedChests.Count > 0 && openedChests[0];
		if (chestImage != null)
			chestImage.enabled = chestFound;
	}
}