using UnityEngine;

public class SlowEffectComponent : MonoBehaviour
{
	private PlayerMotion motion;
	private float originalMoveSpeed;
	private float originalJumpForce;
	private int originalMaxJumps;

	public void Initialize(PlayerMotion playerMotion, float speedMult, float jumpMult, bool disableDblJump)
	{
		motion = playerMotion;

		// Запоминаем оригиналы
		originalMoveSpeed = motion.MoveSpeed;
		originalJumpForce = motion.JumpForce;
		originalMaxJumps = motion.MaxJumps;

		// Применяем замедление
		motion.MoveSpeed = originalMoveSpeed * speedMult;
		motion.JumpForce = originalJumpForce * jumpMult;
		motion.MaxJumps = disableDblJump ? 1 : originalMaxJumps;
	}

	private void OnDestroy()
	{
		// При удалении компонента восстанавливаем оригиналы
		if (motion != null)
		{
			motion.MoveSpeed = originalMoveSpeed;
			motion.JumpForce = originalJumpForce;
			motion.MaxJumps = originalMaxJumps;
		}
	}
}