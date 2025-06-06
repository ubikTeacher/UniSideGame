using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonController : MonoBehaviour
{
    //鉄砲玉用のプレハブ
    public GameObject bulletPrefab;

    //何秒待ってから発射するかの設定
    public float delayTime = 3.0f;

    //鉄砲玉の速さ
    public float speed = 4.0f;

    //鉄砲玉発射開始の距離
    public float kyori = 8.0f;

    //プレイヤーオブジェクト格納用
    GameObject player;

    //ゲートのトランスフォーム格納用
    Transform gateTransform;

    //経過時間
    float keikaJikan = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーを取得
        this.player
         = GameObject.FindGameObjectWithTag("Player");

        //ゲートのトランスフォーム取得
        this.gateTransform 
            = transform.Find("gate");
    }

    // Update is called once per frame
    void Update()
    {
        //経過時間計算
        this.keikaJikan += Time.deltaTime;

        //ターゲットとの射程距離チェック
        if(IsStart(this.player.transform.position))
        {
            //経過時間が指定時間を超えたら発射
            if(this.keikaJikan > this.delayTime)
            {
                //砲弾を作る(位置)
                Vector2 pos
                    = new Vector2(this.gateTransform.position.x
                        , this.gateTransform.position.y);

                //砲弾を作る(実態)
                GameObject bullet
                    = Instantiate(this.bulletPrefab
                                    , pos
                                    , Quaternion.identity);

                //砲撃台が向いている方向に発射する
                Rigidbody2D rbody
                    = bullet.GetComponent<Rigidbody2D>();
                float kakudoZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(kakudoZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(kakudoZ * Mathf.Deg2Rad);
                Vector2 v = new Vector2(x, y) * this.speed;

                //砲弾発射！
                rbody.AddForce(v, ForceMode2D.Impulse);
                //経過時間リセット
                this.keikaJikan = 0; 
            }
        }
    }

    /// <summary>
    /// ターゲットのポジションと自分の距離を調べて
    /// 設定距離以下になればスタート！(true)
    /// それ以外はスタートしない(false)
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    bool IsStart(Vector2 targetPos)
    {
        //ターゲットとの距離を取得
        float k=Vector2.Distance(transform.position
                                , targetPos);
        if(k < this.kyori)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position
                                , this.kyori);
    }
}
