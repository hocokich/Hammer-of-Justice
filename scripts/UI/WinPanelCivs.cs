using UnityEngine;
using UnityEngine.UI;

public class WinPanelCivilians : MonoBehaviour
{
	[Header("»конки жителей")]
	[SerializeField] private Image[] civilianIcons;

	private void OnEnable()
	{
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		var rescued = GameManager.Instance.GetRescuedForLevel(sceneName);

		for (int i = 0; i < civilianIcons.Length; i++)
		{
			civilianIcons[i].enabled = (i < rescued.Count && rescued[i]);
		}
	}
}