using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
	private HashSet<Effect> activeEffects = new();

	public void ApplyEffect(Effect effect)
	{
		if (effect == null || activeEffects.Contains(effect)) return;
		effect.Apply(gameObject);
		activeEffects.Add(effect);
	}

	public void RemoveEffect(Effect effect)
	{
		if (effect == null || !activeEffects.Contains(effect)) return;
		effect.Remove(gameObject);
		activeEffects.Remove(effect);
	}
}