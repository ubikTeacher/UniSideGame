using UnityEngine;

public class WarpContoroller : MonoBehaviour
{
    public float WarpX = -6;
    public float WarpY = -4.5f;
    public GameObject yazirusi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (yazirusi != null)
        {
            yazirusi.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void yazirusi_hyouzi()
    {
        if (yazirusi != null)
        {
            yazirusi.SetActive(true);
        }
    }
}
