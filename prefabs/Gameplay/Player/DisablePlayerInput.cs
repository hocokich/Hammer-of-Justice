using UnityEngine;

public class DisablePlayerInput : MonoBehaviour
{
	public void Disable()
	{
		GameObject player = GameObject.FindWithTag("Player");
		if (player == null) return;

		var motion = player.GetComponent<motion>();
		var melee = player.GetComponent<MeleeAttack>();
		var range = player.GetComponent<RangeAttack>();

		if (motion) motion.enabled = false;
		if (melee) melee.enabled = false;
		if (range) range.enabled = false;
	}
}