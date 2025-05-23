using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    public float leftLimit = 0.0f; //左の限界
    public float rightLimit = 0.0f; //右の限界
    public float topLimit = 0.0f; //上の限界
    public float bottomLimit = 0.0f; //下の限界

    public GameObject subScreen; //サブスクリーン用

    //強制スクロールをさせるかフラグ 
    public bool isForceScrollX=false;

    //1秒間で動かすx座標の量
    public float scrollSpeedX=0.5f;

    //強制スクロールをさせるかフラグ 
    public bool isForceScrollY=false;

    //1秒間で動かすx座標の量
    public float scrollSpeedY=0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.gameState != "playing")
       {
        return;
       }
        //ゲーム上のプレイヤーオブジェクトを取得
        GameObject player=GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            float x= player.transform.position.x;
            float y= player.transform.position.y;
            float z= this.transform.position.z;

            if(this.isForceScrollX)
            {
                //横に強制スクロール
                x=this.transform.position.x
                +(this.scrollSpeedX * Time.deltaTime);
            }
            if(this.isForceScrollY)
            {
                //縦に強制スクロール
                y=this.transform.position.y
                +(this.scrollSpeedY * Time.deltaTime);
            }
            if(x > rightLimit)
            {
                x=rightLimit;
            }
            else if(x < leftLimit)
            {
                x=leftLimit;
            }
            if(y > topLimit)
            {
                y=topLimit;
            }
            else if (y < bottomLimit)
            {
                y=bottomLimit;
            }

            //カメラの座標はプレイヤーの座標をコピー
            this.transform.position=new Vector3(x, y, z);

            //サブクリーンもスクロールしておく
            if(subScreen != null)
            {
                y=subScreen.transform.position.y;
                z=subScreen.transform.position.z;
                subScreen.transform.position
                =new Vector3(x / 2.0f,y,z);
            }
        }
        
    }
}
