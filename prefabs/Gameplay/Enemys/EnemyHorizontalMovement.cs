using UnityEngine;

public class EnemyHorizontalMovement : MonoBehaviour
{
	[Header("Движение")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private SpriteRenderer spriteRenderer;

	private Rigidbody2D rb;
	private bool movingRight = true;
	private bool canMove = true; // ← нужно для Stop/Resume

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		if (spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!canMove) return; // ← если движение остановлено (атака), ничего не делаем

		if (ShouldTurn()) Flip();

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
		if (spriteRenderer) spriteRenderer.flipX = !spriteRenderer.flipX;

		// смещаем точки атаки/обнаружения при повороте
		Transform attackPoint = transform.Find("AttackPoint");
		if (attackPoint) attackPoint.localPosition = new Vector3(-attackPoint.localPosition.x, attackPoint.localPosition.y, attackPoint.localPosition.z);

		Transform detectionZone = transform.Find("DetectionZone");
		if (detectionZone) detectionZone.localPosition = new Vector3(-detectionZone.localPosition.x, detectionZone.localPosition.y, detectionZone.localPosition.z);
	}

	public void StopMovement() => canMove = false;
	public void ResumeMovement() => canMove = true;
}