using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    public GameObject[] Spr = new GameObject[4];

    float num = 0;
    bool isFade = false;

	// Use this for initialization
	void Start () {
        Invoke("StartFade", 2f);
        Invoke("EndLogo", 5f);
	}

    void Update()
    {
        if (num < 255 && isFade) num += 0.2f;
        Color colorA = new Vector4(255, 255, 255, num);
        Spr[0].GetComponent<SpriteRenderer>().color = colorA;
        Spr[1].GetComponent<SpriteRenderer>().color = colorA;
    }

    void StartFade()
    {
        Spr[2].GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
        Spr[3].GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
        Debug.Log("start");
        isFade = true;
    }

    void EndLogo()
    {
        GameManager.instance.FadeAndLoadScene("Title");
    }
}
