using UnityEngine;
using Random = UnityEngine.Random;

public class LootDrop : MonoBehaviour
{
	[Header("Предметы")]
	[SerializeField] private GameObject heartPrefab;
	[SerializeField] private float heartChance = 15f;

	[SerializeField] private GameObject manaPrefab;
	[SerializeField] private float manaChance = 10f;

	[SerializeField] private CoinDrop[] coins;

	[Header("Количество выпадающих предметов")]
	[SerializeField] private int minDropCount = 1;
	[SerializeField] private int maxDropCount = 1;

	[Header("Физика выброса")]
	[SerializeField] private float ejectForce = 5f;
	[SerializeField] private float spreadAngle = 30f;
	[SerializeField] private Vector2 spawnOffset = Vector2.zero;

	private Health health;
	private float totalCoinChance;

	private void Start()
	{
		health = GetComponent<Health>();

		totalCoinChance = 0f;
		foreach (CoinDrop coin in coins)
			totalCoinChance += coin.chance;

		if (health != null)
			health.OnDeath += DropLoot;
	}

	/// <summary> Вызвать при разрушении объекта. Может вызываться извне (AnimationEvent, ChestDrop и т.д.) </summary>
	public void DropLoot()
	{
		int count = Random.Range(minDropCount, maxDropCount + 1);
		for (int i = 0; i < count; i++)
		{
			GameObject prefab = ChoosePrefab();
			if (prefab != null)
				SpawnWithForce(prefab);
		}
	}

	private GameObject ChoosePrefab()
	{
		float heart = heartChance;
		float mana = manaChance;
		float coinTotal = totalCoinChance;
		float total = heart + mana + coinTotal;
		if (total <= 0f) return null;

		float roll = Random.Range(0f, total);
		if (roll < heart) return heartPrefab;
		roll -= heart;
		if (roll < mana) return manaPrefab;
		roll -= mana;

		// Монеты
		foreach (CoinDrop coin in coins)
		{
			if (roll < coin.chance)
				return coin.prefab;
			roll -= coin.chance;
		}
		return null;
	}

	private void SpawnWithForce(GameObject prefab)
	{
		if (prefab == null) return;

		Vector2 spawnPos = (Vector2)transform.position + spawnOffset;
		GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
		Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			float angle = Random.Range(-spreadAngle, spreadAngle);
			Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
			rb.linearVelocity = dir * ejectForce;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	private void OnDestroy()
	{
		if (health != null)
			health.OnDeath -= DropLoot;
	}

	[System.Serializable]
	public class CoinDrop
	{
		public GameObject prefab;
		public float chance;
	}
}