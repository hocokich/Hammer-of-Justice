using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ItemPickup : MonoBehaviour
{
	[SerializeField] private ItemConfig itemConfig;
	[SerializeField] private Animator animator;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player")) return;

		if (itemConfig != null && itemConfig.Use(other.gameObject))
		{
			transform.parent.localScale *= 2f;

			animator.SetTrigger("pickedUp");
		}
	}
}