using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerMenu : MonoBehaviour
{
	[System.Serializable]
	public class ButtonAction
	{
		public string buttonName;        // Имя кнопки для поиска
		public string sceneName;		
		public UnityEngine.Events.UnityEvent onClickAction; // Методы при нажатии
	}
	[Header("Настройки кнопок")]
	[SerializeField] private List<ButtonAction> buttonActions = new List<ButtonAction>();

	[Header("Панель Настроек")]
	[SerializeField] private GameObject SettingsPanel;
	[Header("Панель актов")]
	[SerializeField] private GameObject ActsPanel;
	[Header("Панель 1 акт")]
	[SerializeField] private GameObject ActOnePanel;

	//[Header("Панель 2 акт")]
	//[SerializeField] private GameObject ActSecPanel;
	//[Header("Панель 3 акт")]
	//[SerializeField] private GameObject ActThirdPanel;

	[Header("Уровни первого акта")]
	[SerializeField] private List<ButtonAction> buttonFirstActLvls = new List<ButtonAction>();

	private Dictionary<string, Button> foundButtons = new Dictionary<string, Button>();
	private Dictionary<string, Button> foundLvls = new Dictionary<string, Button>();

	void Start()
	{
		FindAndAssignAllButtons();

		OpenedLvls();
	}

	void FindAndAssignAllButtons()
	{
		// Для кнопок в разделе обычных кнопок
		foreach (var buttonAction in buttonActions)
		{
			FindAndAssignButton(buttonAction.buttonName, buttonAction.onClickAction, foundButtons);
		}

		// Для кнопок в разделе уровней первого акта
		foreach (var buttonAction in buttonFirstActLvls)
		{
			FindAndAssignButton(buttonAction.buttonName, buttonAction.onClickAction, foundLvls);
		}
	}

	public void FindAndAssignButton(string buttonName, UnityEngine.Events.UnityEvent action, Dictionary<string, Button> targetDictionary)
	{
		if (string.IsNullOrEmpty(buttonName))
		{
			Debug.LogWarning("Имя кнопки не указано!");
			return;
		}

		// Поиск кнопки
		Button button = FindButtonByName(buttonName);

		if (button != null)
		{
			// Удаляем старые слушатели
			button.onClick.RemoveAllListeners();

			// Добавляем новые методы
			button.onClick.AddListener(() => action?.Invoke());

			// Сохраняем в указанный словарь
			if (!targetDictionary.ContainsKey(buttonName))
			{
				targetDictionary.Add(buttonName, button);
			}

			Debug.Log($"Кнопка '{buttonName}' найдена и сохранена в {(targetDictionary == foundLvls ? "foundLvls" : "foundButtons")}");
		}
		else
		{
			Debug.LogWarning($"Кнопка с именем '{buttonName}' не найдена!");
		}
	}

	Button FindButtonByName(string buttonName)
	{
		Button[] allButtons;

		// Глобальный поиск по всей сцене, включая неактивные
		List<Button> allButtonsList = new List<Button>();
		Button[] sceneButtons = Resources.FindObjectsOfTypeAll<Button>();

		foreach (Button btn in sceneButtons)
		{
			// Исключаем префабы
			if (btn.gameObject.scene.IsValid())
			{
				allButtonsList.Add(btn);
			}
		}
		allButtons = allButtonsList.ToArray();

		// Ищем по точному имени
		foreach (Button button in allButtons)
		{
			if (button.gameObject.name == buttonName)
			{
				return button;
			}
		}

		return null;
	}

	// Метод для принудительного поиска всех кнопок снова
	public void RefreshButtons()
	{
		foundButtons.Clear();
		FindAndAssignAllButtons();
	}

	// Получить найденную кнопку по имени
	public Button GetFoundButton(string buttonName)
	{
		if (foundButtons.TryGetValue(buttonName, out Button button))
		{
			return button;
		}
		// Если не нашли, ищем в кнопках уровней
		if (foundLvls.TryGetValue(buttonName, out button))
		{
			return button;
		}
		return null;
	}

	//Открываем текущий прогресс
	public void OpenedLvls()
	{
		GameManager.Instance.LoadGame();

		for (int i = 0; i < buttonFirstActLvls.Count; i++)
		{
			Button button = GetFoundButton(buttonFirstActLvls[i].buttonName);

			string sceneName = buttonFirstActLvls[i].sceneName;
			bool isCompleted = GameManager.Instance.IsLevelCompleted(sceneName);

			// Первый уровень всегда открыт, остальные — если предыдущий пройден
			bool isUnlocked;
			if (i == 0)
			{
				isUnlocked = true;
			}
			else
			{
				string prevSceneName = buttonFirstActLvls[i - 1].sceneName;
				isUnlocked = GameManager.Instance.IsLevelCompleted(prevSceneName);
			}

			button.interactable = isUnlocked;
		}
		Debug.Log("");
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void StartGame()
	{
		foundButtons["Start_Button"].gameObject.SetActive(false);
		foundButtons["Settings_Button"].gameObject.SetActive(false);
		foundButtons["Skills_Button"].gameObject.SetActive(false);
		foundButtons["Achivments_Button"].gameObject.SetActive(false);

		ActsPanel.SetActive(true);
	}
	public void OpenSettings()
	{
		foundButtons["Start_Button"].gameObject.SetActive(false);
		foundButtons["Settings_Button"].gameObject.SetActive(false);
		foundButtons["Skills_Button"].gameObject.SetActive(false);
		foundButtons["Achivments_Button"].gameObject.SetActive(false);

		SettingsPanel.SetActive(true);
	}


	public void BackToStart()
	{
		foundButtons["Start_Button"].gameObject.SetActive(true);
		foundButtons["Settings_Button"].gameObject.SetActive(true);
		foundButtons["Skills_Button"].gameObject.SetActive(true);
		foundButtons["Achivments_Button"].gameObject.SetActive(true);

		ActsPanel.SetActive(false);
		SettingsPanel.SetActive(false);
	}

	public void StartAct(int i)
	{
		switch (i)
		{
			case 1:
				ActsPanel.SetActive(false);
				ActOnePanel.SetActive(true);
				break;

			case 2:
				ActsPanel.SetActive(false);
				//ActSecPanel.SetActive(true);
				break;

			case 3:
				ActsPanel.SetActive(false);
				//ActThirdPanel.SetActive(true);
				break;

			default:
				Debug.LogWarning("Не удалось запустить акт");
				break;
		}
	}

	public void BackToActs()
	{
		ActOnePanel.SetActive(false);
		//ActSecPanel.SetActive(false);
		//ActThirdPanel.SetActive(false);

		ActsPanel.SetActive(true);
	}

	public void QuitGame()
	{
		// Если игра запущена в редакторе Unity
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				// В собранной версии игры
				Application.Quit();
		#endif

		// Можно добавить логирование
		Debug.Log("Игра завершена");
	}
}