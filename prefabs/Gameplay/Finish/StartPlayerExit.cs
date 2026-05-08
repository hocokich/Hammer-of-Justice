using UnityEngine;
using System.Collections;

public class StartPlayerExit : MonoBehaviour
{
	[SerializeField] private float walkSpeed = 4f;
	[SerializeField] private float exitDistance = 10f; // на сколько уйти вправо

	public void StartExit()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if (player == null) return;

		// Фиксируем Rigidbody, чтобы двигать Transform
		var rb = player.GetComponent<Rigidbody2D>();
		if (rb)
		{
			rb.linearVelocity = Vector2.zero;
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
			rb.gravityScale = 0;
		}

		// Поворачиваем вправо
		Vector3 scale = player.transform.localScale;
		scale.x = Mathf.Abs(scale.x);
		player.transform.localScale = scale;

		// Запускаем анимацию бега
		var animator = player.GetComponent<Animator>();
		if (animator) animator.SetFloat("Speed", 1);

		// Стартуем движение
		StartCoroutine(WalkOff(player));
	}

	private IEnumerator WalkOff(GameObject player)
	{
		float startX = player.transform.position.x;
		while (player.transform.position.x < startX + exitDistance)
		{
			player.transform.Translate(Vector3.right * walkSpeed * Time.deltaTime);
			yield return null;
		}

		LevelManager.Instance.CompleteLevel();
	}
}