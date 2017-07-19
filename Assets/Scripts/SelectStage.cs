using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) // PC : ESC버튼, 안드로이드 : Back버튼
        {
            GameManager.instance.FadeAndLoadScene("Title");
        }
    }

    public void goStage(int s)
    {
        switch (s)
        {
            case 1:
                GameManager.instance.FadeAndLoadScene("Stage1-1");
                break;
            case 2:
                GameManager.instance.FadeAndLoadScene("Stage1-1");
                break;
            case 3:
                GameManager.instance.FadeAndLoadScene("Stage1-1");
                break;
        }
    }
}
