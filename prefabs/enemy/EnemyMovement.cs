using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundLayer;

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
		Vector2 origin = groundCheck.position;
		Vector2 groundCheckPos = origin + new Vector2(movingRight ? 0.5f : -0.5f, 0);
		RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos, Vector2.down, 0.5f, groundLayer);

		Vector2 wallOrigin = transform.position;
		Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;
		RaycastHit2D wallInfo = Physics2D.Raycast(wallOrigin, wallDirection, 0.6f, groundLayer);

		return !groundInfo || wallInfo;
	}

	private void Flip()
	{
		movingRight = !movingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void StopMovement()
	{
		canMove = false;
		rb.linearVelocity = Vector2.zero;
	}

	public void ResumeMovement()
	{
		canMove = true;
	}
}