using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
	[SerializeField] private int damage = 1;

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Health playerHealth = other.GetComponent<Health>();
			if (playerHealth != null)
			{
				playerHealth.TakeDamage(damage);
			}
		}
	}
}