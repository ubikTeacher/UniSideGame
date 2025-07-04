using UnityEngine;
using UnityEngine.UI; //UIの部品使用しているので入れておく
using System.Collections;
using System.Collections.Generic;
using TMPro;//TextMeshPro用

public class GameManager : MonoBehaviour
{
    //サウンド設定追加
    public AudioClip acGameOver;//ゲームオーバー
    public AudioClip acGameClear;//ゲームクリア

    //スコア追加
    public GameObject scoreText;  //スコアテキスト格納用
    public static int totalScore; //合計スコア
    public int stageScore = 0;    //ステージスコア

    //ゲームオーバー用画像設定用
    public Sprite gameOverSpr;
    //ゲームクリア用画像設定用
    public Sprite gameClearSpr;
    //パネル格納用
    public GameObject panel;
    //リスタートボタン格納用
    public GameObject restartBtn;
    //ネクストボタン格納用
    public GameObject nextBtn;
    //画像を持つゲームオブジェクト格納用
    public GameObject mainImage;
    public GameObject mainImage2;
    //タイムバー格納用
    public GameObject timeBar;
    //残り時間表示テキスト格納用
    public GameObject timeText;

    //タイムコントローラースクリプト格納用
    private TimeController timeCnt;

    //プレイヤー操作
    public GameObject InputUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GAMESTARTを非表示に設定
        Invoke("SetActiveMainImage", 1.0f);
        //パネルを非表示に設定
        this.panel.SetActive(false);

        //タイムコントローラ取得
        this.timeCnt
            = GetComponent<TimeController>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerController.gameState);
        if (PlayerController.gameState == "gameclear")
        {

            if (InputUI != null) InputUI.SetActive(false);
            //ゲームクリアになったら
            //ゲームクリアの画像を表示する
            mainImage.SetActive(true);

            //ボタンが入ってるパネルも表示
            panel.SetActive(true);

            //リスタートボタンは押せないようにする
            Button rbtn = restartBtn.GetComponent<Button>();
            rbtn.interactable = false;

            //メインイメージの画像をGameClearの画像に切り替える
            Image mimg = mainImage.GetComponent<Image>();
            mimg.sprite = this.gameClearSpr;

            //ステータスをゲーム終了にする
            PlayerController.gameState = "gameend";

            //制限時間カウントを止める
            if (this.timeCnt != null)
            {
                this.timeCnt.isTimeOver = true;
            }

            //クリア音再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //今流しているBGMを止める
                soundPlayer.Stop();
                //ゲームクリアの音を鳴らす
                soundPlayer.PlayOneShot(this.acGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            if(InputUI!=null)InputUI.SetActive(false);

            //ゲームオーバーになったら
            //ゲームオーバーの画像を表示する
            mainImage.SetActive(true);

            //ボタンが入ってるパネルも表示
            panel.SetActive(true);

            // ネクストボタンは押せないようにする
            Button nbtn = nextBtn.GetComponent<Button>();
            nbtn.interactable = false;

            //メインイメージの画像をGameOverの画像に切り替える
            Image mimg = mainImage.GetComponent<Image>();
            mimg.sprite = this.gameOverSpr;
            if (this.mainImage2 != null) 
            {
                mimg.transform.position = new Vector2(mimg.transform.position.x + 4, mimg.transform.position.y);
            }
            //ステータスをゲーム終了にする
            PlayerController.gameState = "gameend";

            //制限時間カウントを止める
            if (this.timeCnt != null)
            {
                this.timeCnt.isTimeOver = true;
            }

            //ゲームオーバー音再生
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //今流しているBGMを止める
                soundPlayer.Stop();
                //ゲームクリアの音を鳴らす
                soundPlayer.PlayOneShot(this.acGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            GameObject player
                = GameObject.FindGameObjectWithTag("Player");
            PlayerController pc
                = player.GetComponent<PlayerController>();
            //スコア更新
            if (pc.score != 0)
            {
                this.stageScore
                    += pc.score;
                pc.score = 0;
                UpdateScore();
            }

            if (this.timeCnt != null)
            {
                //タイムテキストを更新
                this.timeText.GetComponent<TMP_Text>().text
                    = this.timeCnt.displayTime.ToString("F1");
                if(this.timeCnt.displayTime==0)
                {
                    pc.GameOver();
                }
            }
        }

        if (PlayerController.isGetTime == true)
        {
            this.timeCnt.displayTime += 10;
            PlayerController.isGetTime = false;
        }
    }

    public void Jump()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerCnt = player.GetComponent<PlayerController>();
        playerCnt.Jump();
    }

    /// <summary>
    /// スコアを更新する
    /// </summary>
    void UpdateScore()
    {
        int score = this.stageScore + totalScore;
        this.scoreText.GetComponent<TMP_Text>().text
                            = score.ToString();
    }


    /// <summary>
    /// メイン画像を非表示にします
    /// </summary>
    void SetActiveMainImage()
    {
        this.mainImage.SetActive(false);
        if (this.mainImage2 != null) 
        {
            this.mainImage2.SetActive(false);
        }
    }
}