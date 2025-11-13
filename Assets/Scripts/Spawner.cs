using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("ブロックプレハブ一覧")]
    public GameObject[] blocks;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SpawnNext();
    }

    public void SpawnNext()
    {
        int index = Random.Range(0, blocks.Length);

        //Boardのoriginを基準に生成
        float spawnX = Mathf.Floor(Board.width / 2);  // 例: width 10 → 5
        float spawnY = Board.height - 1;
        Vector3 spawnPos = Board.origin + new Vector3(spawnX * Board.cellSize, spawnY * Board.cellSize, 0); GameObject newBlock = Instantiate(blocks[index], transform.position, Quaternion.identity);

        newBlock.transform.SetParent(Board.Instance.transform);


        // 生成直後にゲームオーバー判定を実行
        if (gameManager != null)
        {
            gameManager.CheckGameOver();
        }
    }
}
