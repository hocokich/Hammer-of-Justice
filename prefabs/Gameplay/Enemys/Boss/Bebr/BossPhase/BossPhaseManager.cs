using UnityEngine;
using System;
using UnityEngine.Events;

public class BossPhaseManager : MonoBehaviour
{
	[SerializeField] private BossPhase[] phases;
	[SerializeField] private Animator animator;
	[SerializeField] private UnityEvent rest;

	private int currentIndex = 0;

	// Имена параметров (можно вынести в сериализованные поля)
	private static readonly string[] phaseParams = { "phase1", "phase2", "rest" };

	private void Start()
	{
		if (animator == null) animator = GetComponent<Animator>();
		if (phases.Length > 0) ActivatePhase(0);
	}

	private void ActivatePhase(int index)
	{
		// Выключаем предыдущую фазу
		if (currentIndex < phases.Length && phases[currentIndex] != null)
		{
			UnsubscribeFromComplete(phases[currentIndex]);
			phases[currentIndex].Exit(this);
		}

		// Сбрасываем все Bool-флаги фаз
		foreach (string param in phaseParams)
			animator?.SetBool(param, false);

		// Включаем новую фазу
		if (index < phases.Length && phases[index] != null)
		{
			SubscribeToComplete(phases[index]);
			phases[index].Enter(this);

			// Устанавливаем нужный Bool в true
			if (index < phaseParams.Length)
				animator?.SetBool(phaseParams[index], true);
		}
		currentIndex = index;
	}

	private void SubscribeToComplete(BossPhase phase)
	{
		if (phase is Phase1 p1) p1.OnPhaseComplete += OnPhaseComplete;
		else if (phase is Phase2 p2) p2.OnPhaseComplete += OnPhaseComplete;
		else if (phase is Phase3 p3) p3.OnPhaseComplete += OnPhaseComplete;
	}

	private void UnsubscribeFromComplete(BossPhase phase)
	{
		if (phase is Phase1 p1) p1.OnPhaseComplete -= OnPhaseComplete;
		else if (phase is Phase2 p2) p2.OnPhaseComplete -= OnPhaseComplete;
		else if (phase is Phase3 p3) p3.OnPhaseComplete -= OnPhaseComplete;
	}

	private void OnPhaseComplete()
	{
		int nextIndex = (currentIndex + 1) % phases.Length;
		ActivatePhase(nextIndex);

		if (nextIndex == 2)   // началась фаза отдыха
		{
			rest?.Invoke();
		}
	}
}