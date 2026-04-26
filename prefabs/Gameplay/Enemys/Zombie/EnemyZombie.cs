using UnityEngine;

public class EnemyZombie : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private int damage = 1;

	private Health health;
	private EnemyHorizontalMovement movement;

	private void Start()
	{
		health = GetComponent<Health>();
		movement = GetComponent<EnemyHorizontalMovement>();

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