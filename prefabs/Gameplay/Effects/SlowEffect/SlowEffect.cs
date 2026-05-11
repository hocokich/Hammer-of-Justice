using UnityEngine;

[CreateAssetMenu(fileName = "SlowEffect", menuName = "Effects/Slow Effect")]
public class SlowEffect : Effect
{
	[Header("Замедление")]
	[SerializeField] private float speedMultiplier = 0.5f;
	[SerializeField] private float jumpForceMultiplier = 0.5f;
	[SerializeField] private bool disableDoubleJump = true;

	public override void Apply(GameObject target)
	{
		// Проверяем, нет ли уже такого эффекта на цели
		if (target.TryGetComponent<SlowEffectComponent>(out var existing))
		{
			Destroy(existing);
		}

		var motion = target.GetComponent<PlayerMotion>();   // или EnemyMovement, если враги
		if (motion == null) return;

		// Добавляем компонент-хранитель, который запомнит оригиналы и будет жить, пока эффект активен
		var component = target.AddComponent<SlowEffectComponent>();
		component.Initialize(motion, speedMultiplier, jumpForceMultiplier, disableDoubleJump);

		// Если есть длительность, запускаем таймер на удаление
		if (duration > 0f)
		{
			component.StartCoroutine(RemoveAfterDuration(target, duration));
		}
	}

	public override void Remove(GameObject target)
	{
		var component = target.GetComponent<SlowEffectComponent>();
		if (component != null)
		{
			Destroy(component);
		}
	}

	private System.Collections.IEnumerator RemoveAfterDuration(GameObject target, float delay)
	{
		yield return new WaitForSeconds(delay);
		Remove(target);
		// Убираем из EffectManager (он следит за списком активных)
		target.GetComponent<EffectManager>()?.RemoveEffect(this);
	}
}