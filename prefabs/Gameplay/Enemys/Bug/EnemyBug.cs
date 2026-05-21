using UnityEngine;

public class EnemyBug : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float moveDistance = 3f;

	private Health health;
	private Animator animator;
	private Rigidbody2D rb;
	private Vector3 startPos;
	private float animTime;
	private bool isDead;

	private void Start()
	{
		health = GetComponent<Health>();
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		startPos = transform.position;

		// Отключаем гравитацию
		rb.gravityScale = 0;
		rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;

		if (health != null)
			health.OnDeath += Die;
	}

	private void FixedUpdate()
	{
		if (animator == null || isDead) return;

		// Плавно считаем время анимации
		animTime += Time.fixedDeltaTime;
		float cycleTime = animator.GetCurrentAnimatorStateInfo(0).length;
		float t = (animTime % cycleTime) / cycleTime;

		float yOffset = Mathf.Sin(t * Mathf.PI * 2) * moveDistance * 0.5f;

		Vector2 targetPos = new Vector2(startPos.x, startPos.y + yOffset);
		rb.MovePosition(targetPos);
	}

	private void Die()
	{
		isDead = true;

		Transform dt = transform.Find("damageTrigger");
		if (dt != null) dt.gameObject.SetActive(false);

		HealthBar hb = GetComponentInChildren<HealthBar>();
		if (hb != null) hb.gameObject.SetActive(false);

		animator.SetTrigger("die");
	}

	public void OnDie()
	{
		Destroy(gameObject);
	}
}