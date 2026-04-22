using UnityEngine;

public class RangeAttack : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private float shootCooldown = 0.4f;
	[SerializeField] private int manaCost = 1;

	private float lastShootTime = -999f;
	private Mana mana;

	private void Start()
	{
		mana = GetComponent<Mana>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire2") && Time.time >= lastShootTime + shootCooldown)
		{
			if (mana == null || mana.UseMana(manaCost))
			{
				ShootFireball();
			}
		}
	}

	private void ShootFireball()
	{
		lastShootTime = Time.time;

		float direction = transform.localScale.x > 0 ? 1f : -1f;

		GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
		Fireball fb = fireball.GetComponent<Fireball>();

		if (fb != null)
		{
			fb.SetDirection(direction);
		}
	}
}