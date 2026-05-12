using UnityEngine;

public class EnemyHorizontalMovement : MonoBehaviour
{
	[Header("Движение")]
	[SerializeField] private float moveSpeed = 2f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private SpriteRenderer spriteRenderer;

	[Header("Проверка препятствий")]
	[SerializeField] private float wallCheckDistance = 0.6f;
	[SerializeField] private float groundCheckDistance = 0.5f;
	[SerializeField] private float groundCheckOffset = 0.5f;

	[Header("Слои для проверки")]
	[SerializeField] private LayerMask groundLayer;   // для земли / платформ
	[SerializeField] private LayerMask wallLayer;     // для стен

	private Rigidbody2D rb;
	private bool movingRight = true;
	private bool canMove = true;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		if (spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (!canMove) return;

		if (ShouldTurn()) Flip();

		float direction = movingRight ? 1f : -1f;
		rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
	}

	private bool ShouldTurn()
	{
		// Проверка края платформы (земля)
		Vector2 origin = groundCheck.position;
		Vector2 groundCheckPos = origin + new Vector2(movingRight ? groundCheckOffset : -groundCheckOffset, 0);
		RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos, Vector2.down, groundCheckDistance, groundLayer);

		// Проверка стены (отдельный слой)
		Vector2 wallOrigin = transform.position;
		Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;
		RaycastHit2D wallInfo = Physics2D.Raycast(wallOrigin, wallDirection, wallCheckDistance, wallLayer);

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

	public void StopMovement()
	{
		canMove = false;
		rb.linearVelocity = Vector2.zero;
	}

	public void ResumeMovement() => canMove = true;

	private void OnDrawGizmosSelected()
	{
		if (groundCheck == null) return;

		// Луч проверки земли
		Gizmos.color = Color.green;
		Vector2 origin = groundCheck.position;
		Vector2 groundCheckPos = origin + new Vector2(movingRight ? groundCheckOffset : -groundCheckOffset, 0);
		Gizmos.DrawRay(groundCheckPos, Vector2.down * groundCheckDistance);

		// Луч проверки стены
		Gizmos.color = Color.blue;
		Vector2 wallOrigin = transform.position;
		Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;
		Gizmos.DrawRay(wallOrigin, wallDirection * wallCheckDistance);
	}
}