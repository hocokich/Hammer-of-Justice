using UnityEngine;

public class EnemyKamenshik : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private GameObject stonePrefab;
	[SerializeField] private Transform throwPoint;
	[SerializeField] private float throwForceX = 4f;
	[SerializeField] private float throwForceY = 6f;

	private Health health;
	private Animator animator;

	private void Start()
	{
		health = GetComponent<Health>();
		animator = GetComponent<Animator>();

		if (health != null)
			health.OnDeath += Die;
	}

	// Вызывается из Animation Event
	public void ThrowStone()
	{
		GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);
		Stone stoneScript = stone.GetComponent<Stone>();
		Rigidbody2D stoneRb = stone.GetComponent<Rigidbody2D>();

		if (stoneRb != null)
		{
			float direction = transform.localScale.x > 0 ? 1f : -1f;   // вперёд перед собой
			stoneRb.linearVelocity = new Vector2(throwForceX * direction, throwForceY);
		}
	}

	private void Die()
	{
		if (animator != null) animator.enabled = false;
		Destroy(gameObject);
	}
}