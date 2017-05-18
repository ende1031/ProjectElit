using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDrop : MonoBehaviour {

    public int Element; //0:불, 1:물, 2:바람, 3:땅, 4:방해

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (Element) //각 속성별로 다르게 실행되는걸 넣어보자. (ex: 스프라이트 애니메이션)
        {
            case 0: //불속성
                break;
            case 1: //물속성
                break;
            case 2: //바람속성
                break;
            case 3: //땅속성
                break;
            case 4: //방해속성
                break;
        }
    }
}
