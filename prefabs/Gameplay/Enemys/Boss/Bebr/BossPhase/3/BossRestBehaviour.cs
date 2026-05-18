using UnityEngine;

[System.Serializable]
public class BossRestBehaviour
{
	public float wobbleAmplitude = 0.2f;
	public float wobbleFrequency = 1f;
	public float driftSpeed = 0.5f;
	public float minX = -11f;
	public float maxX = 5f;
	public float duration = 5f;

	private Vector3 startPos;
	private bool movingRight = true;

	public void Initialize(Vector3 startPos)
	{
		this.startPos = startPos;
	}

	public Vector2 CalculateVelocity(Vector2 currentPosition)
	{
		float dir = movingRight ? 1f : -1f;
		float newX = currentPosition.x + dir * driftSpeed * Time.fixedDeltaTime;
		if (newX >= maxX) movingRight = false;
		else if (newX <= minX) movingRight = true;

		float newY = startPos.y + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
		return new Vector2(Mathf.Clamp(newX, minX, maxX) - currentPosition.x, newY - currentPosition.y) / Time.fixedDeltaTime;
	}
}