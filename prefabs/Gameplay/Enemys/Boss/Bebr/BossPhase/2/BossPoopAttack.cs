using UnityEngine;

[System.Serializable]
public class BossPoopAttack
{
	public GameObject poopPrefab;
	public float interval = 0.5f;
	[Range(0, 1)] public float skipChance = 0.2f;
	public float spawnOffsetY = -1f;   // <-- новое: смещение по Y относительно позиции босса

	private float nextPoopTime;

	public void Initialize()
	{
		nextPoopTime = Time.time + interval;
	}

	public void TryDropPoop(Vector3 position)
	{
		if (Time.time >= nextPoopTime)
		{
			nextPoopTime = Time.time + interval;
			if (Random.value > skipChance && poopPrefab)
			{
				Vector3 spawnPos = position + Vector3.up * spawnOffsetY;
				Object.Instantiate(poopPrefab, spawnPos, Quaternion.identity);
			}
		}
	}
}