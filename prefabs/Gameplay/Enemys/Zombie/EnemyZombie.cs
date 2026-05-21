using UnityEngine;

public class EnemyZombie : MonoBehaviour
{
	[Header("Настройки")]
	private Health health;
	private EnemyHorizontalMovement movement;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		health = GetComponent<Health>();
		movement = GetComponent<EnemyHorizontalMovement>();

		if (health != null)
			health.OnDeath += Die;
	}

	private void Die()
	{
		if (movement != null)
			movement.StopMovement();

		Transform dt = transform.Find("damageTrigger");
		if (dt != null) dt.gameObject.SetActive(false);

		HealthBar hb = GetComponentInChildren<HealthBar>();
		if (hb != null) hb.gameObject.SetActive(false);

		animator.SetTrigger("die");
	}
	public void OnDie()
	{
		Destroy(gameObject);
	}
}