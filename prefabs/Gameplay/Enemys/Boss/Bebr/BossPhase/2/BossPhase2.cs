using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Boss/Phase2")]
public class Phase2 : BossPhase
{
	[SerializeField] private BossSideMoveBehaviour movement = new BossSideMoveBehaviour();
	[SerializeField] private BossPoopAttack poopAttack = new BossPoopAttack();
	[SerializeField] private int passLimit = 3;
	[SerializeField] private float transitionDuration = 0.5f;   // длительность подъёма/спуска

	private MonoBehaviour host;
	private Rigidbody2D rb;
	private Coroutine mainRoutine;
	private int passCount;

	// Параметры плавного перехода
	private float transitionTimer;
	private float startY;
	private bool isTransitioningUp;
	private bool isTransitioningDown;

	public event Action OnPhaseComplete;

	public override void Enter(MonoBehaviour host)
	{
		this.host = host;
		rb = host.GetComponent<Rigidbody2D>();
		movement.Initialize(host.transform.position);
		poopAttack.Initialize();

		passCount = 0;
		// Запускаем плавный подъём
		startY = rb.position.y;
		transitionTimer = 0f;
		isTransitioningUp = true;
		isTransitioningDown = false;

		mainRoutine = host.StartCoroutine(PhaseRoutine());
	}

	public override void Exit(MonoBehaviour host)
	{
		if (mainRoutine != null) host.StopCoroutine(mainRoutine);
		if (rb)
		{
			rb.linearVelocity = Vector2.zero;
			// Мгновенно возвращаем на нормальную высоту, чтобы следующая фаза началась правильно
			Vector2 pos = rb.position;
			pos.y = movement.GetNormalY();
			rb.MovePosition(pos);
		}
	}

	private IEnumerator PhaseRoutine()
	{
		while (true)
		{
			// Горизонтальное движение
			Vector2 velocity = movement.GetDesiredVelocity();
			rb.linearVelocity = velocity;

			// Вычисляем желаемую Y в зависимости от состояния перехода
			float desiredY;
			if (isTransitioningUp)
			{
				transitionTimer += Time.deltaTime;
				float t = Mathf.Clamp01(transitionTimer / transitionDuration);
				desiredY = Mathf.Lerp(startY, movement.GetAttackY(), t);
				if (t >= 1f) isTransitioningUp = false;
			}
			else if (isTransitioningDown)
			{
				transitionTimer += Time.deltaTime;
				float t = Mathf.Clamp01(transitionTimer / transitionDuration);
				desiredY = Mathf.Lerp(startY, movement.GetNormalY(), t);
				if (t >= 1f)
				{
					// Снижение завершено – фаза заканчивается
					rb.linearVelocity = Vector2.zero;
					OnPhaseComplete?.Invoke();
					yield break;
				}
			}
			else
			{
				desiredY = movement.GetAttackY();   // обычная высота атаки
			}

			// Применяем позицию
			Vector2 newPos = rb.position + velocity * Time.fixedDeltaTime;
			bool reachedEdge = movement.ClampPosition(ref newPos.x);
			newPos.y = desiredY;
			rb.MovePosition(newPos);

			// Обработка достижения края (только в режиме атаки, не на переходе)
			if (!isTransitioningDown && reachedEdge)
			{
				movement.Flip();
				passCount++;
				if (passCount >= passLimit)
				{
					// Запускаем плавное снижение
					isTransitioningDown = true;
					transitionTimer = 0f;
					startY = rb.position.y;
					// Прекращаем сброс кала
					continue;
				}
			}

			// Сброс кала (только в режиме атаки)
			if (!isTransitioningDown)
				poopAttack.TryDropPoop(host.transform.position);

			yield return null;
		}
	}
}