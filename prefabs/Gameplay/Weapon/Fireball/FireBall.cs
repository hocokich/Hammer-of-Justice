using UnityEngine;

public class Fireball : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float speed = 8f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 3f;
	[SerializeField] private LayerMask hitLayers;

	private Rigidbody2D rb;
	private float direction;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, lifetime);
	}

	public void SetDirection(float dir)
	{
		direction = dir;
		if (dir < 0)
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(direction * speed, 0);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Проверяем, входит ли объект в нужные слои
		if (((1 << other.gameObject.layer) & hitLayers) != 0)
		{
			Health health = other.GetComponentInParent<Health>();
			if (health != null)
			{
				health.TakeDamage(damage);
				CameraShake.Instance?.ShakeHit();
				Destroy(gameObject);   // Уничтожаем только при успешном попадании
			}
			else Destroy(gameObject);

		}
	}
}