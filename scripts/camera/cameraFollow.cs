using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform target;
	[SerializeField] private float smoothTime = 0.15f;
	[SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

	private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		if (target == null)
			target = GameObject.FindGameObjectWithTag("Player")?.transform;
	}

	private void LateUpdate()
	{
		if (target == null) return;

		Vector3 desiredPosition = target.position + offset;
		desiredPosition.z = offset.z;

		transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
	}
}