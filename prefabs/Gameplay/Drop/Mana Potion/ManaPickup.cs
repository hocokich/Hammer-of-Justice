using UnityEngine;

public class ManaPickup : MonoBehaviour
{
	[SerializeField] private int manaAmount = 3;

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