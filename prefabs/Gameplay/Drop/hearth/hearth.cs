using UnityEngine;

public class HeartPickup : MonoBehaviour
{
	[SerializeField] private int healAmount = 1;
	[SerializeField] private float bounceForce = 2f;
	[SerializeField] private float sideForce = 1f;

	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		float randomX = Random.Range(-sideForce, sideForce);
		rb.linearVelocity = new Vector2(randomX, -bounceForce);
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
			if (playerHealth != null && playerHealth.CurrentHealth < playerHealth.MaxHealth)
			{
				playerHealth.Heal(healAmount);
				Destroy(gameObject);
			}
		}
	}
}