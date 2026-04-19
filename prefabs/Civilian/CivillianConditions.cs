using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CivilianConditions : MonoBehaviour
{
	[SerializeField] private Civilian[] civilians;

	private void Start()
	{
		string sceneName = SceneManager.GetActiveScene().name;

		if (GameManager.Instance.IsLevelCompleted(sceneName))
		{
			List<bool> rescued = GameManager.Instance.GetRescuedForLevel(sceneName);

			if (rescued != null)
			{
				for (int i = 0; i < civilians.Length && i < rescued.Count; i++)
				{
					if (rescued[i])
					{
						civilians[i].isRescued = true;

						// Находим и разрушаем дочернюю клетку
						CellDestroy cell = civilians[i].GetComponentInChildren<CellDestroy>();
						if (cell != null)
						{
							cell.Destroy();
						}
					}
				}
			}
		}
	}
}