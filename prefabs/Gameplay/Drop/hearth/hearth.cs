using UnityEngine;

public class HeartPickup : MonoBehaviour
{
	[SerializeField] private int healAmount = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
			if (playerHealth != null && playerHealth.CurrentHealth < playerHealth.MaxHealth)
			{
				playerHealth.Heal(healAmount);
				Destroy(gameObject);
			}
		}
	}
}