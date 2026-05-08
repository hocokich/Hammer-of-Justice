using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
	[SerializeField] private DropThroughPlatform dropComponent;   // ссылка на скрипт проваливания
	private Vector2 touchStartPos;

	private void Start()
	{
		if (dropComponent == null)
			dropComponent = GameObject.FindWithTag("Player").GetComponent<DropThroughPlatform>();
	}

	private void Update()
	{
#if !UNITY_EDITOR
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            float swipeDistance = touch.position.y - touchStartPos.y;
            if (swipeDistance < -Screen.height * 0.15f)
            {
                dropComponent?.PerformDrop();
            }
        }
#endif
	}
}