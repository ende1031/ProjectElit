using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject PauseCanvas;

    bool isPause;

    // Use this for initialization
    void Start()
    {
        isPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // PC : ESC버튼, 안드로이드 : Back버튼
        {
            if (!isPause)
                GamePause();
            else
                GameReplay();
        }
    }

    public void GamePause()
    {
        Canvas.SetActive(false);
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
        isPause = true;
    }

    public void GameReplay()
    {
        Canvas.SetActive(true);
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }

    public void BackToTitle()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.FadeAndLoadScene("Title");
    }

    public void SelectStage()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.FadeAndLoadScene("SelectStage");
    }

    //스테이지 다시 시작
    public void RestartStage()
    {
        GameManager.instance.FadeAndLoadScene(EditorApplication.currentScene);
        //Application.LoadLevel(Application.loadedLevel);
    }
}
