using Unity.VisualScripting;
using UnityEngine;

public class Stone : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private int damage = 1;
	[SerializeField] private float explosionRadius = 2f;
	[SerializeField] private LayerMask hitLayers;

	[Header("Анимация")]
	[SerializeField] private Animator animator;

	private void Awake()
	{
		if (animator == null)
			animator = GetComponent<Animator>();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// Отключаем физику, чтобы не было повторных столкновений
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
			rb.bodyType = RigidbodyType2D.Kinematic;
		}

		Collider2D col = GetComponent<Collider2D>();
		if (col != null) col.enabled = false;

		// Запускаем анимацию взрыва. Вся дальнейшая логика — в Animation Events.
		if (animator != null)
			animator.SetBool("Explode", true);
	}

	// Вызывается из Animation Event в момент нанесения урона
	public void DealDamage()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hitLayers);
		foreach (Collider2D hit in hits)
		{
			Health health = hit.GetComponent<Health>() ?? hit.GetComponentInParent<Health>();
			if (health != null) health.TakeDamage(damage);
		}
	}

	// Вызывается из Animation Event в конце анимации
	public void DestroyStone()
	{
		Destroy(gameObject);
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
#endif
}

