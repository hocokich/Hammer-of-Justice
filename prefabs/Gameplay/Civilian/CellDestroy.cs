using UnityEngine;

public class CellDestroy : MonoBehaviour
{
	private Health health;

	private void Start()
	{
		health = GetComponent<Health>();

		if (health != null)
		{
			health.OnDeath += Destroy;
		}
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}
