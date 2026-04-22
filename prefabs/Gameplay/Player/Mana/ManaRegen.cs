using UnityEngine;

public class ManaRegen : MonoBehaviour
{
	[SerializeField] private float regenInterval = 3f;
	[SerializeField] private int regenAmount = 1;

	private Mana mana;
	private float timer;

	private void Start()
	{
		mana = GetComponent<Mana>();
	}

	private void Update()
	{
		if (mana == null) return;

		timer += Time.deltaTime;

		if (timer >= regenInterval)
		{
			timer = 0f;
			mana.RestoreMana(regenAmount);
		}
	}
}