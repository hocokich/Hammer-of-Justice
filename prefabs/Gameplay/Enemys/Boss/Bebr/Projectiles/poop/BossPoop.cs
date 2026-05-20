using UnityEngine;
using System.Collections;

public class BossPoop : MonoBehaviour
{
	[SerializeField] private float fallSpeed = 5f;
	[SerializeField] private int damage = 1;
	[SerializeField] private float lifetime = 5f;
	[SerializeField] private LayerMask hitLayers;

	[SerializeField] private Animator animator;

	private Rigidbody2D rb;

	private void Start()
	{
		animator = GetComponent<Animator>();

		rb = GetComponent<Rigidbody2D>();
		rb.linearVelocity = new Vector2(0f, -fallSpeed);

		StartCoroutine(DelayDestory());
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (((1 << other.gameObject.layer) & hitLayers) != 0)
		{
			Health h = other.GetComponentInParent<Health>();
			if (h)
			{
				h.TakeDamage(damage);
			}
			animator.SetTrigger("destroy");
		}
	}
	public void OnDestroy()
	{
		Destroy(gameObject);
	}

	IEnumerator DelayDestory()
	{
		yield return new WaitForSeconds(lifetime);
		animator.SetTrigger("destroy");
	}
}