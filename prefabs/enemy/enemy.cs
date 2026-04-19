using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private int damage = 1;

	private Health health;
	private EnemyMovement movement;

	private void Start()
	{
		health = GetComponent<Health>();
		movement = GetComponent<EnemyMovement>();

		if (health != null)
		{
			health.OnDeath += Die;
		}
	}

	private void Die()
	{
		if (movement != null)
		{
			movement.StopMovement();
		}

		Destroy(gameObject);
	}
}