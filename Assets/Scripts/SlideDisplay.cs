using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDisplay : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.8F;//카메라 이동에 소요되는 시간
    Vector3 velocity = Vector3.zero;//가속도 (자동으로 바뀜)
    Vector3 targetPosition = (new Vector3(0, 1, -10));
    public int chapter_pos = 1;//보여줄 챕터
    float startpos, endpos;//터치 시작, 끝위치 저장

    public GameObject[] Stage = new GameObject[3];
    
    // Update is called once per frame
    void Update()
    {
        //터치 시작점 끝점 저장
        if (Input.GetMouseButtonDown(0))
        {
            startpos = Input.mousePosition.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endpos = Input.mousePosition.y;
            CameraController();
        }
        //카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void CameraController()
    {
        // 아래로 슬라이드 했을떄
        if (startpos - endpos > 100)
        {
            if (chapter_pos == 1)
            {
                chapter_pos = 2;
                targetPosition = (new Vector3(0, 27, -10));
            }
            else
            {
                chapter_pos = 3;
                targetPosition = (new Vector3(0, 56, -10));
            }

            GameObject.Find("Canvas").GetComponent<LoadButton>().FadeButton(chapter_pos, 2);
            startpos = endpos = 0;
        }

        //반대
        else if (endpos - startpos > 100)
        {
            if (chapter_pos == 3)
            {
                chapter_pos = 2;
                targetPosition = (new Vector3(0, 27, -10));
            }
            else
            {
                chapter_pos = 1;
                targetPosition = (new Vector3(0, 1, -10));
            }

            GameObject.Find("Canvas").GetComponent<LoadButton>().FadeButton(chapter_pos, 2);
            startpos = endpos = 0;
        }
    }
}