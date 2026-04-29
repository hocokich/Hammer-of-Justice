using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDrop : MonoBehaviour
{
	[Header("Дроп")]

	[Header("Хилл")]
	[SerializeField] private GameObject heartPrefab;
	[SerializeField] private float heartChance = 15f;

	[Header("Монеты")]
	[SerializeField] private CoinDrop[] coins;

	[Header("Физика выброса")]
	[SerializeField] private float ejectForce = 5f;
	[SerializeField] private float spreadAngle = 30f;

	private Health health;
	private float totalCoinChance;

	private void Start()
	{
		health = GetComponent<Health>();

		foreach (CoinDrop coin in coins)
			totalCoinChance += coin.chance;

		if (health != null)
			health.OnDeath += DropLoot;
	}

	private void DropLoot()
	{
		float roll = Random.Range(0f, 100f);

		if (roll < heartChance)
		{
			SpawnWithForce(heartPrefab);
			return;
		}

		roll -= heartChance;

		foreach (CoinDrop coin in coins)
		{
			if (roll < coin.chance)
			{
				SpawnWithForce(coin.prefab);
				return;
			}
			roll -= coin.chance;
		}
	}

	private void SpawnWithForce(GameObject prefab)
	{
		if (prefab == null) return;

		GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
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