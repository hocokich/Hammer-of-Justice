using UnityEngine;

[CreateAssetMenu(fileName = "ManaConfig", menuName = "Items/Mana Config")]
public class ManaConfig : ItemConfig
{
	[SerializeField] private int manaAmount = 1;

	public override bool Use(GameObject player)
	{
		Mana m = player.GetComponent<Mana>();
		if (m == null) return false;
		if (m.CurrentMana >= m.MaxMana) return false;

		m.RestoreMana(manaAmount);
		return true;
	}
}