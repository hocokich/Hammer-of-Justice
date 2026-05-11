using UnityEngine;

[CreateAssetMenu(fileName = "HeartConfig", menuName = "Items/Heart Config")]
public class HeartConfig : ItemConfig
{
	[SerializeField] private int healAmount = 1;

	public override bool Use(GameObject player)
	{
		PlayerHealth ph = player.GetComponent<PlayerHealth>();
		if (ph == null || ph.IsDead) return false;
		if (ph.CurrentHealth >= ph.MaxHealth) return false;

		ph.Heal(healAmount);
		return true;
	}
}