using System.Collections;
using UnityEngine;

[System.Serializable]
public class BossFireBehaviour
{
	[Header("Настройки выстрела")]
	public GameObject fireballPrefab;
	public float minCooldown = 1f;
	public float maxCooldown = 2f;
	public GameObject chargeEffect;

	private MonoBehaviour host;
	private Transform firePoint;
	private Transform player;
	private ChargeEffect chargeEffectComp;

	public void Initialize(MonoBehaviour host, Transform firePoint, Transform player)
	{
		this.host = host;
		this.firePoint = firePoint;
		this.player = player;

		if (chargeEffect != null)
			chargeEffectComp = chargeEffect.GetComponent<ChargeEffect>();
	}

	public IEnumerator ChargeAndShoot()
	{
		// Плавно проявляем эффект зарядки
		if (chargeEffectComp != null)
			yield return chargeEffectComp.FadeInRoutine();

		// Спавним снаряд и направляем в точку, где был игрок
		if (fireballPrefab && player)
		{
			Vector3 spawnPos = firePoint ? firePoint.position : host.transform.position;
			GameObject proj = Object.Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
			StraightFireball sf = proj.GetComponent<StraightFireball>();
			if (sf) sf.SetTargetPosition(player.position);
		}

		// Мгновенно гасим эффект
		if (chargeEffectComp != null)
			chargeEffectComp.ResetAlpha();
	}
}