using UnityEngine;

public class CloudSway : MonoBehaviour
{
	[SerializeField] private float swayAmplitudeX = 0.5f;
	[SerializeField] private float swayAmplitudeY = 0.2f;
	[SerializeField] private float swaySpeed = 0.5f;
	[SerializeField] private bool useRandomPhase = true;

	private Vector3 startLocalPosition;
	private float phase;

	private void Start()
	{
		startLocalPosition = transform.localPosition;
		phase = useRandomPhase ? Random.Range(0f, Mathf.PI * 2f) : 0f;
	}

	private void Update()
	{
		float offsetX = Mathf.Sin(Time.time * swaySpeed + phase) * swayAmplitudeX;
		float offsetY = Mathf.Cos(Time.time * swaySpeed * 0.7f + phase) * swayAmplitudeY; // немного другая частота для естественности

		transform.localPosition = startLocalPosition + new Vector3(offsetX, offsetY, 0f);
	}
}