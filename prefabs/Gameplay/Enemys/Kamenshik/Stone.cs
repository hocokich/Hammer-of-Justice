using UnityEngine;

public class Stone : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private int damage = 1;
	[SerializeField] private float explosionRadius = 2f;
	[SerializeField] private LayerMask hitLayers;
	[SerializeField] private GameObject explosionEffect; // Опционально

	private bool exploded = false;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (exploded) return;
		Explode();
	}

	private void Explode()
	{
		exploded = true;

		// Наносим урон всем в радиусе
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, hitLayers);

		foreach (Collider2D hit in hits)
		{
			Health health = hit.GetComponent<Health>();
			if (health == null)
				health = hit.GetComponentInParent<Health>();

			if (health != null)
			{
				health.TakeDamage(damage);
			}
		}

		// Тряска камеры
		//CameraShake.Instance?.ShakeHit();

		// Эффект взрыва
		if (explosionEffect != null)
		{
			Instantiate(explosionEffect, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}