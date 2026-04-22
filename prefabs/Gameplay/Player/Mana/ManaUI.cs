using UnityEngine;

public class ManaUI : MonoBehaviour
{
	[SerializeField] private GameObject[] manaBalls;
	private Mana playerMana;

	private void Start()
	{
		playerMana = FindObjectOfType<Mana>();

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
			manaBalls[i].SetActive(i < current);
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