using UnityEngine;

public class Civilian : MonoBehaviour
{
	public bool isRescued = false;

	[Header("»конка в UI")]
	[SerializeField] private GameObject icon; // »конка этого жител€ в UI

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (isRescued) return;

		if (other.CompareTag("Player"))
		{
			Rescue();
			icon.SetActive(isRescued);
		}
	}

	public void Rescue()
	{
		isRescued = true;
		UpdateIcon();
	}

	private void UpdateIcon()
	{
		if (icon != null)
		{
			icon.SetActive(isRescued);
		}
	}
}