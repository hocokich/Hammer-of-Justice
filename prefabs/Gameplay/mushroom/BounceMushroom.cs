using UnityEngine;

public class BounceMushroom : MonoBehaviour
{
	[SerializeField] private float bounceForce = 15f;
	[SerializeField] private Animator animator;
	[SerializeField] private string bounceTrigger = "Bounce";

	private void Start()
	{
		if (animator == null) animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
		if (playerRb == null) return;

		// Игрок выше гриба и падает вниз
		if (other.transform.position.y > transform.position.y && playerRb.linearVelocity.y <= 0)
		{
			playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
			animator?.SetTrigger(bounceTrigger);
		}
	}
}