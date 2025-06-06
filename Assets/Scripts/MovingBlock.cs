using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingBlock : MonoBehaviour
{
    float movep = 0.0f;
    public float times=0.0f; //何秒で移動か
    public float wait = 0.0f;//停止時間
    bool isReverse = false;  //反転するか

    Vector3 startPos; //スタート位置
    Vector3 endPos; //移動位置

    //プレイヤーが乗ったら動くように
    //したいときはtrueにする
    public bool isMoveOn = false;

    //今動いてるかどうか判定用
    public bool isMoving = true;

    //移動させたい量（x軸とy軸）
    public float moveX = 3.0f;
    public float moveY = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期位置を取得
        this.startPos = this.transform.position;
        this.endPos= new Vector2(startPos.x + this.moveX
                   , startPos.y + this.moveY);

        if (this.isMoveOn == true)
        {
            //プレイヤーがのってから動く設定なので
            //始めは動かさない
            this.isMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isMoving)
        {
            //移動距離を計算
            float kyori
                = Vector2.Distance(startPos, endPos);
            Debug.Log(kyori);
            float ds = kyori / times;//1秒の移動距離
            float df = ds * Time.deltaTime;
            movep += df / kyori;
            if(isReverse)
            {
                //逆移動
                transform.position
              = Vector2.Lerp(endPos, startPos, movep);
            }
            else
            {
                //正移動
                transform.position
              = Vector2.Lerp(startPos, endPos, movep);
            }
            if(movep>=1.0f)
            {
                movep = 0.0f;//移動補完値をリセット
                this.isReverse=!this.isReverse;//移動を反転
                this.isMoving = false;//移動停止
                if(isMoveOn==false)
                {
                    //乗った時に動かない設定なら
                    Invoke("Move", wait);//移動フラグを遅延してたてる
                }
            }
        }
    }
    //移動フラグをONにする
    public void Move()
    {
        this.isMoving=true;
    }
    //移動フラグをOFFにする
    public void Stop()
    {
        this.isMoving = false;
    }
    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = this.transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos
             , new Vector2(fromPos.x + moveX
                         , fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        Gizmos.DrawWireCube(fromPos
                         , new Vector2(size.x, size.y));
        Vector2 toPos
        = new Vector3(fromPos.x + this.moveX
                    , fromPos.y + this.moveY);
        Gizmos.DrawWireCube(toPos
                         , new Vector2(size.x, size.y));
    }

    /// <summary>
    /// 当たったときに呼び出される
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        //ぶつかった物体のタグがプレイヤーかチェック
        if (collision.gameObject.tag == "Player")
        {
            //移動ブロックの子供にする
            collision.transform.SetParent(this.transform);

            if(this.isMoveOn==true)
            {
                // 乗ったら動く設定がONなら
                // 動かし始める
                this.isMoving = true;
            }
        }
    }

    /// <summary>
    /// 当たりおわった時に呼び出される
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerExit2D(Collider2D collision)
    {
        //ぶつかった物体のタグがプレイヤーかチェック
        if (collision.gameObject.tag == "Player")
        {
            //移動ブロックの子供をはずす
            collision.transform.SetParent(null);
        }
    }

}
