using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Board : MonoBehaviour
{

    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid;

    // ボード位置（左下座標）
    public static Vector3 origin;
    public static float cellSize = 1f;

    [Header("ボード設定")]
    [SerializeField] private float inspectorCellSize = 1f;
    [SerializeField] private Color gizmoColor = Color.gray;


    public static Board Instance { get; private set; }

    [Header("エフェクト")]
    public GameObject lineEffectPrefabInspector;
    public static GameObject lineEffectPrefab;

    public GameObject starEffectPrefabInspector;   // インスペクタ用

    public static GameObject starEffectPrefab;     // 静的参照


    void Awake()
    {
        Instance = this;
        // Awake時に現在のBoardオブジェクトの位置を基準点として記録
        origin = transform.position + new Vector3(0.5f,0.5f,0f);
        cellSize = inspectorCellSize;
        grid = new Transform[width, height]; //再初期化
        lineEffectPrefab = lineEffectPrefabInspector;       //エフェクトの設定
        starEffectPrefab = starEffectPrefabInspector;
    }

    // 座標がボード範囲内にあるか
    public static bool InsideBorder(Vector2 worldPos)
    {
        Vector2 localPos = worldPos - (Vector2)origin;
        // 小数点を正確に整数化
        float normalizedX = Mathf.Floor(localPos.x / cellSize + 0.0001f);
        float normalizedY = Mathf.Floor(localPos.y / cellSize + 0.0001f);

        // 範囲内ならtrue
        return (normalizedX >= 0 && normalizedX < width &&
                normalizedY >= 0);
    }

    // グリッド登録
    public static void AddToGrid(Transform block)
    {
        foreach (Transform child in block)
        {
            // ワールド座標 → ボード基準座標に変換
            Vector2 local = (Vector2)child.position - (Vector2)origin;

            // セル番号へ変換
            int x = Mathf.RoundToInt(local.x / cellSize);
            int y = Mathf.RoundToInt(local.y / cellSize);

            // 範囲チェック
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                grid[x, y] = child;
            }
            else
            {
                Debug.LogWarning($"Grid out of range: ({x}, {y})");
            }
        }
    }


    // グリッド上の行削除など（同じ）
    public static bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
                return false;
        }
        return true;
    }

    public static void DeleteLine(int y)
    {

        //エフェクトの再生
        if (lineEffectPrefab != null)
        {
            Vector3 effectPos = origin + new Vector3((width * cellSize) / 2f, y * cellSize, 0);
            GameObject fx = GameObject.Instantiate(lineEffectPrefab, effectPos, Quaternion.identity);
            GameObject.Destroy(fx, 1.0f);  // 1秒で削除
        }


        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public static void MoveLineDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //相対移動ではなく、正確なマス位置へ再配置
                Vector3 newPos = origin + new Vector3(x * cellSize, (y - 1) * cellSize, 0);
                grid[x, y - 1].position = newPos;
            }
        }
    }


    public static void MoveAllDown(int y)
    {
        for (int i = y + 1; i < height; i++)
        {
            MoveLineDown(i);
        }
    }

    public static void CheckLines()
    {

        int linescCleared = 0;
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                DeleteLine(y);
                MoveAllDown(y);
                linescCleared++;
                y--;
            }
        }

        if(linescCleared > 0)
        {
            GameManager.Instance.AddScore(linescCleared);
        }

        if (linescCleared == 4)
        {
            PlayStarEffect();
        }

        //最後に修正
        SnapAllBlocksToGrid();
    }

    private static void PlayStarEffect()
    {
        if (starEffectPrefab == null) return;

        // 表示位置（ボードの真上）
        Vector3 pos = origin + new Vector3(
            (width * cellSize) * 0.5f,
            (height * cellSize) + 1.0f,   // 少し上から降らせる
            0
        );

        GameObject fx = GameObject.Instantiate(starEffectPrefab, pos, Quaternion.identity);

        GameObject.Destroy(fx, 2.5f);  // エフェクト終了後に破棄
    }



    public static Vector2 RoundVector(Vector2 worldPos)
    {
        Vector2 local = worldPos - (Vector2)origin;

        // 世界座標 → グリッド整数座標へ
        int x = Mathf.RoundToInt(local.x / cellSize);
        int y = Mathf.RoundToInt(local.y / cellSize);

        // グリッド整数座標 → 世界座標に戻す
        return origin + new Vector3(x * cellSize, y * cellSize, 0);
    }


    // 可視化（Gizmos）
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 bottomLeft = transform.position;
        Vector3 topRight = bottomLeft + new Vector3(width * cellSize, height * cellSize, 0);

        // 外枠
        Gizmos.DrawLine(bottomLeft, bottomLeft + Vector3.up * height * cellSize);
        Gizmos.DrawLine(bottomLeft, bottomLeft + Vector3.right * width * cellSize);
        Gizmos.DrawLine(topRight, topRight - Vector3.right * width * cellSize);
        Gizmos.DrawLine(topRight, topRight - Vector3.up * height * cellSize);

        // 内側の補助線
        for (int x = 1; x < width; x++)
        {
            Gizmos.DrawLine(bottomLeft + Vector3.right * x * cellSize, bottomLeft + new Vector3(x * cellSize, height * cellSize, 0));
        }
        for (int y = 1; y < height; y++)
        {
            Gizmos.DrawLine(bottomLeft + Vector3.up * y * cellSize, bottomLeft + new Vector3(width * cellSize, y * cellSize, 0));
        }
    }

    /// <summary>
    /// ズレの修正
    /// </summary>
    public static void SnapAllBlocksToGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    Vector3 snapped = origin + new Vector3(x * cellSize, y * cellSize, 0);
                    grid[x, y].position = snapped;
                }
            }
        }
    }




}
