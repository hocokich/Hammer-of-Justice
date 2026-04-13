using UnityEngine;

public class motion : MonoBehaviour
{
	[Header("Настройки движения")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float groundCheckDistance = 0.2f;
	[SerializeField] private LayerMask groundLayer;

	[Header("Физика")]
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Transform groundCheckPoint;

	private float horizontalInput;
	private bool isGrounded;

	void Awake()
	{
		// Автоматически получаем Rigidbody2D если не установлен
		if (rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}

		// Создаем точку проверки земли если её нет
		if (groundCheckPoint == null)
		{
			GameObject checkPoint = new GameObject("GroundCheck");
			checkPoint.transform.SetParent(transform);
			checkPoint.transform.localPosition = new Vector3(0, -0.5f, 0);
			groundCheckPoint = checkPoint.transform;
		}
	}

	void Update()
	{
		// Получаем ввод
		horizontalInput = 0f;

		if (Input.GetKey(KeyCode.A))
		{
			horizontalInput = -1f;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			horizontalInput = 1f;
		}

		// Проверка прыжка
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
		}
	}

	void FixedUpdate()
	{
		// Проверяем землю
		CheckGround();

		// Применяем движение через Rigidbody2D
		MoveWithRigidbody();
	}

	void CheckGround()
	{
		isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
	}

	void MoveWithRigidbody()
	{
		if (Mathf.Abs(horizontalInput) > 0.1f)
		{
			Vector2 LinearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
			rb.linearVelocity = LinearVelocity;
		}
		else
		{
			// Останавливаем горизонтальное движение при отсутствии ввода
			rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
		}
	}
}