using UnityEngine;

public class ManaDispenser : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private GameObject ManaPickupPrefab;
	[SerializeField] private Transform SpawnPoint;          // точка, откуда вылетает мана
	[SerializeField] private float spawnOffsetY = 1.5f;     // смещение по Y относительно SpawnPoint
	[SerializeField] private bool OneTimeUse = true;

	private bool used;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (used) return;
		if (!other.CompareTag("Player")) return;

		Mana playerMana = other.GetComponent<Mana>();
		if (playerMana != null && playerMana.CurrentMana < playerMana.MaxMana)
		{
			Vector3 basePos = SpawnPoint != null ? SpawnPoint.position : transform.position;
			Vector3 spawnPos = basePos + Vector3.up * spawnOffsetY;
			Instantiate(ManaPickupPrefab, spawnPos, Quaternion.identity);

			if (OneTimeUse) used = true;
		}
	}
}