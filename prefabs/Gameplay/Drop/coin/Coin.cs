using UnityEngine;

public class Coin : MonoBehaviour
{
	[SerializeField] private int value = 1;
	//[SerializeField] private float bounceForce = 2f;
	//[SerializeField] private float sideForce = 1f;

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