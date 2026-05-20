using System.Collections;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
	[SerializeField] private SpriteRenderer targetRenderer;
	[SerializeField] private float duration = 0.5f;
	[SerializeField] private Material flashMaterial;        // материал с шейдером Custom/SpriteFlash
	[SerializeField] private bool autoSubscribe = true;     // автоматически искать Health и подписываться

	private Coroutine flashRoutine;
	private Health health;
	private Material originalMaterial;
	private MaterialPropertyBlock block;

	private void Awake()
	{
		if (targetRenderer == null)
			targetRenderer = GetComponent<SpriteRenderer>();

		if (targetRenderer != null)
			originalMaterial = targetRenderer.sharedMaterial;

		block = new MaterialPropertyBlock();

		if (autoSubscribe)
		{
			// Ищем Health на себе или родителе
			health = GetComponent<Health>() ?? GetComponentInParent<Health>();
			if (health != null)
			{
				health.OnHealthChanged += OnHealthChanged;
			}
		}
	}

	private void OnHealthChanged(int current, int max)
	{
		Flash();
	}

	public void Flash()
	{
		if (targetRenderer == null || flashMaterial == null) return;

		if (flashRoutine != null)
			StopCoroutine(flashRoutine);

		flashRoutine = StartCoroutine(FlashRoutine());
	}

	private IEnumerator FlashRoutine()
	{
		// Подменяем материал
		targetRenderer.material = flashMaterial;

		// Устанавливаем FlashAmount = 1 (полная вспышка)
		block.SetFloat("_FlashAmount", 1f);
		targetRenderer.SetPropertyBlock(block);

		float elapsed = 0f;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = 1f - elapsed / duration;   // убывает от 1 до 0
			block.SetFloat("_FlashAmount", t);
			targetRenderer.SetPropertyBlock(block);
			yield return null;
		}

		// Возвращаем исходный материал
		targetRenderer.material = originalMaterial;
	}

	private void OnDestroy()
	{
		if (health != null)
			health.OnHealthChanged -= OnHealthChanged;
	}
}