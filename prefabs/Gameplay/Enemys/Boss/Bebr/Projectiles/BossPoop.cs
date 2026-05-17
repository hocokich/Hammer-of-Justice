using UnityEngine;

public class BossPoop : MonoBehaviour
{
	[SerializeField] private float fallSpeed = 5f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 5f;
	[SerializeField] private LayerMask hitLayers;

	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = new Vector2(0f, -fallSpeed);
		Destroy(gameObject, lifetime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & hitLayers) != 0)
		{
			Health h = other.GetComponentInParent<Health>();
			if (h)
			{
				h.TakeDamage(damage);
				Destroy(gameObject);
			}
		}
	}
}