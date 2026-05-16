using UnityEngine;

public class DisablePlayerInput : MonoBehaviour
{
	public void Disable()
	{
		var playerButtons = GameObject.Find("Player Buttons");
		if (playerButtons) playerButtons.SetActive(false);

#if UNITY_EDITOR
		GameObject player = GameObject.FindWithTag("Player");
		if (player == null) return;

		var mobileInput = FindAnyObjectByType<MobileInputController>();
		var motion = player.GetComponent<PlayerMotion>();
		var melee = player.GetComponent<MeleeAttack>();
		var range = player.GetComponent<RangeAttack>();

		if (mobileInput) mobileInput.enabled = false;
		if (motion) motion.enabled = false;
		if (melee) melee.enabled = false;
		if (range) range.enabled = false;
#endif
	}
}