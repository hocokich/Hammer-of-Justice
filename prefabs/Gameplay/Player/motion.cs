using UnityEngine;

public class motion : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float sideCheckOffset = 0.3f;
	[SerializeField] private float groundCheckDistance = 0.2f;

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
	private bool isGrounded, wasGrounded;
	private bool facingRight = true;
	private int jumpsRemaining;
	private float lastJumpTime = -999f;
	private float jumpCooldown = 0.1f;

	// Перемещение по кнопкам
	private bool leftPressed, rightPressed;

	// Эти методы будут вызываться из скрипта кнопок
	public void OnLeftDown() => leftPressed = true;
	public void OnLeftUp() => leftPressed = false;
	public void OnRightDown() => rightPressed = true;
	public void OnRightUp() => rightPressed = false;

	private void Awake()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
		if (groundCheckPoint == null) CreateGroundCheck();
		if (animator == null) animator = GetComponent<Animator>();
		jumpsRemaining = maxJumps;
	}

	private void CreateGroundCheck()
	{
		GameObject checkPoint = new GameObject("GroundCheck");
		checkPoint.transform.SetParent(transform);
		checkPoint.transform.localPosition = new Vector3(0, -0.5f, 0);
		groundCheckPoint = checkPoint.transform;
	}

	private void Update()
	{
		// Вместо или перед тем, как ты сейчас присваиваешь horizontalInput
		float uiInput = 0f;
		if (leftPressed) uiInput -= 1f;
		if (rightPressed) uiInput += 1f;

		// Приоритет UI-кнопок: если они нажаты – используем их, иначе клавиатуру
		horizontalInput = Mathf.Abs(uiInput) > 0.01f ? uiInput : Input.GetAxisRaw("Horizontal");

		// --- Разворот спрайта ---
		if (horizontalInput > 0 && !facingRight) Flip();
		else if (horizontalInput < 0 && facingRight) Flip();

		// --- Прыжок ---
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

		// --- Анимации ---
		if (animator)
		{
			animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
			animator.SetBool("IsGrounded", isGrounded);
			animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
		}
	}

	private void FixedUpdate()
	{
		CheckGround();
		if (isGrounded && !wasGrounded) jumpsRemaining = maxJumps;
		wasGrounded = isGrounded;
		MoveWithRigidbody();
	}

	private void CheckGround()
	{
		Vector2 origin = groundCheckPoint.position;
		bool c = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
		bool l = Physics2D.Raycast(origin + Vector2.left * sideCheckOffset, Vector2.down, groundCheckDistance, groundLayer);
		bool r = Physics2D.Raycast(origin + Vector2.right * sideCheckOffset, Vector2.down, groundCheckDistance, groundLayer);
		isGrounded = c || l || r;
	}

	public void OnJump()
	{
		if (Time.time > lastJumpTime + jumpCooldown)
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
	}
	private void Jump()
	{
		lastJumpTime = Time.time;
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
		if (animator) animator.SetTrigger("Jump");
	}

	private void MoveWithRigidbody()
	{
		if (Mathf.Abs(horizontalInput) > 0.1f)
			rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
		else
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}