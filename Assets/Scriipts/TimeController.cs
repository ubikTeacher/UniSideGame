using UnityEngine;

public class TimeController : MonoBehaviour
{
    //カウントダウンをするかどうか
    public bool isCountDown = true;

    //ゲームの最大時間
    public float gameTime = 60.0f;

    //現在の経過時間
    public float displayTime = 0.0f;

    //タイムオーバーしたかどうか
    public bool isTimeOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(this.isCountDown==true)
        {
            //カウントダウン設定がtureの場合は
            //表示時間を最大時間に設定
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
            this.displayTime=
            this.displayTime-Time.deltaTime;
            if(this.displayTime<0.0f)
            {
                this.isTimeOver=true;
            }
            Debug.Log("displayTime:"+this.displayTime);
        }
    }
}
