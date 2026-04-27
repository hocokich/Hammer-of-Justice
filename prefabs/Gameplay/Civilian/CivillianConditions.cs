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

		string sceneName = SceneManager.GetActiveScene().name;

		if (!GameManager.Instance.IsLevelCompleted(sceneName)) yield break;	

		List<bool> rescued = GameManager.Instance.GetRescuedForLevel(sceneName);
		if (rescued == null) yield break;

		for (int i = 0; i < civilians.Length && i < rescued.Count; i++)
		{
			civilians[i].SetRescued(rescued[i]);
		}
	}
}