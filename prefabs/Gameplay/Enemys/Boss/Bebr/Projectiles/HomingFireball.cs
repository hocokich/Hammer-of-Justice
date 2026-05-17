using UnityEngine;

public class HomingFireball : MonoBehaviour
{
	[SerializeField] private float speed = 4f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 5f;
	[SerializeField] private LayerMask hitLayers;

	private Transform target;
	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, lifetime);
	}

	public void SetTarget(Transform t) => target = t;

	private void FixedUpdate()
	{
		if (target)
		{
			Vector3 dir = (target.position - transform.position).normalized;
			rb.linearVelocity = dir * speed;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & hitLayers) != 0)
		{
			Health h = other.GetComponentInParent<Health>();
			if (h)
			{
				h.TakeDamage(damage);
				CameraShake.Instance?.ShakeHit();
				Destroy(gameObject);
			}
		}
	}
}