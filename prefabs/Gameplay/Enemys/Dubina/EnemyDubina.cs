using UnityEngine;
using System.Collections;

public class EnemyDubina : MonoBehaviour
{
	[SerializeField] private EnemyHorizontalMovement movement;
	[SerializeField] private MeleeAttack meleeAttack;
	[SerializeField] private EnemyDetection detection;
	[SerializeField] private float attackCooldown = 1.5f;

	private bool playerInRange;
	private Coroutine attackLoop;
	private Health health;

	private void Start()
	{
		movement ??= GetComponent<EnemyHorizontalMovement>();
		meleeAttack ??= GetComponent<MeleeAttack>();
		detection ??= GetComponentInChildren<EnemyDetection>();
		health = GetComponent<Health>();

		if (detection != null)
		{
			detection.OnPlayerDetected.AddListener(OnPlayerDetected);
			detection.OnPlayerLost.AddListener(OnPlayerLost);
		}

		if (health != null)
			health.OnDeath += Die;
	}

	private void OnPlayerDetected()
	{
		playerInRange = true;
		if (attackLoop == null)
			attackLoop = StartCoroutine(AttackLoop());
	}

	private void OnPlayerLost()
	{
		playerInRange = false;
	}

	private IEnumerator AttackLoop()
	{
		movement.StopMovement();

		while (playerInRange && meleeAttack != null)
		{
			yield return new WaitWhile(() => meleeAttack.IsAttacking);
			if (!playerInRange) break;

			yield return new WaitForSeconds(attackCooldown);
			if (!playerInRange) break;

			meleeAttack.OnAttack();
		}

		movement.ResumeMovement();
		attackLoop = null;
	}

	private void Die()
	{
		if (attackLoop != null)
			StopCoroutine(attackLoop);
		movement.StopMovement();

		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (health != null)
			health.OnDeath -= Die;
	}
}