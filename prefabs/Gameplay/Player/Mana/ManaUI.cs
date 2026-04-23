using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
	[Header("Шары маны")]
	[SerializeField] private Image[] manaBalls;

	[Header("Спрайты")]
	[SerializeField] private Sprite fullMana;
	[SerializeField] private Sprite emptyMana;

	private Mana playerMana;

	private void Start()
	{
		playerMana = FindAnyObjectByType<Mana>();

		if (playerMana != null)
		{
			playerMana.OnManaChanged += UpdateManaUI;
			UpdateManaUI(playerMana.CurrentMana, playerMana.MaxMana);
		}
	}

	private void UpdateManaUI(int current, int max)
	{
		for (int i = 0; i < manaBalls.Length; i++)
		{
			if (i < current)
				manaBalls[i].sprite = fullMana;
			else
				manaBalls[i].sprite = emptyMana;
		}
	}

	private void OnDestroy()
	{
		if (playerMana != null)
		{
			playerMana.OnManaChanged -= UpdateManaUI;
		}
	}
}