using UnityEngine;

public class motion : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float groundCheckDistance = 0.2f;
	[SerializeField] private float sideCheckOffset = 0.3f;

	[Header("Прыжки")]
	[SerializeField] private int maxJumps = 2;

	[Header("Физика")]
	[SerializeField] private Rigidbody2D rb;

	[Header("Проверка земли")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private LayerMask groundLayer;

	[Header("Анимация")]
	[SerializeField] private Animator animator;

	private float horizontalInput;
	private bool isGrounded;
	private bool wasGrounded;
	private bool facingRight = true;
	private int jumpsRemaining;
	private float lastJumpTime = -999f;
	private float jumpCooldown = 0.1f;

	void Awake()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
		if (groundCheckPoint == null) CreateGroundCheck();
		if (animator == null) animator = GetComponent<Animator>();
		jumpsRemaining = maxJumps;
	}

	void CreateGroundCheck()
	{
		GameObject checkPoint = new GameObject("GroundCheck");
		checkPoint.transform.SetParent(transform);
		checkPoint.transform.localPosition = new Vector3(0, -0.5f, 0);
		groundCheckPoint = checkPoint.transform;
	}

	void Update()
	{
		horizontalInput = 0f;
		if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
		else if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

		FlipSprite();

		// Прыжок
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastJumpTime + jumpCooldown)
		{
			if (isGrounded)
			{
				jumpsRemaining = maxJumps - 1;
				Jump();
			}
			else if (jumpsRemaining > 0)
			{
				jumpsRemaining--;
				Jump();
			}
		}

		// Анимационные параметры
		if (animator != null)
		{
			animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
			animator.SetBool("IsGrounded", isGrounded);
			animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
		}
	}

	void FixedUpdate()
	{
		CheckGround();
		if (isGrounded && !wasGrounded)
		{
			jumpsRemaining = maxJumps;
		}
		wasGrounded = isGrounded;
		MoveWithRigidbody();
	}

	void CheckGround()
	{
		bool centerHit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
		Vector2 leftPos = groundCheckPoint.position + Vector3.left * sideCheckOffset;
		Vector2 rightPos = groundCheckPoint.position + Vector3.right * sideCheckOffset;
		bool leftHit = Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, groundLayer);
		bool rightHit = Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, groundLayer);
		isGrounded = centerHit || leftHit || rightHit;
	}

	void Jump()
	{
		lastJumpTime = Time.time;
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
		if (animator != null) animator.SetTrigger("Jump");
	}

	void MoveWithRigidbody()
	{
		if (Mathf.Abs(horizontalInput) > 0.1f)
			rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
		else
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
	}

	void FlipSprite()
	{
		if (horizontalInput > 0 && !facingRight) Flip();
		else if (horizontalInput < 0 && facingRight) Flip();
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}