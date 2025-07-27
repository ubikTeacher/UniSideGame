using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;


///<summary>
///プレイヤーを操作するクラス
///</summary>
public class PlayerController : MonoBehaviour
{
    public InputActionAsset actions; // インスペクターからアタッチするため

    private InputAction moveAction;
    private InputAction jumpAction;

    void OnEnable()
    {
        moveAction = actions.FindAction("Move");
        moveAction.Enable();
        jumpAction = actions.FindAction("Jump"); // "Player/Jump"のようなパスも使える
        jumpAction.Enable();
        jumpAction.performed += OnJumpPerformed;

    }
    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.Disable();

    }
    //サウンド設定追加
    public AudioClip acGetitem;//ゲットアイテム
    public AudioClip acJump;//ジャンプ音
    public AudioClip acWarp;//ワープ音

    public GameObject shieldText;
    public bool isShield = false;
    //タッチスクリーン対応追加
    bool isMoving = false;

    /// <summary>
    /// ゲームの状態
    /// playing:ゲーム中
    /// gameclear:クリア状態
    /// gameover:ゲームオーバー
    /// </summary>
    public static string gameState = "playing";

    ///<summary>
    ///プレイヤーの重力部品を入れる変数
    private Rigidbody2D rbody;
    ///<summary>
    ///水平方向の入力取得用の変数
    ///<summary>
    private float inputH = 0.0f;

    /// <summary>
    /// ジャンプ力設定用
    /// </summary>
    public float jump = 5.0f;

    /// <summary>
    /// スピードスピード設定用
    /// </summary>
    public float speed = 8.0f;

    /// <summary>
    /// ジャンプ押されたかどうか
    /// True:ジャンプ False:ジャンプでない
    /// </summary>
    private bool isJump = false;

    /// <summary>
    /// groundレイヤー設定用
    /// </summary>
    public LayerMask groundLayer;

    /// <summary>アニメーション用部品</summary>
    private Animator animator;
    public string stopAnime = "PlayerStop";
    public string moveAnime = "PlayerMove";
    public string jumpAnime = "PlayerJump";
    public string goalAnime = "PlayerGoal";
    public string overAnime = "PlayerOver";
    public static bool isGetTime = false;

    private string nowAnime = ""; //今のアニメーション
    private string oldAnime = ""; //１つ前のアニメーション

    public int score = 0; //スコア
    bool isShieldOn = false;
    float keikajikan = -1.0f;
    float Shieldcount = 2;

    //アイテムの指定数を取得したらゴールとする
    public bool isClearItemCount=false;
    public int GoalCount = 5;
    private int ItemCount = 0;

    //ゴールしたときのボーナスポイント
    public int goalPoint = 100;

    public GameObject pointTextPrefab; // 作成したプレハブをInspectorで設定

    private int warpCnt = 0; // ワープカウント用
    /// 初めに1回だけ実行される
    void Start()
    {
        //Rigidbody2Dをプライベート変数に入れておく
        this.rbody = this.GetComponent<Rigidbody2D>();
        //Animatorをプライベート変数に入れておく
        this.animator = this.GetComponent<Animator>();

        this.nowAnime = this.stopAnime;
        this.oldAnime = this.stopAnime;
        if(this.shieldText!=null)
        {
            this.shieldText.GetComponent<TMP_Text>().text
             = this.Shieldcount.ToString();
        }
 

        //状態をプレイ中に設定
        gameState = "playing"; 
    }

    // 何度も実行される
    void Update()
    {
        if(gameState!="playing")
        {
            return;
        }
        if(isShield)
        {
            if (Keyboard.current[Key.Z].isPressed && keikajikan == -1 && Shieldcount > 0)
            {
                keikajikan = 0;
                isShieldOn = true;
                Shieldcount -= 1;
                if (this.shieldText != null)
                {
                    this.shieldText.GetComponent<TMP_Text>().text
                     = this.Shieldcount.ToString();
                }
            }
            if (keikajikan > -1)
            {
                keikajikan += Time.deltaTime;
                if (keikajikan > 3)
                {
                    keikajikan = -1;
                    isShieldOn = false;
                }
            }
        }

        //移動
        if (isMoving == false)
        {
            Vector2 move = moveAction.ReadValue<Vector2>();
            this.inputH = move.x;
        }

        //画像の向きを設定
        if (this.inputH == -1)
        {
            //左に進むとき
            transform.localScale
                = new Vector2(-1, 1);
            //this.nowAnime = this.moveAnime;
        }
        else if(this.inputH ==1)
        {
            //右に進むとき
            transform.localScale
                = new Vector2(1, 1);
            //this.nowAnime = this.moveAnime;
        }
        else if(this.inputH == 0)
        {
            //this.nowAnime = this.stopAnime;
        }
    }

    public void SetAxis(float h, float v)
    {
        this.inputH = h;
        if(this.inputH==0)
        {
            isMoving = false;
        }else
        {
            isMoving = true;
        }
    }

    //一定の間隔で何度も実行される
    void FixedUpdate()
    {
        if (gameState != "playing")
        {
            return;
        }
        this.rbody.linearVelocity
        = new Vector2(this.inputH * this.speed
        ,this.rbody.linearVelocityY);

        //地面かどうかフラグ
        bool isGround = false;

        //地面のレイヤーと
        //接触しているかを取得する
        isGround = Physics2D.CircleCast(
               transform.position
               , 0.2f
               , Vector2.down
               , 0.0f
               , this.groundLayer
            );

        //ジャンプ中フラグが立っているかチェック
        if(this.isJump==true && isGround==true)
        {
            //ジャンプボタン押された＋地面にいる

            //ジャンプのベクトルを作る
            Vector2 jumpPw = new Vector2(0, jump);

            //瞬間的にプレイヤーにその力を加える
            this.rbody.AddForce(jumpPw
                , ForceMode2D.Impulse);
                //音をならす
            //ジャンプ音を再生
            if (acJump != null)
            {
                AudioSource soundPlayer = GetComponent<AudioSource>();
                if (soundPlayer != null)
                {
                    //ジャンプ音を鳴らす
                    soundPlayer.PlayOneShot(this.acJump);
                }
            }

            //ジャンプ中フラグをまたオフにしておく
            this.isJump = false;
        }

        //アニメーション設定
        if (isGround == true)
        {
            //地面にいる
            if(this.inputH==0)
            {
                this.nowAnime = this.stopAnime;
            }
            else
            {
                this.nowAnime = this.moveAnime;
            }
        }else
        {
            //空中にいる
            this.nowAnime = this.jumpAnime;
        }

        //前回再生したアニメーションと
        //今回再生するアニメーションが
        //違っている時だけ再生する
        if(this.nowAnime != this.oldAnime)
        {
            this.animator.Play(this.nowAnime);
            this.oldAnime = this.nowAnime;
        }

    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Jump();
    }

    public void Jump()
    {
        //ジャンプ中に設定
        this.isJump = true;        
    }

    /// <summary>
    /// 当たったときに呼び出される
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (gameState == "gameend")
        {
            return;
        }
        //ぶつかった物体のタグがGoalかチェック
        if (collision.gameObject.tag == "Goal")
        {
            //アイテムカウントの数がゴールカウント以上になったら、ゲームクリアに
            if (this.isClearItemCount && this.ItemCount >= GoalCount)
            {
                GameClear();
            }
            //アイテムを使わない人に
            if (this.isClearItemCount == false) 
            {
                GameClear();
            }

            // プレハブを指定位置に生成
            GameObject popup = Instantiate(pointTextPrefab
                                            , collision.gameObject.transform.position
                                            , Quaternion.identity);

            // テキストを設定
            //TitleManager.Score += this.goalPoint;
            TextMeshProUGUI text = popup.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "+" + this.goalPoint.ToString();

            // 上方向に移動しながらフェードアウト
            popup.transform.Translate(Vector3.up * 1.0f);
            Destroy(popup, 3.0f); // 1.5秒後に削除
        }
        //ぶつかった物体のタグがDeadかチェック
        if (collision.gameObject.tag == "Dead")
        {
            if (isShieldOn == true)
            {

            }
            else
            {
                GameOver();
            }
        }
        //ぶつかった物体のタグがScoreItemかチェック
        if (collision.gameObject.tag == "ScoreItem")
        {
            GetItem(collision);
            //音をならす
            //ゲットアイテム音を再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //ゲットアイテムの音を鳴らす
                soundPlayer.PlayOneShot(this.acGetitem);
            }

            //アイテムカウントを増やす
            if (this.isClearItemCount == true) 
            {
                this.ItemCount += 1;
            }

            
        }

        //ぶつかった物体のタグがWarpかチェック
        if (collision.gameObject.tag == "Warp")
        {
            if (collision.gameObject.name=="Warp")
            {
                warpCnt += 1;
                if(warpCnt== 1)
                {
                    GameObject yajirusi = GameObject.Find("yazirusi_0 (1)");
                    if(yajirusi != null)
                    {
                        //ワープカウントが2回以上なら、ワープできない
                        Vector3 scale = yajirusi.transform.localScale; // コピーを取得
                        scale.x = scale.x*-1;                          // 値を変更

                        yajirusi.transform.localScale = scale;
                    }
                }
            }

            this.transform.position = new Vector3(collision.gameObject.GetComponent<WarpContoroller>().WarpX,
             collision.gameObject.GetComponent<WarpContoroller>().WarpY, 0);
            collision.gameObject.GetComponent<WarpContoroller>().yazirusi_hyouzi();
            //音をならす
            //ジャンプ音を再生
            if (acWarp != null)
            {
                AudioSource soundPlayer = GetComponent<AudioSource>();
                if (soundPlayer != null)
                {
                    //ジャンプ音を鳴らす
                    soundPlayer.PlayOneShot(this.acWarp);
                }
            }
        }

        //ぶつかった物体のタグがScoreItemかチェック
            if (collision.gameObject.tag == "TimeItem")
            {
                isGetTime = true;
                Destroy(collision.gameObject);
            }
    }

    /// <summary>
    /// Itemをゲットしたときの処理
    /// </summary>
    public void GetItem(Collider2D collision)
    {
        ItemData item = collision.gameObject.
                GetComponent<ItemData>();

        this.score = item.value;

        // プレハブを指定位置に生成
        GameObject popup = Instantiate(pointTextPrefab
                                        , collision.gameObject.transform.position
                                        , Quaternion.identity);

        // テキストを設定
        TextMeshProUGUI text = popup.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "+" + this.score.ToString();

        // 上方向に移動しながらフェードアウト
        popup.transform.Translate(Vector3.up * 1.0f);
        Destroy(popup, 1.5f); // 1.5秒後に削除
                              //破棄する（消す）
        Destroy(collision.gameObject);
    }


    /// <summary>
    /// ゲームクリアの処理
    /// </summary>
    public void GameClear()
    {
        //Debug.Log("Goal!!");
        //ゴールのアニメーション再生
        this.animator.Play(this.goalAnime);
        gameState = "gameclear";
    }

    /// <summary>
    /// ゲームオーバーの処理
    /// </summary>
    public void GameOver()
    {
        //Debug.Log("GameOver!!");
        //ゲームオーバーのアニメーション再生
        this.animator.Play(this.overAnime);
        gameState = "gameover";

        //速度を0にして強制的に停止
        this.rbody.linearVelocity
            = new Vector2(0, 0);

        //プレイヤーを少し上にあげてから
        //落とす！
        this.rbody.AddForce(new Vector2(0, 5)
                        , ForceMode2D.Impulse);

        //プレイヤーのコライダーを無効にする
        GetComponent<CapsuleCollider2D>().enabled
            = false;
    }
}
