using UnityEngine;

public class SpriteFlipToPlayer : MonoBehaviour
{
	[SerializeField] private Detection detection;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private bool startFacingRight = true; // исходное направление спрайта

	private void Start()
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer>();
		if (detection == null)
			detection = GetComponentInChildren<Detection>();

		if (detection != null)
		{
			detection.OnPlayerDetected.AddListener(OnPlayerDetected);
			detection.OnPlayerLost.AddListener(OnPlayerLost);
		}
	}

	private void OnPlayerDetected()
	{
		if (spriteRenderer == null) return;
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			// flipX = true, если игрок слева от нас
			spriteRenderer.flipX = player.transform.position.x < transform.position.x;
		}
	}

	private void OnPlayerLost()
	{
		if (spriteRenderer == null) return;
		// возвращаем исходное направление
		spriteRenderer.flipX = !startFacingRight;
	}

	private void OnDestroy()
	{
		if (detection != null)
		{
			detection.OnPlayerDetected.RemoveListener(OnPlayerDetected);
			detection.OnPlayerLost.RemoveListener(OnPlayerLost);
		}
	}
}