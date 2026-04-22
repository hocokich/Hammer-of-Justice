using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake Instance;

	private Vector3 originalPosition;
	private float shakeTimeRemaining;
	private float shakePower;
	private float shakeFadeTime;
	private float shakeRotation;

	void Awake()
	{
		Instance = this;
	}

	void LateUpdate()
	{
		if (shakeTimeRemaining > 0)
		{
			shakeTimeRemaining -= Time.unscaledDeltaTime;

			float xOffset = Random.Range(-1f, 1f) * shakePower;
			float yOffset = Random.Range(-1f, 1f) * shakePower;

			transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

			if (shakeTimeRemaining <= 0)
			{
				transform.position = originalPosition;
			}
		}
	}

	public void StartShake(float duration, float power)
	{
		originalPosition = transform.position;
		shakeTimeRemaining = duration;
		shakePower = power;
	}

	public void ShakeHit() // Короткий удар
	{
		StartShake(0.1f, 0.08f);
	}
}