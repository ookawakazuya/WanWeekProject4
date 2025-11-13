using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    [Header("シーン設定")]
    [Tooltip("タイトルシーン名")]
    public string titleScene = "";

    [Tooltip("メインゲームシーン名")]
    public string mainScene = "";

    [Tooltip("リザルトシーン名")]
    public string resultScene = "";
    public static SceneController Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでも維持
    }
    public void GoToTitle()
    {
        LoadScene(titleScene);
    }

    public void StartGame()
    {
        LoadScene(mainScene);
    }

    public void GoToResult()
    {
        LoadScene(resultScene);
    }

    public void RetryGame()
    {
        LoadScene(mainScene);
    }

    public void QuitGame()
    {
        Debug.Log("アプリを終了します");
        Application.Quit();
    }


    // 実際の読み込み処理
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("シーン名が設定されていません！");
            return;
        }

        Debug.Log($"シーンをロード: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }


}
