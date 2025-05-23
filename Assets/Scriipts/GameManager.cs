using UnityEngine;
using UnityEngine.UI; //UIの部品を使用しているので入れておく
using System. Collections;
using System. Collections.Generic;
using TMPro;//TextMeshPro用

public class GameManager : MonoBehaviour
{
    //スコア追加
    public GameObject scoreText;  //スコアテキスト格納用
    public static int totalScore;  //合計スコア
    public int stageScore = 0;  //ステージスコア

    //ゲームオーバー用画像設定用
    public Sprite gameOverSpr;
    //ゲームクリア用画像設定用
    public Sprite gameClearSpr;
    //パネル格納用
    public GameObject panel;
    //リスタートボタン格納用
    public GameObject restarBtn;
    //ネクストボタン格納用
    public GameObject nextBtn;
    //画像を持つゲームオブジェクト格納用
    public GameObject mainImage;

    //タイマー格納用
    public GameObject timebar;
    //残り時間表示テキスト格納用
    public GameObject timeTxst;
    //タイムコントローラースクリプト格納用
    private TimeController TimeCut;

    // 始めに1回だけ実行
    void Start()
    {
        //GAMESTARTを非表示に設定
        Invoke("SetActiveMainImage", 1.0f);
        //パネルを非表示に設定
        this.panel.SetActive(false);

        //タイムコントローラー取得
        this.TimeCut=GetComponent<TimeController>();
    }

    // 何度も実行
    void Update()
    {
        //Debug.Log(playerController.gameState);
       if(playerController.gameState=="gameclear")
       {
        //ゲームクリアになったら
        //ゲームクリアの画像を表示する
        mainImage.SetActive(true);

        //ボタンが入ってるパネルも表示
        panel.SetActive(true);

        //リスタートボタンは押せないようにする
        Button rbtn = restarBtn.GetComponent<Button>();
        rbtn.interactable = false;

        //メインイメージの画像をGameClearの画像に切り替える
        Image mimg=mainImage.GetComponent<Image>();
        mimg.sprite=this.gameClearSpr;

        //ステータスをゲーム終了にする。
        playerController.gameState = "gameend";
        //制限時間カウントを止める
        if(this.TimeCut != null)
        {
            this.TimeCut.isTimeOver=true;
        }
       }
       else if(playerController.gameState=="gameover")
       {
        //ゲームオーバーになったら
        //ゲームオーバーの画像を表示する
        mainImage.SetActive(true);

        //ボタンが入ってるパネルも表示
        panel.SetActive(true);

        //ネクストボタンは押せないようにする
        Button nbtn = nextBtn.GetComponent<Button>();
        nbtn.interactable = false;

        //メインイメージの画像をGameOverの画像に切り替える
        Image mimg=mainImage.GetComponent<Image>();
        mimg.sprite=this.gameOverSpr;

        //ステータスをゲーム終了にする。
        playerController.gameState = "gameend";

        //制限時間カウントを止める
        if(this.TimeCut != null)
        {
            this.TimeCut.isTimeOver=true;
        }
       }
       else if(playerController.gameState=="playing")
       {
            if(this.TimeCut != null)
            {
                //タイムテキストを更新
                this.timeTxst.GetComponent<TMP_Text>().text
                =this.TimeCut.displayTime.ToString("F1");
            }

            GameObject player
            = GameObject.FindGameObjectWithTag("Player");
            playerController pc
            =player.GetComponent<playerController>();

            //スコア更新
            if(pc.score!=0)
            {
                this.stageScore
                +=pc.score;
                pc.score=0;
                UpdateScore();
            }
       }
    }

    void UpdateScore()
    {
        int score = this.stageScore + totalScore;
        this.scoreText.GetComponent<TMP_Text>().text
        = score.ToString();
    }

    ///<summary>
    /// メイン画像を非表示にします
    /// </summary>
    void SetActiveMainImage()
    {
        //パネルを非表示に設定
        this.mainImage.SetActive(false);
    }
}
