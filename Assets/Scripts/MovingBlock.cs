using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    float movep = 0.0f;                // 補間用の値
    public float times = 2.0f;         // 移動にかかる時間（秒）
    public float wait = 1.0f;          // 停止時間（秒）
    bool isReverse = false;           // 方向を反転するか

    Vector3 startPos;                 // 開始位置
    Vector3 endPos;                   // 移動先位置

    public bool isMoveOn = false;     // 乗ったら動く？
    public bool isMoving = true;      // 今動いてるかどうか

    public float moveX = 3.0f;        // X軸移動距離
    public float moveY = 0.0f;        // Y軸移動距離（今回は0で左右のみ）

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(startPos.x + moveX, startPos.y + moveY, startPos.z);

        if (isMoveOn) isMoving = false;
    }

    void Update()
    {
        if (!isMoving) return;

        float distance = Vector2.Distance(startPos, endPos);
        float speed = distance / times;
        float delta = speed * Time.deltaTime;
        movep += delta / distance;

        // 補間で移動
        transform.position = isReverse
            ? Vector2.Lerp(endPos, startPos, movep)
            : Vector2.Lerp(startPos, endPos, movep);

        if (movep >= 1.0f)
        {
            movep = 0.0f;
            isReverse = !isReverse;
            isMoving = false;

            if (!isMoveOn)
            {
                Invoke(nameof(Move), wait);
            }
        }
    }

    public void Move()
    {
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 basePos = Application.isPlaying ? startPos : transform.position;
        Vector3 toPos = new Vector3(basePos.x + moveX, basePos.y + moveY, basePos.z);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(basePos, toPos);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Gizmos.DrawWireCube(basePos, sr.size);
            Gizmos.DrawWireCube(toPos, sr.size);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
            if (isMoveOn) isMoving = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}