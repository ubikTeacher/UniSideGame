using UnityEngine;
//↓シーンの切り替えに必要↓
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //読み込むシーン名
    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ///<summary>
    /// シーン読み込む用
    ///</summary>
    public void Load()
    {
        SceneManager.LoadScene(this.sceneName);
    }

}
