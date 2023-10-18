using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, BoxGroup("Settings")] private Animator _loaderAnimator;
    [SerializeField, BoxGroup("Settings")] private float _transitionTime; 
    [SerializeField, BoxGroup("Settings")] private float _waitingTime; 

    private static SceneLoader _instance;

    [SerializeField, Foldout("Events")] private UnityEvent _onSceneFullyLoaded;
    public event UnityAction OnSceneFullyLoaded { add => _onSceneFullyLoaded.AddListener(value); remove => _onSceneFullyLoaded.RemoveListener(value); }

    [SerializeField, Foldout("Events")] private UnityEvent _onSceneUnloaded;
    public event UnityAction OnSceneUnoaded { add => _onSceneUnloaded.AddListener(value); remove => _onSceneUnloaded.RemoveListener(value); }


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == 1)
            {
                return;
            }
        }

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadGameScene();
        }
    }

    private static IEnumerator LoadLevel(int buildIndex)
    {
        _instance._loaderAnimator.SetBool("Transition", true);
        yield return new WaitForSeconds(_instance._transitionTime);
        AsyncOperation asyncLoadLevel = null;

        // Unload scene exepte levelLoader
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex != 0)
            {
                asyncLoadLevel = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));

                while (!asyncLoadLevel.isDone)
                {
                    yield return null;
                }

                _instance._onSceneUnloaded?.Invoke();
            }
        }

        // Delay
        yield return new WaitForSeconds(_instance._waitingTime);

        // Load next scene
        asyncLoadLevel = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }

        _instance._loaderAnimator.SetBool("Transition", false);
        _instance._onSceneFullyLoaded?.Invoke();
    }

    private static void LoadSceneAtIndex(int index)
    {
        if (!_instance) throw new System.Exception("Scene Loader does not exist in this scene.");

        _instance.StartCoroutine(LoadLevel(index));
    }

    public static void LoadMenuScene() => LoadSceneAtIndex(1);

    public static void LoadGameScene() => LoadSceneAtIndex(2);

    public static void QuitGame() => Application.Quit();
}
