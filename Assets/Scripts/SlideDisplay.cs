using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDisplay : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.8F;//카메라 이동에 소요되는 시간
    Vector3 velocity = Vector3.zero;//가속도 (자동으로 바뀜)
    Vector3 targetPosition = (new Vector3(0, 1, -10));
    [HideInInspector] public int chapter_pos = 0;
    float startpos, endpos;

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
        //현재 카메라 위치 파악
        CurrentCamera();
        //카메라 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void CurrentCamera()
    {
        //0 = 챕터 이동중, 1~3은 챕터 1~3을 의미
        if (target.position.y < 20) chapter_pos = 1;

        else if (target.position.y > 33) chapter_pos = 3;

        else if (target.position.y > 25 && target.position.y < 28) chapter_pos = 2;
    }

    void CameraController()
    {
        // 아래로 슬라이드 했을떄
        if (startpos - endpos > 100)
        {
            if (chapter_pos == 1) targetPosition = (new Vector3(0, 27, -10));
            else targetPosition = (new Vector3(0, 56, -10));

            //모든 챕터의 버튼을 모두 페이드아웃, 인
            for (int i = 0; i < 2; i++)
            {
                Stage[i].GetComponent<LoadButton>().FadeButton(1.5f);
            }
            startpos = endpos = 0;
        }
        //반대
        else if (endpos - startpos > 100)
        {
            if (chapter_pos == 3) targetPosition = (new Vector3(0, 27, -10));
            else targetPosition = (new Vector3(0, 1, -10));

            for (int i = 0; i < 2; i++)
            {
                Stage[i].GetComponent<LoadButton>().FadeButton(1.5f);
            }
            startpos = endpos = 0;
        }
    }
}