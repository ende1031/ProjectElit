using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//꼬리 구슬
public class ElementDrop : MonoBehaviour {

    public int Element; //0:불, 1:물, 2:바람, 3:땅, 4:방해

    Vector2 Coordinate; //좌표
    float GridSize;

    float MoveSpeed;
    public int Direction; // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
    Vector3 MoveVec; //방향 벡터

    public int DropNumber; //이 꼬리구슬이 앞에서 몇 번째 구슬인지

    // Use this for initialization
    void Start () {
        GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
        MoveSpeed = GameObject.Find("Player").GetComponent<PlayerController>().MoveSpeed;
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
