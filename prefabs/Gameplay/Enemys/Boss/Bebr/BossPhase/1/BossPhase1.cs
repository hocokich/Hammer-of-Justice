using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Boss/Phase1")]
public class Phase1 : BossPhase
{
	[SerializeField] private BossMoveBehaviour movement = new BossMoveBehaviour();
	[SerializeField] private BossFireBehaviour fire = new BossFireBehaviour();
	[SerializeField] private int shotLimit = 5;
	public ChargeEffect chargeEffect;


	private MonoBehaviour host;
	private Rigidbody2D rb;
	private Transform player;
	private Coroutine mainRoutine;
	private int shotsFired;

	public event Action OnPhaseComplete;

	public override void Enter(MonoBehaviour host)
	{
		this.host = host;
		rb = host.GetComponent<Rigidbody2D>();
		player = GameObject.FindWithTag("Player")?.transform;

		var chargeComp = host.GetComponentInChildren<ChargeEffect>(true);
		if (chargeComp != null)
			fire.chargeEffect = chargeComp.gameObject;   // <-- передаём GameObject в fire

		Transform firePoint = host.transform.Find("FirePoint");
		movement.Initialize(player, host.transform.position);
		fire.Initialize(host, firePoint, player);

		shotsFired = 0;
		mainRoutine = host.StartCoroutine(PhaseRoutine());
	}

	public override void Exit(MonoBehaviour host)
	{
		if (mainRoutine != null) host.StopCoroutine(mainRoutine);
		if (rb) rb.linearVelocity = Vector2.zero;
	}

	private IEnumerator PhaseRoutine()
	{
		float nextShotTime = Time.time + UnityEngine.Random.Range(fire.minCooldown, fire.maxCooldown);

		while (true)
		{
			// Движение
			Vector2 velocity = movement.CalculateVelocity(rb.position);
			rb.linearVelocity = velocity;

			// Стрельба
			if (Time.time >= nextShotTime)
			{
				yield return fire.ChargeAndShoot();
				nextShotTime = Time.time + UnityEngine.Random.Range(fire.minCooldown, fire.maxCooldown);
				shotsFired++;
				if (shotsFired >= shotLimit)
				{
					OnPhaseComplete?.Invoke();
					yield break;
				}
			}

			yield return null;
		}
	}
}