using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingService : MonoBehaviour
{
	public static LoadingService Instance { get; private set; }
	[SerializeField] private GameObject loadingCanvasPrefab;
	private SceneLoader sceneLoader;

	void Awake()
	{
		if (Instance != null) { Destroy(gameObject); return; }
		Instance = this;
		DontDestroyOnLoad(gameObject);
		InstantiateCanvas();
	}

	void InstantiateCanvas()
	{
		if (loadingCanvasPrefab == null)
		{
			Debug.LogError("LoadingCanvasPrefab не назначен в LoadingService!");
			return;
		}
		GameObject canvas = Instantiate(loadingCanvasPrefab);
		DontDestroyOnLoad(canvas);
		sceneLoader = canvas.GetComponentInChildren<SceneLoader>(true);
		if (sceneLoader == null)
			Debug.LogError("SceneLoader не найден в префабе LoadingCanvas!");
	}

	// По имени сцены (без изменений)
	public void LoadScene(string sceneName, bool skipFadeIn = false)
	{
		sceneLoader?.LoadScene(sceneName, skipFadeIn);
	}

	// По индексу сцены (новая перегрузка)
	public void LoadScene(int buildIndex, bool skipFadeIn = false)
	{
		string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
		if (string.IsNullOrEmpty(path))
		{
			Debug.LogError($"Сцена с индексом {buildIndex} не найдена в Build Settings!");
			return;
		}
		string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
		sceneLoader?.LoadScene(sceneName, skipFadeIn);
	}
}