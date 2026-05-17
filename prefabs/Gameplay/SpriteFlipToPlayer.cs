using UnityEngine;

public class SpriteFlipToPlayer : MonoBehaviour
{
	[SerializeField] private Detection detection;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private bool startFacingRight = true;

	private bool playerInRange;

	private void Start()
	{
		if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
		if (detection == null) detection = GetComponentInChildren<Detection>();

		if (detection != null)
		{
			detection.OnPlayerDetected.AddListener(() => playerInRange = true);
			detection.OnPlayerLost.AddListener(() =>
			{
				playerInRange = false;
				spriteRenderer.flipX = !startFacingRight;
			});
		}
	}

	private void Update()
	{
		if (!playerInRange || spriteRenderer == null) return;
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
			spriteRenderer.flipX = player.transform.position.x < transform.position.x;
	}

	private void OnDestroy()
	{
		if (detection != null)
		{
			detection.OnPlayerDetected.RemoveAllListeners();
			detection.OnPlayerLost.RemoveAllListeners();
		}
	}
}