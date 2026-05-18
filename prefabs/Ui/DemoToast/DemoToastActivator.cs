using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DemoToastActivator : MonoBehaviour
{
	[Header("Список неактивных кнопок")]
	[SerializeField] private List<Button> disabledButtons;

	[Header("Сам тост")]
	[SerializeField] private DemoToast toast;

	private void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0))
			CheckClick();
#else
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        CheckClick();
#endif
	}

	private void CheckClick()
	{
		PointerEventData pointerData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerData, results);

		foreach (RaycastResult result in results)
		{
			Button hitButton = result.gameObject.GetComponent<Button>();
			if (hitButton != null && disabledButtons.Contains(hitButton))
			{
				toast.Show();
				return;
			}
		}
	}
}