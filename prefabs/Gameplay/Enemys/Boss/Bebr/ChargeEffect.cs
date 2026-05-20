using UnityEngine;
using System.Collections;

public class ChargeEffect : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private float fadeInDuration = 0.4f;   // длительность появления

	private void Awake()
	{
		if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
		// Изначально полностью прозрачен
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
	}

	/// <summary> Плавно проявить эффект (альфа от 0 до 1). </summary>
	public IEnumerator FadeInRoutine()
	{
		float elapsed = 0f;
		while (elapsed < fadeInDuration)
		{
			elapsed += Time.deltaTime;
			float a = Mathf.Clamp01(elapsed / fadeInDuration);
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
			yield return null;
		}
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
	}

	/// <summary> Мгновенно сбросить альфу в 0 (после выстрела). </summary>
	public void ResetAlpha()
	{
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
	}
}