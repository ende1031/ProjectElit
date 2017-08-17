using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//꼬리 구슬
public class ElementDrop : MonoBehaviour
{

    public int Element; //0:불, 1:물, 2:바람, 3:땅, 4:방해
    public int DropNumber; //이 꼬리구슬이 앞에서 몇 번째 구슬인지

    public int Direction; // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽

    Color BallColor;
    //Vector2 Coordinate; //좌표
    //float GridSize;

    //float MoveSpeed;
    //Vector3 MoveVec; //방향 벡터
    


    // Use this for initialization
    void Start()
    {
        //GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
        //MoveSpeed = GameObject.Find("Player").GetComponent<PlayerController>().MoveSpeed;

        BallColor = GetComponent<SpriteRenderer>().color;
        BallColor.a = 0;
        GetComponent<SpriteRenderer>().color = BallColor;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BallColor.a < 1.0f)
            BallColor.a += 2 * Time.deltaTime;
        if (BallColor.a >= 1.0f)
            BallColor.a = 1.0f;

        GetComponent<SpriteRenderer>().color = BallColor;
        
    }
}
