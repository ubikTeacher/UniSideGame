using UnityEngine;
using UnityEngine.UI; //UI�̕��i�g�p���Ă���̂œ���Ă���
using System.Collections;
using System.Collections.Generic;
using TMPro;//TextMeshPro�p

public class GameManager : MonoBehaviour
{
    //�T�E���h�ݒ�ǉ�
    public AudioClip acGameOver;//�Q�[���I�[�o�[
    public AudioClip acGameClear;//�Q�[���N���A

    //�X�R�A�ǉ�
    public GameObject scoreText;  //�X�R�A�e�L�X�g�i�[�p
    public static int totalScore; //���v�X�R�A
    public int stageScore = 0;    //�X�e�[�W�X�R�A

    //�Q�[���I�[�o�[�p�摜�ݒ�p
    public Sprite gameOverSpr;
    //�Q�[���N���A�p�摜�ݒ�p
    public Sprite gameClearSpr;
    //�p�l���i�[�p
    public GameObject panel;
    //���X�^�[�g�{�^���i�[�p
    public GameObject restartBtn;
    //�l�N�X�g�{�^���i�[�p
    public GameObject nextBtn;
    //�摜�����Q�[���I�u�W�F�N�g�i�[�p
    public GameObject mainImage;
    
    //�^�C���o�[�i�[�p
    public GameObject timeBar;
    //�c�莞�ԕ\���e�L�X�g�i�[�p
    public GameObject timeText;
    
    //�^�C���R���g���[���[�X�N���v�g�i�[�p
    private TimeController timeCnt;

    //�v���C���[����
    public GameObject InputUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GAMESTART���\���ɐݒ�
        Invoke("SetActiveMainImage", 1.0f);
        //�p�l�����\���ɐݒ�
        this.panel.SetActive(false);

        //�^�C���R���g���[���擾
        this.timeCnt 
            = GetComponent<TimeController>();  

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerController.gameState);
        if(PlayerController.gameState=="gameclear")
        {

            InputUI.SetActive(false);
            //�Q�[���N���A�ɂȂ�����
            //�Q�[���N���A�̉摜��\������
            mainImage.SetActive(true);

            //�{�^���������Ă�p�l�����\��
            panel.SetActive(true);

            //���X�^�[�g�{�^���͉����Ȃ��悤�ɂ���
            Button rbtn = restartBtn.GetComponent<Button>();
            rbtn.interactable = false;

            //���C���C���[�W�̉摜��GameClear�̉摜�ɐ؂�ւ���
            Image mimg = mainImage.GetComponent<Image>();
            mimg.sprite = this.gameClearSpr;

            //�X�e�[�^�X���Q�[���I���ɂ���
            PlayerController.gameState = "gameend";

            //�������ԃJ�E���g���~�߂�
            if (this.timeCnt != null)
            {
                this.timeCnt.isTimeOver = true;
            }

            //�N���A���Đ�
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //�������Ă���BGM���~�߂�
                soundPlayer.Stop();
                //�Q�[���N���A�̉���炷
                soundPlayer.PlayOneShot(this.acGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover")
        {
            InputUI.SetActive(false);

            //�Q�[���I�[�o�[�ɂȂ�����
            //�Q�[���I�[�o�[�̉摜��\������
            mainImage.SetActive(true);

            //�{�^���������Ă�p�l�����\��
            panel.SetActive(true);

            // �l�N�X�g�{�^���͉����Ȃ��悤�ɂ���
            Button nbtn = nextBtn.GetComponent<Button>();
            nbtn.interactable = false;

            //���C���C���[�W�̉摜��GameOver�̉摜�ɐ؂�ւ���
            Image mimg = mainImage.GetComponent<Image>();
            mimg.sprite = this.gameOverSpr;

            //�X�e�[�^�X���Q�[���I���ɂ���
            PlayerController.gameState = "gameend";

            //�������ԃJ�E���g���~�߂�
            if (this.timeCnt != null)
            {
                this.timeCnt.isTimeOver = true;
            }

            //�Q�[���I�[�o�[���Đ�
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                //�������Ă���BGM���~�߂�
                soundPlayer.Stop();
                //�Q�[���N���A�̉���炷
                soundPlayer.PlayOneShot(this.acGameOver);
            }
        }
        else if (PlayerController.gameState == "playing")
        {
            if (this.timeCnt != null)
            {
                //�^�C���e�L�X�g���X�V
                this.timeText.GetComponent<TMP_Text>().text
                    = this.timeCnt.displayTime.ToString("F1");
            }

            GameObject player
                = GameObject.FindGameObjectWithTag("Player");
            PlayerController pc
                = player.GetComponent<PlayerController>();
            //�X�R�A�X�V
            if (pc.score != 0)
            {
                this.stageScore
                    += pc.score;
                pc.score = 0;
                UpdateScore();
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
    /// �X�R�A���X�V����
    /// </summary>
    void UpdateScore()
    {
        int score = this.stageScore + totalScore;
        this.scoreText.GetComponent<TMP_Text>().text
                            = score.ToString();    
    }


    /// <summary>
    /// ���C���摜���\���ɂ��܂�
    /// </summary>
    void SetActiveMainImage()
    {
        this.mainImage.SetActive(false);
    }
}
