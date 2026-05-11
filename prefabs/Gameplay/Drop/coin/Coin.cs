using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private int value = 1;

	private Rigidbody2D rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			CoinManager.Instance.CollectedCoin(value);
			Destroy(gameObject);
		}
	}
}