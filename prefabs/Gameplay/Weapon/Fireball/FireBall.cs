using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float speed = 8f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 3f;
	[SerializeField] private LayerMask hitLayers;
	[SerializeField] private Animator animator;

	private Rigidbody2D rb;
	private float direction;

	private void Start()
	{
		animator = GetComponent<Animator>();

		rb = GetComponent<Rigidbody2D>();

		StartCoroutine(DelayDestroy());
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
			Collider2D col = GetComponent<Collider2D>();
			if (col) col.enabled = false;

			Health health = other.GetComponentInParent<Health>();
			if (health != null)
			{
				health.TakeDamage(damage);
				CameraShake.Instance?.ShakeHit();
			}

			animator.SetTrigger("destroy");
		}
	}

	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(lifetime);
		// Если ещё не уничтожен при столкновении – запускаем анимацию
		if (gameObject)
		{
			animator.SetTrigger("destroy");
		}
	}

	public void OnDestroy()
	{
		Destroy(gameObject);
	}
	public void OnStartDestroy()
	{
		Collider col = GetComponent<Collider>();
		if (col != null) col.enabled=false;

		rb.simulated = false;
	}
}