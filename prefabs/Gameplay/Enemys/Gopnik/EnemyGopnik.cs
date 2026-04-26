using UnityEngine;

public class EnemyGopnik : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private GameObject spitPrefab;
	[SerializeField] private Transform spitPoint;

	private Health health;
	private Animator animator;

	private void Start()
	{
		health = GetComponent<Health>();
		animator = GetComponent<Animator>();

		if (health != null)
			health.OnDeath += Die;
	}

	// Вызывается из Animation Event
	public void Spit()
	{
		GameObject spit = Instantiate(spitPrefab, spitPoint.position, Quaternion.identity);
		Fireball fb = spit.GetComponent<Fireball>();

		if (fb != null)
		{
			float direction = transform.localScale.x > 0 ? 1f : -1f;
			fb.SetDirection(direction);
		}
	}

	private void Die()
	{
		if (animator != null) animator.enabled = false;
		Destroy(gameObject);
	}
}