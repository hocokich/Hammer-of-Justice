using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
	[Header("Настройки атаки")]
	[SerializeField] private int damage = 1;
	[SerializeField] private float attackCooldown = 0.3f;
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackRange = 2f;
	[SerializeField] private LayerMask enemyLayer;

	private float lastAttackTime = -999f;

	private void Update()
	{
		if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
		{
			PerformAttack();
		}
	}

	private void PerformAttack()
	{
		lastAttackTime = Time.time;

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

		// проверка попадания
		if (hitEnemies.Length > 0)
		{
			CameraShake.Instance?.ShakeHit();
		}

		foreach (Collider2D enemy in hitEnemies)
		{
			Health enemyHealth = enemy.GetComponent<Health>();
			if (enemyHealth != null)
			{
				enemyHealth.TakeDamage(damage);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}