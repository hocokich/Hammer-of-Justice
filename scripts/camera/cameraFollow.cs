using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Цель")]
	[SerializeField] private Transform target;

	[Header("Плавность")]
	[SerializeField] private float smoothTime = 0.15f;

	[Header("Ограничения камеры по X")]
	[SerializeField] private bool limitX = true;
	[SerializeField] private float minX;
	[SerializeField] private float maxX;

	private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		transform.position = new Vector3(0, 0, -10);

		if (target == null)
			target = GameObject.FindGameObjectWithTag("Player")?.transform;
	}

	private void LateUpdate()
	{
		if (target == null) return;

		Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -10);

		if (limitX)
		{
			desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
		}

		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
	}
}