using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject defaultSelectedButton;

    public GameObject clear_H;
    public GameObject clear_I;
    public GameObject clear_S;
    public GameObject clear_N;
    public GameObject button_H;
    public GameObject button_I;
    public GameObject button_S;
    public GameObject button_N;
    public GameObject clear_NK;
    public GameObject clear_NI;
    public GameObject clear_SK;
    public GameObject button_NK;
    public GameObject button_NI;
    public GameObject button_SK;
    public static bool isClear_H = false;
    public static bool isClear_I = false;
    public static bool isClear_S = false;
    public static bool isClear_N = false;
    public static bool isClear_NK = false;
    public static bool isClear_NI = false;
    public static bool isClear_SK = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        // ���̃t���[���őI���i���ꂪ�R�c�j
        /* 下記のプログラムを残りの３人分(SK,NKI,NKK)追加 */
        StartCoroutine(SetDefaultButtonNextFrame());

        if (isClear_H)
        {
            clear_H.SetActive(true);
            button_H.GetComponent<Button>().interactable = false;
        }
        else
        {
            clear_H.SetActive(false);
        }
        if (isClear_I)
        {
            clear_I.SetActive(true);
            button_I.GetComponent<Button>().interactable = false;
        }
        else
        {
            clear_I.SetActive(false);
        }
        if (isClear_S)
        {
            clear_S.SetActive(true);
            button_S.GetComponent<Button>().interactable = false;
        }
        else
        {
            clear_S.SetActive(false);
        }
        if (isClear_N)
        {
            clear_N.SetActive(true);
            button_N.GetComponent<Button>().interactable = false;
        }
        else
        {
            clear_N.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private System.Collections.IEnumerator SetDefaultButtonNextFrame()
    {
        yield return null; // 1�t���[���҂�
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }
}
