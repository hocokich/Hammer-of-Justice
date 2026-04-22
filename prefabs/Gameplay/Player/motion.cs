using UnityEngine;

public class motion : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float groundCheckDistance = 0.3f;
	[SerializeField] private float sideCheckOffset = 0.3f; // Смещение для боковых лучей

	[Header("Прыжки")]
	[SerializeField] private int maxJumps = 2;

	[Header("Физика")]
	[SerializeField] private Rigidbody2D rb;

	[Header("Проверка земли")]
	[SerializeField] private Transform groundCheckPoint;
	[SerializeField] private LayerMask groundLayer;

	private float horizontalInput;
	private bool isGrounded;
	private bool wasGrounded; // ← Для отслеживания момента приземления
	private bool facingRight = true;
	private int jumpsRemaining;

	// Защита от бесконечных прыжков
	private float lastJumpTime = -999f;
	private float jumpCooldown = 0.1f;

	void Awake()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
		if (groundCheckPoint == null) CreateGroundCheck();

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

		// Прыжок с защитой от спама
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
	}

	void FixedUpdate()
	{
		CheckGround();

		// Сбрасываем прыжки ТОЛЬКО в момент касания земли (не каждый кадр)
		if (isGrounded && !wasGrounded)
		{
			jumpsRemaining = maxJumps;
		}
		wasGrounded = isGrounded;

		MoveWithRigidbody();
	}

	void CheckGround()
	{
		// Центральный луч
		bool centerHit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);

		// Левый луч
		Vector2 leftPos = groundCheckPoint.position + Vector3.left * sideCheckOffset;
		bool leftHit = Physics2D.Raycast(leftPos, Vector2.down, groundCheckDistance, groundLayer);

		// Правый луч
		Vector2 rightPos = groundCheckPoint.position + Vector3.right * sideCheckOffset;
		bool rightHit = Physics2D.Raycast(rightPos, Vector2.down, groundCheckDistance, groundLayer);

		isGrounded = centerHit || leftHit || rightHit;

		// Дебаг
		Debug.DrawRay(groundCheckPoint.position, Vector2.down * groundCheckDistance, centerHit ? Color.green : Color.red);
		Debug.DrawRay(leftPos, Vector2.down * groundCheckDistance, leftHit ? Color.green : Color.blue);
		Debug.DrawRay(rightPos, Vector2.down * groundCheckDistance, rightHit ? Color.blue : Color.red);
	}

	void Jump()
	{
		lastJumpTime = Time.time;
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
	}

	void MoveWithRigidbody()
	{
		if (Mathf.Abs(horizontalInput) > 0.1f)
		{
			rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
		}
		else
		{
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
		}
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