using UnityEngine;

public class manaAnim : MonoBehaviour
{
	[SerializeField] private Animator animator;

	public void Start() => animator = GetComponent<Animator>();

	public void OnDestroy()
	{
		Destroy(gameObject);
	}
}
