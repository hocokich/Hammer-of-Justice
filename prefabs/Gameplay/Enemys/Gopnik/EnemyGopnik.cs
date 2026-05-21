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