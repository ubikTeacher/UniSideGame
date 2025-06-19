using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{

    public float MaxLength = 70;//タブが動く最大距離
    public bool is4DPad = false; //4方向パッドかどうか
    GameObject player; //プレイヤーオブジェクト
    Vector2 defPos;//タブの初期位置
    Vector2 downPos;//タッチ位置

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイヤーキャラクターを取得
        player = GameObject.FindGameObjectWithTag("Player");

        //タブの初期座標
        defPos = GetComponent<RectTransform>().localPosition;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Downイベント
    public void PadDown()
    {
        //タッチ位置を取得
        downPos = Input.mousePosition;
    }

    //ドラッグイベント
    public void PadDrag()
    {
        //タッチ位置を取得
        Vector2 mousePos = Input.mousePosition;
        //タブの移動距離を計算
        Vector2 newTabPos = mousePos - downPos;

        if (is4DPad == false)
        {
            newTabPos.y = 0;//横スクロールの場合はy軸0
        }

        //移動ベクトルを計算
        Vector2 axis = newTabPos.normalized;

        //２点の距離を求める
        float len = Vector2.Distance(defPos, newTabPos);

        if (len > MaxLength)
        {
            //限界を超えた場合は最大距離に設定
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }

        //タブの位置を更新
        GetComponent<RectTransform>().localPosition = newTabPos;

        //プレイヤーの移動処理
        player.GetComponent<PlayerController>().SetAxis(axis.x, axis.y);
    }

    public void PadUp()
    {
        //タブの位置を初期位置に戻す
        GetComponent<RectTransform>().localPosition = defPos;
        //プレイヤーの移動処理
        player.GetComponent<PlayerController>().SetAxis(0, 0);
    }
}
