using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private int value = 1;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			CoinManager.Instance.CollectedCoin(value);
			Destroy(gameObject);
		}
	}
}