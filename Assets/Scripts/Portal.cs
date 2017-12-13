using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{

    public string SceneName;
    public GameObject SuccessCanvas;
    public GameObject PlayerRef;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SuccessCanvas.SetActive(true);
            PlayerRef.gameObject.SetActive(false);
        }

    }

    public void NextStageButton()
    {
        GameManager.instance.FadeAndLoadScene(SceneName);
    }
}
