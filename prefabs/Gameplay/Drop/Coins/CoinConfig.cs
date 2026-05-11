using UnityEngine;

[CreateAssetMenu(fileName = "CoinConfig", menuName = "Items/Coin Config")]
public class CoinConfig : ItemConfig
{
	[SerializeField] private int coinValue = 1;

	public override bool Use(GameObject player)
	{
		CoinManager.Instance?.CollectedCoin(coinValue);
		return true; // монеты всегда подбираются
	}
}