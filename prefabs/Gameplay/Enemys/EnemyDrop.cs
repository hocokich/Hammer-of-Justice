using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
	[Header("Дроп")]

	[Header("Хилл")]
	[SerializeField] private GameObject heartPrefab;
	[SerializeField] private float heartChance = 15f;

	[Header("Монеты")]
	[SerializeField] private CoinDrop[] coins;


	private Health health;
	private float totalCoinChance;

	private void Start()
	{
		health = GetComponent<Health>();

		// Считаем общий шанс монет
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
			Instantiate(heartPrefab, transform.position, Quaternion.identity);
			return;
		}

		roll -= heartChance;

		foreach (CoinDrop coin in coins)
		{
			if (roll < coin.chance)
			{
				Instantiate(coin.prefab, transform.position, Quaternion.identity);
				return;
			}
			roll -= coin.chance;
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
		public float chance; // Шанс выпадения (0-100)
	}
}