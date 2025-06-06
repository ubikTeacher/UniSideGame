using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GimmicBlock : MonoBehaviour
{
    /// <summary>
    /// 自動落下検知距離
    /// </summary>
    public float length = 0.0f;

    /// <summary>
    /// 落下後に削除するかどうかフラグ（True:落下後に削除)
    /// </summary>
    public bool isDelete = false;

    /// <summary>DEADオブジェクト格納用</summary>
    public GameObject deadObj;

    /// <summary>落ちたかどうか(初めはfalse)</summary>
    bool isFell = false;

    /// <summary>フェードアウトの時間</summary>
    float fadeoutTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //スタート時に落下しないように、
        //重力部品のタイプを変更しておく
        Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        
        //落下前にギミックの下部にふれても
        //死なないようにしておく
        deadObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのゲームオブジェクトを取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            //プレイヤーとギミックブロックの距離を取得
            float kyori=Vector2.Distance(this.transform.position
                                    , player.transform.position);
            
            //Debug.Log(kyori);
            if(length>=kyori)
            {
                //プレイヤーが射程圏内に入ったので
                //RigidbodyのbodyTypeを再度Dynamicに戻す！
                Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
                rbody.bodyType = RigidbodyType2D.Dynamic;

                //踏みつぶされたら死ぬように設定
                deadObj.SetActive(true);
            }
        }

        //落下したかどうか
        if(this.isFell)
        {
            //落下した場合は、透明にしていく
            this.fadeoutTime -= Time.deltaTime;

            //スプライトに設定されている
            //色を取得し、透明の設定値を変更する
            Color clr = this.GetComponent<SpriteRenderer>().color;
            
            //透明値を徐々に減らす
            clr.a = this.fadeoutTime;
            
            //透明値が0より小さくなったらギミックを消す
            if(this.fadeoutTime<0.0f)
            {
                Destroy(gameObject);
            }
        }

    }

    /// <summary>
    /// 当たったときに呼び出される
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("おちたかな？");
        if(this.isDelete)
        {
            //落下フラグをオンにする
            this.isFell = true;
        }
    }

    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position
                                , this.length);
    }

}
