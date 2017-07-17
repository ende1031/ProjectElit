using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject PauseCanvas;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GamePause()
    {
        Canvas.SetActive(false);
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameReplay()
    {
        Canvas.SetActive(true);
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackToTitle()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.FadeAndLoadScene("Title");
    }
}
