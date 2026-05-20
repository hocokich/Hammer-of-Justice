using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class ButtonManager : MonoBehaviour
{
	[System.Serializable]
	public class ButtonAction
	{
		public string buttonName;        // Имя кнопки для поиска
		public UnityEngine.Events.UnityEvent onClickAction; // Методы при нажатии
	}

	[Header("Панель паузы")]
	[SerializeField] private GameObject PausePanel;

	[Header("Панель победы")]
	[SerializeField] private GameObject WinPanel;

	[Header("Настройки кнопок")]
	[SerializeField] private List<ButtonAction> buttonActions = new List<ButtonAction>();

	private Dictionary<string, Button> foundButtons = new Dictionary<string, Button>();

	void Start()=> FindAndAssignAllButtons();
	void FindAndAssignAllButtons()
	{
		foreach (var buttonAction in buttonActions)
		{
			FindAndAssignButton(buttonAction.buttonName, buttonAction.onClickAction);
		}
	}
	public void FindAndAssignButton(string buttonName, UnityEngine.Events.UnityEvent action)
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
			// Удаляем старые слушатели (опционально)
			button.onClick.RemoveAllListeners();

			// Добавляем новые методы
			button.onClick.AddListener(() => action?.Invoke());

			// Сохраняем в словарь
			if (!foundButtons.ContainsKey(buttonName))
			{
				foundButtons.Add(buttonName, button);
			}

			//Debug.Log($"Кнопка '{buttonName}' найдена и настроена");
		}
		else
		{
			Debug.LogWarning($"Кнопка с именем '{buttonName}' не найдена!");
		}
	}

	// Найти кнопку по имени
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
		return null;
	}

	//Показывает скрывает UI
	public void HideButtonsPanel()
	{
		if (PausePanel != null)
			PausePanel.SetActive(false);
	}
	public void ShowPausePanel()
	{
		if (PausePanel != null)
		{
			PausePanel.SetActive(true);
			// Переискать кнопки после активации панели
			RefreshButtons();
		}
	}

	//Показывает экран победы
	public void ShowWinPanel()
	{
		if (WinPanel != null)
		{
			WinPanel.SetActive(true);
			// Переискать кнопки после активации панели
			RefreshButtons();
		}
	}

	//Методы для загрузок сцен
	public void LoadScene(string sceneName)
	{
		LoadingService.Instance.LoadScene(sceneName);
	}
	public void NextLoadScene()
	{
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

		// Проверяем, существует ли следующая сцена
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
		{
			LoadingService.Instance.LoadScene(nextSceneIndex);
			//SceneManager.LoadScene();
		}
		else
		{
			LoadingService.Instance.LoadScene("menu");
			//SceneManager.LoadScene(0);
		}

		Time.timeScale = 1f; // Сбрасываем паузу если была
	}
	public void ReLoadScene()
	{
		LoadingService.Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}