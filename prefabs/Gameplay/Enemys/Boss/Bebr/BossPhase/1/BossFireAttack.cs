using System.Collections;
using UnityEngine;

[System.Serializable]
public class BossFireBehaviour
{
	[Header("Настройки выстрела")]
	public GameObject fireballPrefab;
	public float minCooldown = 1f;
	public float maxCooldown = 2f;
	public float chargeDuration = 0.8f;
	public GameObject chargeEffect;

	private MonoBehaviour host;
	private Transform firePoint;
	private Transform player;

	public void Initialize(MonoBehaviour host, Transform firePoint, Transform player)
	{
		this.host = host;
		this.firePoint = firePoint;
		this.player = player;
	}

	public IEnumerator ChargeAndShoot()
	{
		if (chargeEffect) chargeEffect.SetActive(true);
		yield return new WaitForSeconds(chargeDuration);
		if (chargeEffect) chargeEffect.SetActive(false);

		if (fireballPrefab)
		{
			Vector3 spawnPos = firePoint ? firePoint.position : host.transform.position;
			GameObject proj = GameObject.Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
			HomingFireball hf = proj.GetComponent<HomingFireball>();
			if (hf && player) hf.SetTarget(player);
		}
	}
}