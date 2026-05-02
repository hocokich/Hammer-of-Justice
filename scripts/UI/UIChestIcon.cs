using UnityEngine;
using UnityEngine.UI;

public class UIChestIcon : MonoBehaviour
{
	[SerializeField] private Image chestIcon;   // само изображение сундука (по умолчанию отключено)

	private void Start()
	{
		if (chestIcon != null)
			chestIcon.enabled = false;          // старт – скрыто

		// Подписываемся на событие открытия любого сундука
		ChestDrop.OnAnyChestOpened += ShowChestIcon;
	}

	private void ShowChestIcon()
	{
		if (chestIcon != null)
			chestIcon.enabled = true;
	}

	private void OnDestroy()
	{
		ChestDrop.OnAnyChestOpened -= ShowChestIcon;
	}
}