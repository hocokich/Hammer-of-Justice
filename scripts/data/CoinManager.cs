using UnityEngine;

public class CoinManager : MonoBehaviour
{
	public static CoinManager Instance;
	public int coinsCollected = 0;

	void Awake()
	{
		Instance = this;
	}

	public void CollectedCoin(int amount)
	{
		coinsCollected += amount;
	}
}