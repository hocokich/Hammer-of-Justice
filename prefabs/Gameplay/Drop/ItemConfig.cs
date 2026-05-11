using UnityEngine;

public abstract class ItemConfig : ScriptableObject
{
	/// <summary>Попытаться использовать предмет. Возвращает true, если предмет был использован.</summary>
	public abstract bool Use(GameObject player);
}