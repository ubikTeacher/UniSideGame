using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;//TextMeshPro用

public class ResultManager : MonoBehaviour
{
    //スコアテキスト格納用
    public GameObject scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //合計スコアをスコアボードに表示する
        scoreText.GetComponent<TMP_Text>().text
                = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
