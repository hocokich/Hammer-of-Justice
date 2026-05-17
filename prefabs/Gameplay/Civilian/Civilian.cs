using UnityEngine;
using UnityEngine.UI;

public class Civilian : MonoBehaviour
{
	public bool isRescued = false;

	[Header("»конка в UI")]
	[SerializeField] private Image icon;
	[SerializeField] private Sprite spriteCaged;
	[SerializeField] private Sprite spriteRescued;

	private void Start()
	{
		UpdateIcon();
	}

	public void Rescue()
	{
		isRescued = true;
		UpdateIcon();
	}

	public void SetRescued(bool rescued)
	{
		isRescued = rescued;

		if (rescued)
		{
			Cage cell = GetComponentInChildren<Cage>();
			if (cell != null) cell.Destroy();
		}

		UpdateIcon();
	}

	public void UpdateIcon()
	{
		if (icon != null)
			icon.sprite = isRescued ? spriteRescued : spriteCaged;
	}
}