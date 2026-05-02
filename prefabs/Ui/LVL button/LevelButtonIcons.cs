using UnityEngine;
using UnityEngine.UI;

public class LevelButtonIcons : MonoBehaviour
{
	[Header("Иконки")]
	[SerializeField] private Image[] civilianIcons;   // 3 кружочка
	[SerializeField] private Image chestIcon;

	[Header("Спрайты жителей")]
	[SerializeField] private Sprite civilianCage;
	[SerializeField] private Sprite civilianResc;

	[Header("Спрайты сундука")]
	[SerializeField] private Sprite chestClose;
	[SerializeField] private Sprite chestOpen;

	/// <param name="rescued">Массив флагов спасённых жителей. Длина может быть меньше 3 – тогда остальные тусклые.</param>
	/// <param name="chestOpened">true если сундук открыт.</param>
	public void UpdateIcons(bool[] rescued, bool chestOpened)
	{
		// Жители
		for (int i = 0; i < civilianIcons.Length; i++)
		{
			bool saved = i < rescued.Length && rescued[i];
			civilianIcons[i].sprite = saved ? civilianResc : civilianCage;
		}

		// Сундук
		if (chestIcon != null)
			chestIcon.sprite = chestOpened ? chestOpen : chestClose;
	}
}