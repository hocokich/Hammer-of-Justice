using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	[Header("Общая кнопка Назад")]
	[SerializeField] private GameObject backButton;

	[Header("Затемнённая панель")]
	[SerializeField] private GameObject shadePanel;

	[Header("Панель Настроек")]
	[SerializeField] private GameObject StartPanel;
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

	IEnumerator Start()
	{
		Time.timeScale = 1f;

		FindAndAssignAllButtons();

		backButton.SetActive(false);

		//Don't touch
		yield return null;
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

			//Debug.Log($"Кнопка '{buttonName}' найдена и сохранена в {(targetDictionary == foundLvls ? "foundLvls" : "foundButtons")}");
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
		if (GameManager.Instance == null) return;

		for (int i = 0; i < buttonFirstActLvls.Count; i++)
		{
			Button button = GetFoundButton(buttonFirstActLvls[i].buttonName);
			if (button == null) continue;

			if (i == 0)
			{
				button.interactable = true;
				continue;
			}

			string prevSceneName = buttonFirstActLvls[i - 1].sceneName;
			bool isUnlocked = GameManager.Instance.GetLevelData(prevSceneName) != null;

			button.interactable = isUnlocked;
		}
	}

	public void LoadScene(string sceneName)
	{
		LoadingService.Instance.LoadScene(sceneName);
		//SceneManager.LoadScene(sceneName);
	}

	public void StartGame()
	{
		StartPanel.GetComponent<UIFader>().FadeOut(() =>
		{
			ActsPanel.GetComponent<UIFader>().FadeInFromHidden();
			RefreshBackButton();
		});
	}
	public void OpenSettings()
	{
		StartPanel.GetComponent<UIFader>().FadeOut(()=>
		{
			SettingsPanel.GetComponent<UIFader>().FadeInFromHidden();
			RefreshBackButton();
		});
	}

	public void BackToStart()
	{
		// Сразу начинаем скрывать кнопку
		HideBackButton();

		if (ActsPanel.activeSelf)
		{
			ActsPanel.GetComponent<UIFader>()?.FadeOut(() =>
			{
				ActsPanel.SetActive(false);
				_showStartPanel();
			});
		}
		else if (SettingsPanel.activeSelf)
		{
			SettingsPanel.GetComponent<UIFader>()?.FadeOut(() =>
			{
				SettingsPanel.SetActive(false);
				_showStartPanel();
			});
		}
		else
		{
			_showStartPanel();
		}
	}
	private void _showStartPanel()
	{
		StartPanel.GetComponent<UIFader>().FadeInFromHidden();
	}

	public void StartAct(int i)
	{
		switch (i)
		{
			case 1:
				ActsPanel.GetComponent<UIFader>().FadeOut(() =>
				{
					ActOnePanel.GetComponent<UIFader>().FadeInFromHidden();
					RefreshBackButton();
				});
				break;
				// аналогично для 2 и 3, когда они будут готовы
		}
	}
	public void BackToActs()
	{
		UIFader fader = ActOnePanel.GetComponent<UIFader>();
		if (fader != null)
			fader.FadeOut(() =>
			{
				ActOnePanel.SetActive(false);    // ← скрываем панель уровней
				ActsPanel.GetComponent<UIFader>().FadeInFromHidden();
				RefreshBackButton();            // кнопка «Назад» останется видимой
			});
		else
		{
			ActOnePanel.SetActive(false);
			ActsPanel.SetActive(true);
			RefreshBackButton();
		}
	}

	public void UniversalBack()
	{
		if (ActOnePanel.activeSelf)
			BackToActs();
		else if (ActsPanel.activeSelf || SettingsPanel.activeSelf)
			BackToStart();
	}
	private void HideBackButton()
	{
		// Скрываем кнопку и затемнённую панель одновременно
		if (backButton != null)
		{
			UIFader fader = backButton.GetComponent<UIFader>();
			if (fader != null)
				fader.FadeOut(() => backButton.SetActive(false));
			else
				backButton.SetActive(false);
		}

		if (shadePanel != null)
		{
			UIFader fader = shadePanel.GetComponent<UIFader>();
			if (fader != null)
				fader.FadeOut(() => shadePanel.SetActive(false));
			else
				shadePanel.SetActive(false);
		}
	}
	/// <summary> Показать кнопку «Назад», если мы не на стартовом экране </summary>
	private void RefreshBackButton()
	{
		bool onStart = StartPanel.activeSelf && !ActsPanel.activeSelf && !SettingsPanel.activeSelf && !ActOnePanel.activeSelf;

		// Показываем кнопку и затемнённую панель (или скрываем, если onStart)
		if (backButton != null)
		{
			UIFader fader = backButton.GetComponent<UIFader>();
			if (fader != null)
			{
				if (onStart)
					fader.FadeOut(() => backButton.SetActive(false));
				else
				{
					if (!backButton.activeSelf)
					{
						backButton.SetActive(true);
						fader.SetAlpha(0f);
					}
					fader.FadeIn();
				}
			}
			else
			{
				backButton.SetActive(!onStart);
			}
		}

		// То же самое для затемнённой панели
		if (shadePanel != null)
		{
			UIFader fader = shadePanel.GetComponent<UIFader>();
			if (fader != null)
			{
				if (onStart)
					fader.FadeOut(() => shadePanel.SetActive(false));
				else
				{
					if (!shadePanel.activeSelf)
					{
						shadePanel.SetActive(true);
						fader.SetAlpha(0f);
					}
					fader.FadeIn();
				}
			}
			else
			{
				shadePanel.SetActive(!onStart);
			}
		}
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