using UnityEngine;

public class StatsOnOff : MonoBehaviour
{
	[SerializeField] private GameObject statsObject;
	private InteractableButton button;

	private void Start()
	{
		button = GetComponent<InteractableButton>();
		if (button != null)
		{
			button.OnInteractableChanged += OnInteractableChanged;
			// Начальное состояние
			statsObject.SetActive(button.IsInteractable());
		}
	}

	private void OnInteractableChanged(bool interactable)
	{
		if (statsObject != null)
			statsObject.SetActive(interactable);
	}

	private void OnDestroy()
	{
		if (button != null)
			button.OnInteractableChanged -= OnInteractableChanged;
	}
}