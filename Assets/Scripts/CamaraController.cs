﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public int ScreenSizeX;
    public int ScreenSizeY;

    public GameObject Player;
    public float CameraSpeed;

    public GameObject TopWall;
    public GameObject BotWall;
    public GameObject LeftWall;
    public GameObject RightWall;

    Vector3 TargetTransform; //이동해야 하는 위치
    Vector3 TempTransform; //카메라의 위치에서 Offset을 적용하기 전 값

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //실행중 화면 꺼짐 방지
        Screen.SetResolution(ScreenSizeX, ScreenSizeY, false);
    }

    // Use this for initialization
    void Start ()
    {
        TempTransform = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        //타겟 위치를 받아옴
        TargetTransform = Player.transform.position;

        //벽에 가까이 있으면 TargetTransform를 조정
        if (TargetTransform.x - LeftWall.transform.position.x < 3.5f)
            TargetTransform.x = LeftWall.transform.position.x + 3.5f;
        if (RightWall.transform.position.x - TargetTransform.x < 3.5f)
            TargetTransform.x = RightWall.transform.position.x - 3.5f;
        
        if (TopWall.transform.position.y - TargetTransform.y < 5.5f)
            TargetTransform.y = TopWall.transform.position.y - 5.5f;
        if (TargetTransform.y - BotWall.transform.position.y < 5.5f)
            TargetTransform.y = BotWall.transform.position.y + 5.5f;

        //타겟을 향해 움직임
        TempTransform.x += (TempTransform.x - TargetTransform.x) / -15 * CameraSpeed * Time.deltaTime;
        TempTransform.y += (TempTransform.y - TargetTransform.y) / -15 * CameraSpeed * Time.deltaTime;
        //위치 적용
        transform.position = TempTransform;
    }
}