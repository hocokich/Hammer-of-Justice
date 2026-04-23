using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
	public delegate void ParallaxCameraDelegate(float deltaX, float deltaY);
	public ParallaxCameraDelegate onCameraTranslate;

	private float oldPositionX;
	private float oldPositionY;

	void Start()
	{
		oldPositionX = transform.position.x;
		oldPositionY = transform.position.y;
	}

	void Update()
	{
		bool movedX = transform.position.x != oldPositionX;
		bool movedY = transform.position.y != oldPositionY;

		if (movedX || movedY)
		{
			if (onCameraTranslate != null)
			{
				float deltaX = oldPositionX - transform.position.x;
				float deltaY = oldPositionY - transform.position.y;
				onCameraTranslate(deltaX, deltaY);
			}

			oldPositionX = transform.position.x;
			oldPositionY = transform.position.y;
		}
	}
}