using UnityEngine;
using UnityEngine.UI;
using System;

public class InteractableButton : Button
{
	public event Action<bool> OnInteractableChanged;
	private bool wasInteractable;

	// Проверяем изменение interactable каждый раз, когда система его запрашивает
	public override bool IsInteractable()
	{
		bool current = base.IsInteractable();
		if (current != wasInteractable)
		{
			wasInteractable = current;
			OnInteractableChanged?.Invoke(current);
		}
		return current;
	}
}