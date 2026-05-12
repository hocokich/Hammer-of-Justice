using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class CivilianConditions : MonoBehaviour
{
	[SerializeField] private Civilian[] civilians;

	private IEnumerator Start()
	{
		yield return null; // ∆дЄм кадр, чтобы Civilian.Start() отработал

		// ≈сли GameManager отсутствует Ц просто выходим (жители будут как при первом прохождении)
		if (!LevelManager.Instance.IsLevelCompleted())
			yield break;

		List<bool> rescued = LevelManager.Instance.GetRescuedForLevel();
		if (rescued == null) yield break;

		for (int i = 0; i < civilians.Length && i < rescued.Count; i++)
		{
			civilians[i].SetRescued(rescued[i]);
		}
	}
}