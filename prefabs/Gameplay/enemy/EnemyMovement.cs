using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundLayer;

	[Header("Проверка препятствий")]
	[SerializeField] private float wallCheckDistance = 0.6f;
	[SerializeField] private float groundCheckDistance = 0.8f;
	[SerializeField] private float groundCheckOffset = 0.8f;

	private Rigidbody2D rb;
	private bool movingRight = true;
	private bool canMove = true;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (!canMove) return;

		if (ShouldTurn())
		{
			Flip();
		}

		float direction = movingRight ? 1f : -1f;
		rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
	}

	private bool ShouldTurn()
	{
		// Проверка края платформы
		Vector2 groundCheckPos = groundCheck.position + new Vector3(movingRight ? groundCheckOffset : -groundCheckOffset, 0, 0);
		RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos, Vector2.down, groundCheckDistance, groundLayer);
		bool noGround = !groundInfo;

		// Проверка стены
		Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;
		RaycastHit2D wallInfo = Physics2D.Raycast(transform.position, wallDirection, wallCheckDistance, groundLayer);
		bool wallAhead = wallInfo && wallInfo.collider != null;

		// Дебаг (можно закомментировать)
		Debug.DrawRay(groundCheckPos, Vector2.down * groundCheckDistance, noGround ? Color.red : Color.green);
		Debug.DrawRay(transform.position, wallDirection * wallCheckDistance, wallAhead ? Color.red : Color.blue);

		return noGround || wallAhead;
	}

	private void Flip()
	{
		movingRight = !movingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void StopMovement() => canMove = false;
	public void ResumeMovement() => canMove = true;
}