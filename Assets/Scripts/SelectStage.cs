using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    int UnlockedStage;
    // Use this for initialization
    void Start()
    {
        UnlockedStage = GameObject.Find("GameManager").GetComponent<SaveManager>().OutputStage();
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
        if (s > UnlockedStage + 1) return;
        switch (s)
        {
            case 1:
                GameManager.instance.FadeAndLoadScene("Stage1-1");
                break;
            case 2:
                GameManager.instance.FadeAndLoadScene("Stage1-2");
                break;
            case 3:
                GameManager.instance.FadeAndLoadScene("Stage1-3");
                break;
            case 4:
                GameManager.instance.FadeAndLoadScene("Stage1-4");
                break;
            case 5:
                GameManager.instance.FadeAndLoadScene("Stage1-5");
                break;
            case 6:
                GameManager.instance.FadeAndLoadScene("Stage2-1");
                break;
            case 7:
                GameManager.instance.FadeAndLoadScene("Stage2-2");
                break;
            case 8:
                GameManager.instance.FadeAndLoadScene("Stage2-3");
                break;
            case 9:
                GameManager.instance.FadeAndLoadScene("Stage2-4");
                break;
            case 10:
                GameManager.instance.FadeAndLoadScene("Stage2-5");
                break;
            case 11:
                GameManager.instance.FadeAndLoadScene("Stage3-1");
                break;
            case 12:
                GameManager.instance.FadeAndLoadScene("Stage3-2");
                break;
            case 13:
                GameManager.instance.FadeAndLoadScene("Stage3-3");
                break;
            case 14:
                GameManager.instance.FadeAndLoadScene("Stage3-4");
                break;
            case 15:
                GameManager.instance.FadeAndLoadScene("Stage3-5");
                break;
        }
    }

    public void goChapter(int s)
    {
        if (s * 5 > UnlockedStage + 1) return;

    }

}
