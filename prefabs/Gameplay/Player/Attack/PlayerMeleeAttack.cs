using UnityEngine;

public class PlayerMeleeAttack : MeleeAttack
{
#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetButtonDown("Fire1") && !IsAttacking)
		{
			OnAttack();
		}
	}
#endif
}