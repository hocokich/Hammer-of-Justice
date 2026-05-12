using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	private Health health;

	private void Start()
	{
		health = GetComponent<Health>();

		if (health != null)
			health.OnDeath += Die;
	}

	private void Die()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (health != null)
			health.OnDeath -= Die;
	}
}