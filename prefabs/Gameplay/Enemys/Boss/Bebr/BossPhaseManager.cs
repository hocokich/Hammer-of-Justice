using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
	[Header("Компоненты фаз")]
	[SerializeField] private BossMovement phase1Movement;
	[SerializeField] private BossFireAttack phase1Attack;
	[SerializeField] private BossPhase2Behavior phase2Behavior;

	[Header("Лимиты для переключения")]
	[SerializeField] private int phase1ShotLimit = 5;   // сколько выстрелов в первой фазе
	[SerializeField] private int phase2PassLimit = 3;   // сколько пролётов во второй фазе

	private int phase1Shots;
	private int phase2Passes;

	private enum Phase { One, Two, Rest }
	private Phase currentPhase = Phase.One;

	private void Start()
	{
		ActivatePhase(Phase.One);
		if (phase1Attack != null)
			phase1Attack.OnShotFired += OnShotFired;   // нужно добавить событие в BossFireAttack
	}

	private void OnShotFired()
	{
		if (currentPhase != Phase.One) return;
		phase1Shots++;
		if (phase1Shots >= phase1ShotLimit)
		{
			ActivatePhase(Phase.Two);
		}
	}

	// Вызывается, когда босс достиг края во второй фазе (можно через коллбэк или проверку)
	public void OnPhase2PassCompleted()
	{
		if (currentPhase != Phase.Two) return;
		phase2Passes++;
		if (phase2Passes >= phase2PassLimit)
		{
			ActivatePhase(Phase.Rest);
		}
	}

	private void ActivatePhase(Phase phase)
	{
		currentPhase = phase;

		// Выключаем всё
		if (phase1Movement) phase1Movement.enabled = false;
		if (phase1Attack) phase1Attack.enabled = false;
		if (phase2Behavior) phase2Behavior.enabled = false;

		// Включаем нужное
		switch (phase)
		{
			case Phase.One:
				if (phase1Movement) phase1Movement.enabled = true;
				if (phase1Attack) phase1Attack.enabled = true;
				phase1Shots = 0;
				break;

			case Phase.Two:
				if (phase2Behavior) phase2Behavior.enabled = true;
				phase2Passes = 0;
				break;

			case Phase.Rest:
				// Фаза отдыха: просто медленные колебания (можно сделать отдельный скрипт или оставить пустым)
				break;
		}
	}
}