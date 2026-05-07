using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
	[Header("Настройки атаки")]
	[SerializeField] private int damage = 1;
	[SerializeField] private Transform attackPoint;
	[SerializeField] private float attackRange = 2f;
	[SerializeField] private Vector2 attackOffset = Vector2.zero;
	[SerializeField] private LayerMask enemyLayer;

	[Header("Анимация")]
	[SerializeField] private Animator animator;

	private bool isAttacking;

	private void Start()
	{
		if (animator == null) animator = GetComponent<Animator>();
	}

	private void Update()
	{
		#if UNITY_EDITOR
		if (Input.GetButtonDown("Fire1") && !isAttacking)
		{
			isAttacking = true;
			animator?.SetTrigger("Attack");
			StartCoroutine(ResetAttackAfterAnimation());
		}
		#endif
	}

	public void OnAttack()
	{
		if (isAttacking) return;
		isAttacking = true;
		animator?.SetTrigger("Attack");
		StartCoroutine(ResetAttackAfterAnimation());
	}

	private IEnumerator ResetAttackAfterAnimation()
	{
		// Ждём один кадр, чтобы аниматор перешёл в состояние Attack
		yield return null;

		// Получаем длину текущей анимации (атаки)
		float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
		yield return new WaitForSeconds(animLength);

		isAttacking = false;
	}

	// Animation Event: момент нанесения урона
	public void DealDamage()
	{
		float dir = Mathf.Sign(transform.localScale.x);
		Vector3 offset = new Vector3(attackOffset.x * dir, attackOffset.y, 0f);
		Vector3 center = attackPoint.position + offset;

		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(center, attackRange, enemyLayer);
		if (hitEnemies.Length > 0)
			CameraShake.Instance?.ShakeHit();

		foreach (Collider2D enemy in hitEnemies)
			enemy.GetComponent<Health>()?.TakeDamage(damage);
	}
}