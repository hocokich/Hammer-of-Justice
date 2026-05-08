using UnityEngine;

public class DropThroughPlatform : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float dropDuration = 0.3f;  // время, на которое отключается коллизия
	[SerializeField] private LayerMask groundLayer;       // слой, на котором находятся платформы

	private Collider2D objectCollider;
	private Rigidbody2D objectRb;

	private void Awake()
	{
		objectCollider = GetComponent<Collider2D>();
		objectRb = GetComponent<Rigidbody2D>();
	}

	/// <summary> Провалиться сквозь платформу (вызывать из SwipeDetector или AI). </summary>
	public void PerformDrop()
	{
		// Ищем платформу под ногами
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
		if (hit.collider == null) return;

		PlatformEffector2D effector = hit.collider.GetComponent<PlatformEffector2D>();
		if (effector == null) return;   // это не полупроницаемая платформа

		// Временно отключаем коллизию между объектом и платформой
		Physics2D.IgnoreCollision(objectCollider, hit.collider, true);
		// Включаем обратно через dropDuration секунд
		StartCoroutine(RestoreCollision(hit.collider));
	}

	private System.Collections.IEnumerator RestoreCollision(Collider2D platformCollider)
	{
		yield return new WaitForSeconds(dropDuration);
		if (platformCollider != null)
			Physics2D.IgnoreCollision(objectCollider, platformCollider, false);
	}

	// Для тестов в редакторе (работает только на игроке)
	private void Update()
	{
#if UNITY_EDITOR
		if (gameObject.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				PerformDrop();
			}
		}
#endif
	}
}