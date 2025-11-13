using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Spawner spawner;  // Spawner参照
    [SerializeField] private SceneController sceneController; // シーン遷移制御
    private bool isGameOver = false;

    public int score = 0;                       //現在のスコア

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (spawner == null)
            spawner = FindObjectOfType<Spawner>();

        if (sceneController == null)
            sceneController = FindObjectOfType<SceneController>();
    }


    public void AddScore(int lines)
    {
        int add = 0;

        switch (lines)
        {
            case 1: add = 100; break;
            case 2: add = 300; break;
            case 3: add = 500; break;
            case 4: add = 800; break;
        }

        score += add;

    }

    public void CheckGameOver()
    {
        // スポーン位置にすでにブロックがある場合はゲームオーバー
        for (int x = 0; x < Board.width; x++)
        {
            for (int y = Board.height - 1; y >= Board.height - 2; y--) // 上2行をチェック
            {
                //  範囲チェック
                if (x < 0 || x >= Board.width || y < 0 || y >= Board.height)
                    continue;

                if (Board.grid[x, y] != null)
                {
                    Debug.Log(" Game Over!");
                    OnGameOver();
                    return;
                }
            }
        }
    }

    private void OnGameOver()
    {
        Debug.Log(" GAME OVER!");

        // 時間停止
        Time.timeScale = 0f;

        // SceneController経由でリザルトへ
        if (SceneController.Instance != null)
        {
            ScoreData.LastScore = score;
            SceneController.Instance.LoadScene("ResultScene");
        }
        else
            Debug.LogWarning("SceneController が見つかりません！");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
