using UnityEngine;
using UnityEngine.UI;

public class WinPanelCivilians : MonoBehaviour
{
	[Header("Иконки жителей")]
	[SerializeField] private Image[] civilianIcons;

	// Вызывается из LevelManager.CompleteLevel()
	public void UpdateWinPanelCivilians()
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		var rescued = GameManager.Instance.GetRescuedForLevel(sceneName);

		for (int i = 0; i < civilianIcons.Length; i++)
		{
			civilianIcons[i].enabled = (i < rescued.Count && rescued[i]);
		}
	}
}