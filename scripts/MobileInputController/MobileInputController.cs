using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileInputController : MonoBehaviour
{
	[Header("Кнопки атаки / выстрела / прыжка")]
	[SerializeField] private Button attackButton;
	[SerializeField] private Button shootButton;
	[SerializeField] private Button jumpButton;

	[Header("Кнопки движения")]
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;

	// Ссылки на компоненты игрока
	private motion playerMotion;
	private MeleeAttack meleeAttack;
	private RangeAttack rangeAttack;

	private void Start()
	{
		// Ищем игрока по тегу (убедись, что у игрока стоит тег "Player")
		GameObject player = GameObject.FindWithTag("Player");
		if (player != null)
		{
			playerMotion = player.GetComponent<motion>();
			meleeAttack = player.GetComponent<MeleeAttack>();
			rangeAttack = player.GetComponent<RangeAttack>();
		}

		// Привязка мгновенных кнопок
		if (attackButton != null) attackButton.onClick.AddListener(() => meleeAttack?.OnAttack());
		if (shootButton != null) shootButton.onClick.AddListener(() => rangeAttack?.OnShoot());
		if (jumpButton != null) jumpButton.onClick.AddListener(() => playerMotion?.OnJump());

		// Настройка кнопок движения (зажатие)
		SetupHoldButton(leftButton, true);
		SetupHoldButton(rightButton, false);
	}

	private void SetupHoldButton(Button button, bool isLeft)
	{
		if (button == null) return;

		EventTrigger trigger = button.GetComponent<EventTrigger>();
		if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();
		trigger.triggers.Clear();

		// Нажатие (палец коснулся кнопки)
		EventTrigger.Entry downEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
		downEntry.callback.AddListener(_ =>
		{
			if (isLeft) playerMotion?.OnLeftDown();
			else playerMotion?.OnRightDown();
		});
		trigger.triggers.Add(downEntry);

		// Отпускание (палец поднялся над кнопкой)
		EventTrigger.Entry upEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
		upEntry.callback.AddListener(_ =>
		{
			if (isLeft) playerMotion?.OnLeftUp();
			else playerMotion?.OnRightUp();
		});
		trigger.triggers.Add(upEntry);

		// Палец ушёл за границы кнопки – сразу отжимаем
		EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
		exitEntry.callback.AddListener(_ =>
		{
			if (isLeft) playerMotion?.OnLeftUp();
			else playerMotion?.OnRightUp();
		});
		trigger.triggers.Add(exitEntry);

		// Палец зашёл на кнопку (например, при перетаскивании с другой кнопки) – нажимаем
		EventTrigger.Entry enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
		enterEntry.callback.AddListener(_ =>
		{
			if (isLeft) playerMotion?.OnLeftDown();
			else playerMotion?.OnRightDown();
		});
		trigger.triggers.Add(enterEntry);
	}
}