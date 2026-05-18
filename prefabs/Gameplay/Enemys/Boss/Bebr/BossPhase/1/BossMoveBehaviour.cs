using UnityEngine;

[System.Serializable]
public class BossMoveBehaviour
{
	[Header("Дистанция")]
	public float preferredDistance = 5f;
	public float moveSpeed = 3f;

	[Header("Границы камеры")]
	public float minX = -11f;
	public float maxX = 5f;

	[Header("Колебания")]
	public float wobbleAmplitude = 0.3f;
	public float wobbleFrequency = 2f;

	private Transform player;
	private Vector3 startPos;

	public void Initialize(Transform player, Vector3 startPos)
	{
		this.player = player;
		this.startPos = startPos;
	}

	public Vector2 CalculateVelocity(Vector2 currentPosition)
	{
		if (player == null) return Vector2.zero;

		Vector2 dirToPlayer = player.position - (Vector3)currentPosition;
		float dist = dirToPlayer.magnitude;
		Vector2 dirNorm = dirToPlayer.normalized;

		Vector2 desiredVelocity = Vector2.zero;
		if (dist < preferredDistance * 0.8f)
			desiredVelocity = -dirNorm * moveSpeed;
		else if (dist > preferredDistance * 1.2f)
			desiredVelocity = dirNorm * moveSpeed;

		// Ограничение камерой
		float newX = Mathf.Clamp(currentPosition.x + desiredVelocity.x * Time.fixedDeltaTime, minX, maxX);
		float newY = startPos.y + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;

		return new Vector2(newX - currentPosition.x, newY - currentPosition.y) / Time.fixedDeltaTime;
	}
}