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
	private bool isDead;

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
		if (animator == null || isDead) return;

		// Читаем время из анимации
		animTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;

		// Применяем кривую
		float yOffset = springCurve.Evaluate(animTime) * dropDistance;
		transform.position = new Vector3(startPos.x, startPos.y - yOffset, startPos.z);
	}

	private void Die()
	{
		isDead = true;

		Transform dt = transform.Find("damageTrigger");
		if (dt != null) dt.gameObject.SetActive(false);

		HealthBar hb = GetComponentInChildren<HealthBar>();
		if (hb != null) hb.gameObject.SetActive(false);

		animator.SetTrigger("die");
	}
	public void OnDie()
	{
		Destroy(gameObject);
	}
}