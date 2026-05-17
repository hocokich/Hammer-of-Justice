using UnityEngine;

public class BossMovement : MonoBehaviour
{
	[Header("Настройки расстояния")]
	[SerializeField] private float preferredDistance = 5f;   // дистанция, которую босс старается держать до игрока
	[SerializeField] private float moveSpeed = 3f;           // скорость полёта

	[Header("Границы камеры (левая и правая)")]
	[SerializeField] private float minX = -11f;
	[SerializeField] private float maxX = 5f;

	[Header("Колебания в воздухе")]
	[SerializeField] private float wobbleAmplitude = 0.3f;
	[SerializeField] private float wobbleFrequency = 2f;

	private Transform player;
	private Rigidbody2D rb;
	private Vector3 startPos;   // для Wobble

	private void Start()
	{
		var go = GameObject.FindWithTag("Player");
		if (go) player = go.transform;
		rb = GetComponent<Rigidbody2D>();
		startPos = transform.position;
	}

	private void FixedUpdate()
	{
		if (player == null) return;

		Vector3 dirToPlayer = player.position - transform.position;
		float currentDist = dirToPlayer.magnitude;
		Vector3 dirNorm = dirToPlayer.normalized;

		Vector2 desiredVelocity;

		if (currentDist < preferredDistance * 0.8f)
			// Игрок слишком близко — отлетаем
			desiredVelocity = -dirNorm * moveSpeed;
		else if (currentDist > preferredDistance * 1.2f)
			// Игрок далеко — догоняем
			desiredVelocity = dirNorm * moveSpeed;
		else
			// В "комфортной" зоне — стоим, только wobble
			desiredVelocity = Vector2.zero;

		rb.linearVelocity = desiredVelocity;

		// Ограничение камерой
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp(pos.x, minX, maxX);
		transform.position = pos;

		// Визуальный wobble (синусоида по Y)
		float wobble = Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
		transform.position = new Vector3(transform.position.x, startPos.y + wobble, transform.position.z);
	}
}