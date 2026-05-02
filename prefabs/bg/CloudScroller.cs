using UnityEngine;

public class CloudScroller : MonoBehaviour
{
	[SerializeField] private float scrollSpeed = 0.3f;   // скорость движени€ вправо
	private SpriteRenderer spriteRenderer;
	private float spriteWidth;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
			spriteWidth = spriteRenderer.bounds.size.x;
		else
			spriteWidth = 10f;   // fallback, если нет спрайта
	}

	private void LateUpdate()
	{
		// ƒвигаем только по X (Y остаЄтс€ под управлением ParallaxLayer)
		Vector3 pos = transform.localPosition;
		pos.x += scrollSpeed * Time.deltaTime;

		// «ацикливаем позицию, когда спрайт полностью ушЄл вправо за свою ширину
		if (pos.x > spriteWidth)
			pos.x -= spriteWidth;
		else if (pos.x < -spriteWidth)
			pos.x += spriteWidth;

		transform.localPosition = pos;
	}
}