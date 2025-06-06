using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletController : MonoBehaviour
{
    //発射から削除されるまでの時間
    public float deleteTime = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //破棄するタイミングを設定しておく
        Destroy(gameObject, this.deleteTime); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
