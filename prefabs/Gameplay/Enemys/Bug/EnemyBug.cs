using UnityEngine;

public class EnemyBug : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float moveDistance = 3f; // Дистанция движения вверх-вниз

	private Health health;
	private Animator animator;
	private Vector3 startPos;

	private void Start()
	{
		health = GetComponent<Health>();
		animator = GetComponent<Animator>();
		startPos = transform.position;

		if (health != null)
			health.OnDeath += Die;
	}

	private void FixedUpdate()
	{
		if (animator == null) return;

		// Получаем прогресс зацикленной анимации
		float t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

		// Превращаем в движение по синусоиде: 0 = верх, 0.5 = низ, 1 = верх
		float yOffset = Mathf.Sin(t * Mathf.PI * 2) * moveDistance * 0.5f;

		transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
	}

	private void Die()
	{
		if (animator != null) animator.enabled = false;
		Destroy(gameObject);
	}
}