using UnityEngine;

[System.Serializable]
public class BossPoopAttack
{
	public GameObject poopPrefab;
	public float interval = 0.5f;
	[Range(0, 1)] public float skipChance = 0.2f;

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
				GameObject.Instantiate(poopPrefab, position + Vector3.down * 1f, Quaternion.identity);
			}
		}
	}
}