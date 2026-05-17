using UnityEngine;
using System.Collections;

public class BossFireAttack : MonoBehaviour
{
	[SerializeField] private GameObject fireballPrefab;
	[SerializeField] private Transform firePoint;
	[SerializeField] private float minCooldown = 1f;
	[SerializeField] private float maxCooldown = 2f;
	[SerializeField] private float chargeDuration = 0.8f;
	[SerializeField] private ChargeEffect chargeEffect;   // ссылка на компонент эффекта

	public System.Action OnShotFired;

	private float timer;
	private bool isCharging;

	private void Start()
	{
		timer = Random.Range(minCooldown, maxCooldown);
	}

	private void Update()
	{
		if (isCharging) return;
		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			Shoot();
			timer = Random.Range(minCooldown, maxCooldown);
		}
	}

	private void Shoot()
	{
		StartCoroutine(ChargeAndShoot());
		OnShotFired?.Invoke();   // в конце Shoot
	}

	private IEnumerator ChargeAndShoot()
	{
		isCharging = true;

		// Запускаем визуальный эффект зарядки
		chargeEffect?.Play();

		// Ждём зарядку
		yield return new WaitForSeconds(chargeDuration);

		// Спавним снаряд
		GameObject proj = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
		HomingFireball hf = proj.GetComponent<HomingFireball>();
		if (hf)
		{
			GameObject player = GameObject.FindWithTag("Player");
			if (player) hf.SetTarget(player.transform);
		}

		isCharging = false;
	}
}
