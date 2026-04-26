using UnityEngine;

public class EnemySpider : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private float dropDistance = 3f;
	[SerializeField] private AnimationCurve springCurve; // ← Кривая пружины

	private Health health;
	private Animator animator;
	private Vector3 startPos;
	private float animTime;

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

		// Читаем время из анимации
		animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

		// Применяем кривую
		float yOffset = springCurve.Evaluate(animTime) * dropDistance;
		transform.position = new Vector3(startPos.x, startPos.y - yOffset, startPos.z);
	}

	private void Die()
	{
		if (animator != null) animator.enabled = false;
		Destroy(gameObject);
	}
}