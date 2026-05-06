using UnityEngine;
using System.Collections;

public class RangeAttack : MonoBehaviour
{
	[Header("Настройки")]
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private int manaCost = 1;

	[Header("Анимация")]
	[SerializeField] private Animator animator;
	[SerializeField] private float shootAnimDuration = 0.6f;   // точная длина анимации выстрела

	private Mana mana;
	private bool isShooting;

	private void Start()
	{
		mana = GetComponent<Mana>();
		if (animator == null) animator = GetComponent<Animator>();
	}

	// Вызывается из Input System (Send Messages)
	public void OnShoot()
	{
		if (isShooting) return;
		if (mana == null || mana.UseMana(manaCost))
		{
			isShooting = true;
			animator?.SetTrigger("Shoot");
			StartCoroutine(ResetShoot());
		}
	}

	private IEnumerator ResetShoot()
	{
		// Получаем длину текущей анимации (атаки)
		float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
		yield return new WaitForSeconds(animLength);

		isShooting = false;
	}

	// Animation Event: вызов в нужный момент анимации
	public void ShootFireball()
	{
		float direction = transform.localScale.x > 0 ? 1f : -1f;
		GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, Quaternion.identity);
		fireball.GetComponent<Fireball>()?.SetDirection(direction);
	}
}