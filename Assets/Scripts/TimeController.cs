using UnityEngine;

public class TimeController : MonoBehaviour
{
    //カウントダウンをするかどうか
    public bool isCountDown = true;

    //ゲームの最大時間
    public float gameTime = 30.0f;

    //現在の経過時間
    public float displayTime = 0.0f;

    //タイムオーバーしたかどうか
    public bool isTimeOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.isCountDown==true)
        {
            //カウントダウン設定がTrueの場合は
            //表示時間を最大時間に設定する
            this.displayTime = this.gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //時間切れならもうカウントしない
        if(this.isTimeOver==true)
        {
            return;
        }

        if(this.isCountDown==true)
        {
            this.displayTime =
                this.displayTime-Time.deltaTime;
            
            //表示時間が0になったら時間切れ
            if(this.displayTime<0.0f)
            {
                this.isTimeOver = true;
            }
            //Debug.Log("displayTime:" + this.displayTime);
        }
    }
}
