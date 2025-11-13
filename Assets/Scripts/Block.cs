using UnityEngine;

public class Block : MonoBehaviour
{
    private float fallTime = 0.8f;
    private float fallTimer = 0f;

    void Update()
    {
        HandleInput();

        if (transform.childCount == 0)
            Destroy(gameObject);
    }

    void HandleInput()
    {
        // 左右移動 
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left * Board.cellSize;

            // 壁 or 他ブロックに当たるなら戻す
            if (!ValidMove())
                transform.position -= Vector3.left * Board.cellSize;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right * Board.cellSize;

            if (!ValidMove())
                transform.position -= Vector3.right * Board.cellSize;
        }

        //  回転 
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Rotate(0, 0, 90);

            if (!ValidMove())
                transform.Rotate(0, 0, -90);
        }

        //  ソフトドロップ 
        if (Input.GetKey(KeyCode.S))
            fallTimer += Time.deltaTime * 10;

        //  自動落下 
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallTime)
        {
            transform.position += Vector3.down * Board.cellSize;

            // 落下時のみグリッドに合わせる
            transform.position = Board.RoundVector(transform.position);

            if (!ValidMove())
            {
                // 一段戻す
                transform.position += Vector3.up * Board.cellSize;

                Board.AddToGrid(transform);
                Board.CheckLines();

                enabled = false;

                FindObjectOfType<Spawner>().SpawnNext();
                FindObjectOfType<GameManager>().CheckGameOver();
            }

            fallTimer = 0;
        }
    }

    //  範囲 & 衝突チェック
    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = Board.RoundVector(child.position);
            Vector2 local = pos - (Vector2)Board.origin;

            int x = Mathf.RoundToInt(local.x / Board.cellSize);
            int y = Mathf.RoundToInt(local.y / Board.cellSize);

            // X範囲チェック
            if (x < 0 || x >= Board.width)
                return false;

            // Y範囲チェック
            if (y < 0)
                return false;

            // ミノ衝突チェック
            if (y < Board.height && Board.grid[x, y] != null)
                return false;
        }
        return true;
    }


}
