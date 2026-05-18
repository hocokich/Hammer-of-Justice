using UnityEngine;

public class BossHealthDrop : MonoBehaviour
{
	[SerializeField] private GameObject heartPrefab;
	[SerializeField] private int heartsToDrop = 1;
	[SerializeField] private float dropRadius = 1f;   // разброс для разнообразия

	public void DropHealth()
	{
		for (int i = 0; i < heartsToDrop; i++)
		{
			Vector3 randomOffset = Random.insideUnitCircle * dropRadius;
			Vector3 spawnPos = transform.position + randomOffset;
			Instantiate(heartPrefab, spawnPos, Quaternion.identity);
		}
	}
}