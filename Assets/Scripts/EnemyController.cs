using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f; //移動速度

    public bool isToRight = false; //true:右向き　false:左向き

    public float revTime = 0.0f; //反転時間

    float time = 0; //反転時間はかるための変数

    public LayerMask groundLayer; //維持面レイヤー(地面判定用)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.isToRight)
        {
            //向きを反転させる
            this.transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.revTime > 0)
        {
            time += Time.deltaTime;
            if (time >= this.revTime)
            {
                //左右フラグを反転させる
                isToRight = !isToRight;
                //時間を初期化
                time = 0;
                if (this.isToRight)
                {
                    //向きを反転させる
                    this.transform.localScale = new Vector2(-1, 1);
                }
                else
                {
                    //向きを反転させる
                    this.transform.localScale = new Vector2(1, 1);
                }
            }
        }
    }

    void FixedUpdate()
    {
        ///地面かどうかフラグ
        bool isGround = false;

        ///地面とレイヤーと接触しているかを習得する
        isGround = Physics2D.CircleCast(transform.position, 0.5f, Vector2.down, 0.5f, this.groundLayer);

        if (isGround)
        {
            //地面の上にいるときだけ動かす
            //Rigidbody2Dの部品を取得する
            Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
            if (this.isToRight)
            {
                //右移動
                rbody.linearVelocity = new Vector2(speed, rbody.linearVelocityY);
            }
            else
            {
                //左移動
                rbody.linearVelocity = new Vector2(-speed, rbody.linearVelocityY);
            }
        }
    }
}
