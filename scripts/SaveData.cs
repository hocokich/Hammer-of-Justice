using UnityEngine;
[System.Serializable]
public class SaveData
{
	public int levelsCompleted;
	public int playerExperience;
	public int unlockedLevelIndex; // „тобы при запуске загружать сразу нужный акт
}