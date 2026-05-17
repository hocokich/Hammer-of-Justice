using UnityEngine;
using System.Collections;

public class BossPhase2Behavior : MonoBehaviour
{
	[Header("Движение")]
	[SerializeField] private float horizontalSpeed = 3f;
	[SerializeField] private float minX = -11f;
	[SerializeField] private float maxX = 5f;

	[Header("Колебания (wobble)")]
	[SerializeField] private float wobbleAmplitude = 0.3f;
	[SerializeField] private float wobbleFrequency = 2f;

	[Header("Сброс кала")]
	[SerializeField] private GameObject poopPrefab;
	[SerializeField] private int poopCount = 4;               // сколько снарядов за залп
	[SerializeField] private float poopInterval = 2f;         // интервал между залпами
	[SerializeField] private float poopSpawnSpacing = 1.5f;   // расстояние между снарядами

	private Rigidbody2D rb;
	private float direction = 1f;
	private float timer;
	private Vector3 startPos;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		startPos = transform.position;
		timer = poopInterval;
	}

	private void Update()
	{
		// Горизонтальное движение
		if (transform.position.x >= maxX)
			direction = -1f;
		else if (transform.position.x <= minX)
			direction = 1f;

		rb.linearVelocity = new Vector2(direction * horizontalSpeed, 0f);

		// Колебания по Y
		float wobble = Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
		transform.position = new Vector3(transform.position.x, startPos.y + wobble, transform.position.z);

		// Таймер сброса кала
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			StartCoroutine(DropPoop());
			timer = poopInterval;
		}
	}

	private IEnumerator DropPoop()
	{
		// Выбираем безопасную позицию (одну случайную из возможных)
		int safeIndex = Random.Range(0, poopCount);

		for (int i = 0; i < poopCount; i++)
		{
			if (i == safeIndex) continue;   // пропускаем безопасное место

			float xOffset = (i - (poopCount - 1) * 0.5f) * poopSpawnSpacing;
			Vector3 spawnPos = transform.position + new Vector3(xOffset, -1f, 0f);   // чуть ниже босса

			Instantiate(poopPrefab, spawnPos, Quaternion.identity);

			yield return new WaitForSeconds(0.1f);   // небольшая задержка между снарядами
		}
	}
}