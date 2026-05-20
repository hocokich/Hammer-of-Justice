using UnityEngine;

public class Cage : MonoBehaviour
{
	private Health health;
	private Civilian civilian;
	private SpriteFlash spriteFlash;

	private void Start()
	{
		civilian = GetComponentInParent<Civilian>();
		if (civilian != null && civilian.isRescued)
			Destroy();

		spriteFlash = GetComponent<SpriteFlash>();

		if (TryGetComponent<Health>(out health))
		{
			health.OnDeath += Destroy;
		}
	}

	public void Destroy()
	{
		civilian?.Rescue();
		Destroy(gameObject);
	}
}