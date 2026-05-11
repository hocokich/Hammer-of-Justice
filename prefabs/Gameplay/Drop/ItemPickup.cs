using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	[SerializeField] private ItemConfig itemConfig;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		if (itemConfig != null && itemConfig.Use(other.gameObject))
			Destroy(transform.parent.gameObject); // уничтожаем весь предмет (родитель)
	}
}