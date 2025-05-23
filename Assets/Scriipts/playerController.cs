using UnityEngine;
using System. Collections;
using System. Collections.Generic;
///<summary>
///プレイヤーを操作するクラス
///</summary>
public class playerController : MonoBehaviour
{
    ///<summary>
    /// ゲームの状態
    /// playing:ゲーム中
    /// gameclear:クリア状態
    /// gameover:ゲームオーバー
    ///</summary>
    public static string gameState = "playing";
    ///プレイヤーの重力部品を入れる変数

    private Rigidbody2D rbody;

    ///水平方向の入力取得用の変数

    private float inputH=0.0f;

    ///<summary>
    ///ジャンプ力設定用
    ///</summary>
    private float jump = 5.0f;

    ///<summary>
    ///ジャンプが押されたかどうか
    /// True:ジャンプ False:ジャンプでない
    ///</summary>
    private bool isJump = false;

    ///<summary>
    /// groundレイヤー設定
    ///</summary>
    public LayerMask groundLayer;

    ///<summary>
    ///アニメーション用の部品
    ///</summary>
    private Animator animetor;
    public string stopAnime = "playerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "playerGoal";
    public string overAnime = "playerOver";

    private string nowAnime = ""; //今のアニメーション
    private string oldAnime = ""; //1つ前のアニメーション

    public int score = 0; //スコア

    // 始めに1回だけ実行
    void Start()
    {
        //Rigidbody2Dをプライベート変数に入れる
        this.rbody = this.GetComponent<Rigidbody2D>();
        //Animatorをプライベート変数に入れておく
        this.animetor = this.GetComponent<Animator>();

        this.nowAnime=this.stopAnime;
        this.oldAnime=this.stopAnime;

        //状態をプレイ中に設定
        gameState = "playing";
    }

    // 何度も実行
    void Update()
    {
        if(gameState!="playing")
        {
            return;
        }
        //水平方向の入力があるかを取得する
        //←を押されたら-1、→を押されたら+1が入る
        this.inputH = Input.GetAxisRaw("Horizontal");
        //画像の向きを設定
        if(this.inputH == -1)
        {
            //左に進むとき
            transform.localScale
            =new Vector2(-1,1);
            //this.nowAnime=this.moveAnime;
        }
        else if(this.inputH ==1)
        {
            //右に進むとき
            transform.localScale
            =new Vector2(1,1);
            //this.nowAnime=this.moveAnime;
        }
        else if(this.inputH==0)
        {
            //this.nowAnime=this.stopAnime;
        }

        //ジャンプボタンがおされたか
        if(Input.GetButtonDown("Jump")==true)
        {
            //ジャンプ中に設定
            this.isJump=true;
        }
    }

    // 一定の間隔で何度も実行
    void FixedUpdate()
    {
        if(gameState!="playing")
        {
            return;
        }
        this.rbody.linearVelocity
        =new Vector2(this.inputH*5.0f
        , this.rbody.linearVelocityY);

        //地面かどうかフラグ
        bool isGround = false;

        isGround=Physics2D.CircleCast(
            transform.position
            ,0.2f
            ,Vector2.down
            ,0.0f
            ,this.groundLayer
        );
        
        //ジャンプ中にフラグが立っているかチェック
        if(this.isJump==true && isGround==true)
        {
            //ジャンプボタン押された+地面にいる

            //ジャンプのベクトルを作る
            Vector2 jumpPw = new Vector2(0, jump);

            //瞬間的にプレイヤーにその力を加える
            this.rbody.AddForce(jumpPw
            , ForceMode2D.Impulse);

            //ジャンプ中フラグをまたオフにしておく
            this.isJump=false;
        }

        //アニメーション設定
        if(isGround==true)
        {
            //地面にいる
            if(this.inputH==0)
            {
                this.nowAnime=this.stopAnime;
            }
            else
            {
                this.nowAnime=this.moveAnime;
            }
        }
        else
        {
            //空中にいる
            this.nowAnime=this.jumpAnime;
        }
        if(this.nowAnime!=this.oldAnime)
        {
            this.animetor.Play(this.nowAnime);
            this.oldAnime = this.nowAnime;
        }
    }
    ///<summary>
    /// 当たったとき呼び出される
    ///</summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        //ぶつかった物体のタグがGoalかチェック
        if(collision.gameObject.tag=="goal")
        {
            GameClear();
        }
        //ぶつかった物体のタグがDADEかチェック
        if(collision.gameObject.tag=="DEAD")
        {
            GameOver();
        }
        //ぶつかった物体のタグがScoreItemかチェック
        if(collision.gameObject.tag=="ScoreItem")
        {
            GetItem(collision);

        }
    }

    ///<summary>
    /// Itemをゲットしたときの処理
    ///</summary>
    public void GetItem(Collider2D collision)
    {
        ItemDate item = collision.gameObject.
            GetComponent<ItemDate>();

        this.score = item.value;

        //破棄する(消す)
        Destroy(collision.gameObject);
    }

    ///<summary>
    /// ゲームクリアの処理
    ///</summary>
    public void GameClear()
    {
        Debug.Log("Goal!!");
        //ゴールアニメーション再生
        this.animetor.Play(goalAnime);
        gameState="gameclear";
        //速度を0にして強制的に停止
        this.rbody.linearVelocity
        =new Vector2(0,0);
    }

    ///<summary>
    /// ゲームオーバーの処理
    ///</summary>
    public void GameOver()
    {
        Debug.Log("YOUDEAD!!");
        //ゲームオーバーのアニメーション再生
        this.animetor.Play(this.overAnime);
        gameState="gameover";
        //速度を0にして強制的に停止
        this.rbody.linearVelocity
        =new Vector2(0,0);

        //プレイヤーを少し上にあげてから
        //落とす
        this.rbody.AddForce(new Vector2(0,5),ForceMode2D.Impulse);

        //プレイヤーのコライダーを無効にする(落ちた時に跳ねる回数を1回にする)
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
