using System.Collections;
using UnityEngine;

public class StraightFireball : MonoBehaviour
{
	[SerializeField] private float speed = 4f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 5f;
	[SerializeField] private LayerMask hitLayers;
	[SerializeField] private Animator animator;

	private Rigidbody2D rb;
	private Vector2 moveDirection;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void Start()=> StartCoroutine(DelayDestroy());

	/// <summary> «апомнить позицию игрока в момент выстрела и задать траекторию. </summary>
	public void SetTargetPosition(Vector3 targetPosition)
	{
		Vector3 direction = (targetPosition - transform.position).normalized;
		SetDirectionVector(direction);
	}

	/// <summary> «адать направление вручную (вектор). </summary>
	public void SetDirectionVector(Vector2 direction)
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();   // подстраховка

		moveDirection = direction.normalized;
		if (rb != null)
		{
			rb.linearVelocity = moveDirection * speed;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
	}

	/// <summary> «адать направление по знаку: >0 Ц вправо, <=0 Ц влево. </summary>
	public void SetDirection(float dir)
	{
		moveDirection = dir > 0 ? Vector2.right : Vector2.left;
		rb.linearVelocity = moveDirection * speed;
		transform.rotation = Quaternion.Euler(0f, 0f, dir > 0 ? 0f : 180f);
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
			}
			// ќстанавливаем физику и запускаем анимацию разрушени€
			rb.simulated = false;
			animator.SetTrigger("destroy");
		}
	}

	// Animation Event: вызываетс€ в конце анимации destroy
	public void OnDestroy()
	{
		Destroy(gameObject);
	}

	IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(lifetime);
		// ≈сли ещЄ не уничтожен при столкновении Ц запускаем анимацию
		if (gameObject)
		{
			animator.SetTrigger("destroy");
		}
	}
}