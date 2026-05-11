using UnityEngine;

public abstract class Effect : ScriptableObject
{
	[Header("Длительность (0 = бесконечно, управляется триггером)")]
	[SerializeField] protected float duration;

	public abstract void Apply(GameObject target);
	public abstract void Remove(GameObject target);
}