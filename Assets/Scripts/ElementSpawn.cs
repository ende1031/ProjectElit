using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawn : MonoBehaviour
{

    public GameObject FireBall;
    public GameObject WaterBall;
    public GameObject WindBall;
    public GameObject SandBall;

    public GameObject NullBall_fire;
    public GameObject NullBall_water;
    public GameObject NullBall_wind;
    public GameObject NullBall_sand;

    public int elemntLimit;

    Vector3 randomPos;
    int randomElement;
    int randomNullElement;
    float top, bot, left, right;

    // Use this for initialization
    void Start()
    {
        ObjectManager.instance.FieldElementNum = 0;
        InvokeRepeating("RandomElementSpawn", 3, 1);//게임 시작 3초뒤에 1초 마다 반복

        randomElement = 0;
        randomNullElement = 0;
        top = GameObject.Find("topWall").transform.position.y;
        bot = GameObject.Find("botWall").transform.position.y;
        left = GameObject.Find("leftWall").transform.position.x;
        right = GameObject.Find("rightWall").transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void RandomEmptyPosition()
    {
        int tryNum = 0;//한번에 RandomEmptyPosition이 실행된 횟수.

        randomPos = new Vector3((int)Random.Range(left, right), (int)Random.Range(bot, top), 0);

        while (ObjectManager.instance.isPlace(randomPos, "tree") || ObjectManager.instance.isPlace(randomPos, "monster")
            || ObjectManager.instance.isPlace(randomPos, "tail") || ObjectManager.instance.isPlace(randomPos, "null_ball")
            || ObjectManager.instance.isPlace(randomPos, "fire_ball") || ObjectManager.instance.isPlace(randomPos, "water_ball")
            || ObjectManager.instance.isPlace(randomPos, "sand_ball") || ObjectManager.instance.isPlace(randomPos, "wind_ball"))
        {
            if (tryNum > 10) return;
            tryNum++;
            randomPos.Set((int)Random.Range(left, right), (int)Random.Range(bot, top), 0);
        }

        tryNum = 0;
    }

    void SpawnElement(int ElementType, int NullElementType, Vector3 Position)
    {
        //필드상 구슬개수 카운트
        ObjectManager.instance.FieldElementNum++;

        //기본 구슬 4가지
        if (ElementType == 0)
            Instantiate(FireBall, Position, Quaternion.identity);
        else if (ElementType == 1)
            Instantiate(WaterBall, Position, Quaternion.identity);
        else if (ElementType == 2)
            Instantiate(WindBall, Position, Quaternion.identity);
        else if (ElementType == 3)
            Instantiate(SandBall, Position, Quaternion.identity);
        //방해구슬 4가지
        else if (randomElement == 4)
        {
            if (NullElementType == 0)
                Instantiate(NullBall_fire, Position, Quaternion.identity);
            else if (NullElementType == 1)
                Instantiate(NullBall_water, Position, Quaternion.identity);
            else if (NullElementType == 2)
                Instantiate(NullBall_wind, Position, Quaternion.identity);
            else if (NullElementType == 3)
                Instantiate(NullBall_sand, Position, Quaternion.identity);
        }
        return;
    }

    void RandomElementSpawn()
    {

        //필드상의 구슬이 제한량 보다 많으면 실행 안함
        if (ObjectManager.instance.FieldElementNum > elemntLimit) return;
        //속성구슬 정하기
        randomElement = (int)Random.Range(0, 5);
        randomNullElement = (int)Random.Range(0, 5);
        //위치 정하기
        RandomEmptyPosition();
        //구슬 소환
        SpawnElement(randomElement, randomNullElement, randomPos);
    }
}
