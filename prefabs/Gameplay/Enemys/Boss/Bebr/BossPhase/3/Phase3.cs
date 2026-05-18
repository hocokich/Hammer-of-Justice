using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Boss/Phase3")]
public class Phase3 : BossPhase
{
	[SerializeField] private BossRestBehaviour rest = new BossRestBehaviour();

	private MonoBehaviour host;
	private Rigidbody2D rb;
	private Coroutine mainRoutine;

	public event Action OnPhaseComplete;

	public override void Enter(MonoBehaviour host)
	{
		this.host = host;
		rb = host.GetComponent<Rigidbody2D>();
		rest.Initialize(host.transform.position);
		mainRoutine = host.StartCoroutine(RestRoutine());
	}

	public override void Exit(MonoBehaviour host)
	{
		if (mainRoutine != null) host.StopCoroutine(mainRoutine);
		if (rb) rb.linearVelocity = Vector2.zero;
	}

	private IEnumerator RestRoutine()
	{
		float timer = 0f;
		while (timer < rest.duration)
		{
			Vector2 vel = rest.CalculateVelocity(rb.position);
			rb.linearVelocity = vel;
			timer += Time.deltaTime;
			yield return null;
		}
		OnPhaseComplete?.Invoke();
	}
}