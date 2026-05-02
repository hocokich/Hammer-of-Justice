using UnityEngine;
using UnityEngine.UI;

public class WinPanelChest : MonoBehaviour
{
	[SerializeField] private Image chestImage;   // изображение сундука, которое появляется, если сундук найден

	private void OnEnable()
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		var openedChests = GameManager.Instance.GetOpenedChestsForLevel(sceneName);

		// Сундук отображается, если он был открыт (т.е. найден и разбит)
		bool chestFound = openedChests != null && openedChests.Count > 0 && openedChests[0];
		if (chestImage != null)
			chestImage.enabled = chestFound;
	}
}