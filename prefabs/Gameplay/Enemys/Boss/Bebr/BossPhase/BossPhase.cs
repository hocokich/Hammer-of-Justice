using UnityEngine;

public abstract class BossPhase : ScriptableObject
{
	/// <summary>Вызывается при активации фазы. Host – объект босса.</summary>
	public abstract void Enter(MonoBehaviour host);

	/// <summary>Вызывается при деактивации фазы.</summary>
	public abstract void Exit(MonoBehaviour host);
}