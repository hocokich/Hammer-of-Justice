using UnityEngine;

public class CellDestroy : MonoBehaviour
{
	private Health health;
	private Civilian ñivilian;

	private void Start()
	{
		ñivilian = GetComponentInParent<Civilian>();

		if (ñivilian != null && ñivilian.isRescued)
			Destroy();

		if (TryGetComponent<Health>(out health))
			health.OnDeath += Destroy;
	}

	public void Destroy()
	{
		ñivilian.Rescue();
		Destroy(gameObject);
		return;
	}
}
