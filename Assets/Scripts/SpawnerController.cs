//using UnityEngine;

//public class SpawnerController : MonoBehaviour
//{
//    public float moveSpeed = 5f;    //オブジェクトの移動速度
//    public float leftLimit = -8f;   //左の最大行動範囲
//    public float rightLimit = 8f;   //右の最大行動範囲



//    [Header("惑星設定")]
//    public GameObject[] planetPrefabs;  // 惑星のプレハブ配列
//    public Transform spawnPoint;        // 惑星を出す位置
//    public Transform previewPoint;      // プレビュー惑星の位置

//    // 生成レベル範囲
//    [Header("生成レベル範囲")]
//    [SerializeField] int minSpawnLevel = 1;
//    [SerializeField] int maxSpawnLevel = 3;


//    private GameObject previewInstance; // 現在のプレビュー惑星
//    public int currentPlanetIndex = 0;


//    void Start()
//    {
//        currentPlanetIndex = GetRandomSpawnIndex();
//        SpawnPreviewPlanet(); // 開始時にプレビュー惑星生成;
//    }

//    void Update()
//    {
//        HandleMovement();
//        HandleDrop();
//    }

//    void HandleMovement()
//    {
//        float move = Input.GetAxisRaw("Horizontal");
//        transform.position += Vector3.right * move * moveSpeed * Time.deltaTime;

//        // 端から反対側にワープ
//        if (transform.position.x < leftLimit)
//            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
//        else if (transform.position.x > rightLimit)
//            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);

//        // プレビュー惑星があれば一緒に動かす
//        if (previewInstance != null)
//        {
//            previewInstance.transform.position = previewPoint.position;
//        }
//    }
//    void HandleDrop()
//    {
//        if (Input.GetKeyDown(KeyCode.Space) && previewInstance != null)
//        {
//            // プレビューを落下させる
//            GameObject fallingPlanet = previewInstance;
//            previewInstance = null;

//            Rigidbody2D rb = fallingPlanet.GetComponent<Rigidbody2D>();
//            if (rb != null)
//            {
//                rb.isKinematic = false;
//                rb.gravityScale = 1f;
//            }

//            // 次の惑星を準備
//            currentPlanetIndex = Random.Range(0, planetPrefabs.Length);
//            SpawnPreviewPlanet();
//        }
//    }

//    void SpawnPreviewPlanet()
//    {
//        if (previewInstance != null)
//            Destroy(previewInstance);

//        GameObject prefab = planetPrefabs[currentPlanetIndex];
//        previewInstance = Instantiate(prefab, previewPoint.position, Quaternion.identity);

//        // プレビュー中は物理停止・透明度変更
//        Rigidbody2D rb = previewInstance.GetComponent<Rigidbody2D>();
//        if (rb != null)
//        {
//            rb.isKinematic = true;
//            rb.gravityScale = 0f;
//        }

//        Collider2D col = previewInstance.GetComponent<Collider2D>();
//        if (col != null) col.enabled = false;

//        SpriteRenderer sr = previewInstance.GetComponent<SpriteRenderer>();
//        if (sr != null)
//        {
//            Color c = sr.color;
//            c.a = 0.6f; // 半透明にしてプレビューっぽく
//            sr.color = c;
//        }
//    }

//    int GetRandomSpawnIndex()
//    {
//        // 範囲チェック（保険）
//        int min = Mathf.Clamp(minSpawnLevel, 0, planetPrefabs.Length - 1);
//        int max = Mathf.Clamp(maxSpawnLevel, min, planetPrefabs.Length - 1);

//        int index = Random.Range(min, max + 1);
//        return index;
//    }
//}
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float leftLimit = -8f;
    public float rightLimit = 8f;

    [Header("惑星設定")]
    public GameObject[] planetPrefabs; // 惑星プレハブ一覧（全レベル登録）
    public Transform spawnPoint;
    public Transform previewPoint;

    // 生成レベル範囲（
    [Header("生成レベル範囲")]
    [SerializeField] int minSpawnLevel = 1;
    [SerializeField] int maxSpawnLevel = 3;

    private int currentPlanetIndex = 0;
    private GameObject previewInstance;

    void Start()
    {
        // 最初にプレビュー惑星生成
        currentPlanetIndex = GetRandomSpawnIndex();
        SpawnPreviewPlanet();
    }

    void Update()
    {
        HandleMovement();
        HandleDrop();
    }

    void HandleMovement()
    {
        float move = Input.GetAxisRaw("Horizontal");
        transform.position += Vector3.right * move * moveSpeed * Time.deltaTime;

        // 端から反対側にワープ
        if (transform.position.x < leftLimit)
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        else if (transform.position.x > rightLimit)
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);

        // プレビュー惑星をSpawnerに追従
        if (previewInstance != null)
            previewInstance.transform.position = previewPoint.position;
    }

    void HandleDrop()
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // プレビューを落下開始
                GameObject fallingPlanet = previewInstance;
                previewInstance = null;

                Rigidbody2D rb = fallingPlanet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.gravityScale = 1f;
                }

                // コライダーを有効化
                Collider2D col = fallingPlanet.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = true;

                // 見た目を元に戻す
                SpriteRenderer sr = fallingPlanet.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = 1f;
                    sr.color = c;
                }

                // 次の惑星を準備
                currentPlanetIndex = GetRandomSpawnIndex();
                SpawnPreviewPlanet();
            }
    }

    void SpawnPreviewPlanet()
    {
        if (previewInstance != null)
            Destroy(previewInstance);

        GameObject prefab = planetPrefabs[currentPlanetIndex];
        previewInstance = Instantiate(prefab, previewPoint.position, Quaternion.identity);

        // プレビュー中は停止＆透明
        Rigidbody2D rb = previewInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.gravityScale = 0f;
        }

        Collider2D col = previewInstance.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        SpriteRenderer sr = previewInstance.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.6f; // 半透明にしてプレビューっぽく
            sr.color = c;
        }
    }

    // 1~3レベルだけランダムに生成
    int GetRandomSpawnIndex()
    {
        // 範囲チェック（保険）
        int min = Mathf.Clamp(minSpawnLevel, 0, planetPrefabs.Length - 1);
        int max = Mathf.Clamp(maxSpawnLevel, min, planetPrefabs.Length - 1);

        int index = Random.Range(min, max + 1);
        return index;
    }
}

