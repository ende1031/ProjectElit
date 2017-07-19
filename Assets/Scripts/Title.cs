using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetMouseButtonDown(0)) //엔터 누르거나 화면 아무곳 클릭
        {
            GameStart();
        }

        if (Input.GetKey(KeyCode.Escape)) // PC : ESC버튼, 안드로이드 : Back버튼
        {
#if UNITY_ANDROID //안드로이드일 경우
            Application.Quit();
#endif
        }
    }

    public void GameStart() //나중에 다른데서 쓸수도 있을 것 같아서 따로 만들어둠.
    {
        GameManager.instance.FadeAndLoadScene("SelectStage");
    }
}