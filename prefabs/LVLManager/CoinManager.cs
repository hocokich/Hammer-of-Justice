using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
	public static CoinManager Instance;
	public int coinsCollected = 0;

	[SerializeField] private TextMeshProUGUI CoinsPerLvlText;
	[SerializeField] private TextMeshProUGUI TotalCoinsText;

	void Awake()
	{
		Instance = this;
	}

	private void Start() => UpdateTotalCoins();

	public void CollectedCoin(int amount)
	{
		coinsCollected += amount;
		UpdateTotalCoins();
	}
	public void UpdateTotalCoins()
	{
		if (GameManager.Instance != null)
			TotalCoinsText.text = $"{coinsCollected + GameManager.Instance.totalCoins}";
		else
			TotalCoinsText.text = $"{coinsCollected}";
	}

	public void UpdateCoinsPerLvl()
	{
		if (CoinsPerLvlText != null)
			CoinsPerLvlText.text = $"{coinsCollected}";
	}
}