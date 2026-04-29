using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using Cainos.PixelArtPlatformer_VillageProps;

public class ChestDrop : MonoBehaviour
{
	[Header("Настройки дропа")]
	[SerializeField] private CoinDrop[] coins;
	[SerializeField] private int minCoins = 3;
	[SerializeField] private int maxCoins = 6;
	[SerializeField] private float ejectForce = 5f;
	[SerializeField] private float spreadAngle = 30f;
	[SerializeField] private Vector2 spawnOffset = new Vector2(0f, 0.8f);

	[Header("Сохранение")]
	[SerializeField] private int chestID = 0;          // 0,1,2...

	private Chest chest;
	private Health health;

	private void Start()
	{
		chest = GetComponent<Chest>();
		health = GetComponent<Health>();

		if (health != null)
			health.OnDeath += OnChestDestroyed;

		// Если уровень уже проходили, восстанавливаем открытые сундуки
		string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
		if (GameManager.Instance.IsLevelCompleted(sceneName))
		{
			List<bool> openedChests = GameManager.Instance.GetOpenedChestsForLevel(sceneName);
			if (openedChests != null && chestID < openedChests.Count && openedChests[chestID])
			{
				SetAlreadyOpened();
			}
		}
	}

	private void SetAlreadyOpened()
	{
		chest.Open();   // анимация открытия
		if (health != null)
			Destroy(health);          // чтобы нельзя было сломать повторно
		Collider2D col = GetComponent<Collider2D>();
		if (col != null) col.enabled = false;   // чтобы не мешал игроку
	}

	private void OnChestDestroyed()
	{
		if (chest.IsOpened) return;   // уже открыт
		chest.Open();                 // открываем сундук (монеты вылетят по Animation Event)
	}

	public void EjectCoins()
	{
		int coinCount = Random.Range(minCoins, maxCoins + 1);
		for (int i = 0; i < coinCount; i++)
		{
			SpawnRandomCoin();
		}
	}

	private void SpawnRandomCoin()
	{
		if (coins == null || coins.Length == 0) return;

		float totalChance = 0f;
		foreach (var coin in coins) totalChance += coin.chance;

		float roll = Random.Range(0f, totalChance);
		float cumulative = 0f;
		GameObject selectedPrefab = coins[0].prefab;

		foreach (var coin in coins)
		{
			cumulative += coin.chance;
			if (roll <= cumulative)
			{
				selectedPrefab = coin.prefab;
				break;
			}
		}

		Vector2 spawnPos = (Vector2)transform.position + spawnOffset;
		GameObject coinObj = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
		Rigidbody2D rb = coinObj.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			float angle = Random.Range(-spreadAngle, spreadAngle);
			Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
			rb.linearVelocity = dir * ejectForce;
			rb.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public int GetChestID() => chestID;

	private void OnDestroy()
	{
		if (health != null) health.OnDeath -= OnChestDestroyed;
	}

	[Serializable]
	public class CoinDrop
	{
		public GameObject prefab;
		public float chance;
	}
}