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
	[SerializeField] protected Animator animator;

	protected bool isAttacking;

	public bool IsAttacking => isAttacking;

	private void Start()
	{
		if (animator == null) animator = GetComponent<Animator>();
	}

	/// <summary> Вызвать извне для начала атаки. </summary>
	public virtual void OnAttack()
	{
		if (isAttacking) return;
		isAttacking = true;
		animator?.SetTrigger("Attack");
		StartCoroutine(ResetAttackAfterAnimation());
	}

	private IEnumerator ResetAttackAfterAnimation()
	{
		yield return null;
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

		Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRange, enemyLayer);
		if (hits.Length > 0)
			CameraShake.Instance?.ShakeHit();

		foreach (Collider2D hit in hits)
			hit.GetComponent<Health>()?.TakeDamage(damage);
	}

	// Визуализация радиуса атаки в редакторе
	private void OnDrawGizmosSelected()
	{
		if (attackPoint == null) return;
		float dir = transform.localScale.x > 0 ? 1f : -1f;
		Vector3 offset = new Vector3(attackOffset.x * dir, attackOffset.y, 0f);
		Vector3 center = attackPoint.position + offset;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(center, attackRange);
	}
}