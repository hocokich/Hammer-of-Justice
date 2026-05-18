using UnityEngine;

[System.Serializable]
public class BossSideMoveBehaviour
{
	public float horizontalSpeed = 3f;
	public float minX = -8f;
	public float maxX = 8f;
	public float wobbleAmplitude = 0.3f;
	public float wobbleFrequency = 2f;
	public float attackYOffset = 1.5f;   // насколько босс поднимается при атаке

	private Vector3 startPos;
	private bool movingRight = true;

	public void Initialize(Vector3 startPos) => this.startPos = startPos;

	public Vector2 GetDesiredVelocity()
	{
		float dir = movingRight ? 1f : -1f;
		return new Vector2(dir * horizontalSpeed, 0f);
	}

	// Высота в фазе атаки (с подъёмом + wobble)
	public float GetAttackY()
	{
		return startPos.y + attackYOffset + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
	}

	// Обычная высота (без подъёма)
	public float GetNormalY()
	{
		return startPos.y + Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
	}

	public bool ClampPosition(ref float x)
	{
		if (x >= maxX) { x = maxX; return true; }
		if (x <= minX) { x = minX; return true; }
		return false;
	}

	public void Flip() => movingRight = !movingRight;
}