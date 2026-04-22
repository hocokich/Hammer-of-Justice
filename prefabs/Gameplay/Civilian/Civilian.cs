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

	public void SetRescued(bool rescued)
	{
		isRescued = rescued;

		if (rescued)
		{
			CellDestroy cell = GetComponentInChildren<CellDestroy>();
			if (cell != null) cell.Destroy();
		}

		UpdateIcon();
	}

	public void UpdateIcon()
	{
		if (icon != null) icon.SetActive(isRescued);
	}
}