using UnityEngine;

public class ManaPickup : MonoBehaviour
{
	[SerializeField] private int manaAmount = 1;
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
			Mana playerMana = collision.gameObject.GetComponent<Mana>();
			if (playerMana == null)
				playerMana = collision.gameObject.GetComponentInParent<Mana>();

			if (playerMana != null && playerMana.CurrentMana < playerMana.MaxMana)
			{
				playerMana.RestoreMana(manaAmount);
				Destroy(gameObject);
			}
		}
	}
}