using UnityEngine;

public class ResetButton : MonoBehaviour
{
	public void OnResetClicked()
	{
		GameManager.Instance.ResetProgress();
	}
}
