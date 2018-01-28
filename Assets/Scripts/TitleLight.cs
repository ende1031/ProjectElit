using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLight : MonoBehaviour {

    public float movingLightSpeed;//구슬이 움직이는 속도
    public float changeOpercityValue;//구슬의 투명도가 바뀌는 속도

    int i = 0;
    int a = 0;

    // Update is called once per frame
    void Update () {
        this.transform.Translate(0, movingLightSpeed, 0);
        this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, 
            this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, this.GetComponent<SpriteRenderer>().color.a + changeOpercityValue);
        i++;
        if (i > 100) { i = 0; movingLightSpeed *= -1; changeOpercityValue *= -1; }

    }
}
