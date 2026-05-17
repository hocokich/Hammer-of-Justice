using UnityEngine;

public class BossFlipToPlayer : MonoBehaviour
{
	[SerializeField] private Detection detection;
	[SerializeField] private bool startFacingRight = true;

	private bool playerInRange;

	private void Start()
	{
		if (detection == null) detection = GetComponentInChildren<Detection>();

		if (detection != null)
		{
			detection.OnPlayerDetected.AddListener(() => playerInRange = true);
			detection.OnPlayerLost.AddListener(() =>
			{
				playerInRange = false;
				// Возвращаем исходную ориентацию
				float absX = Mathf.Abs(transform.localScale.x);
				transform.localScale = new Vector3(startFacingRight ? absX : -absX, transform.localScale.y, transform.localScale.z);
			});
		}
	}

	private void Update()
	{
		if (!playerInRange) return;
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			float dir = player.transform.position.x < transform.position.x ? -1f : 1f;
			float absX = Mathf.Abs(transform.localScale.x);
			transform.localScale = new Vector3(dir * absX, transform.localScale.y, transform.localScale.z);
		}
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