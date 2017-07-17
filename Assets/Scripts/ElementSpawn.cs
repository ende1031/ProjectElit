using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawn : MonoBehaviour
{

    public GameObject FireBall;
    public GameObject WaterBall;
    public GameObject WindBall;
    public GameObject EarthBall;
    public GameObject NullBall;

    float GridSize;

    // Use this for initialization
    void Start()
    {
        ObjectManager.instance.FieldElementNum = 0;
        InvokeRepeating("SpawnElement", 3, 1);
        GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnElement()
    {
        if (ObjectManager.instance.FieldElementNum > 20) return;

        Vector2 randomPos = new Vector2(Random.Range(-14, 15), Random.Range(-8, 9));
        int randomElement = (int)Random.Range(0, 5);
        while (ObjectManager.instance.isPlace(Coordinate(randomPos), "tree"))
        {
            randomPos.Set(Random.Range(-14, 15), Random.Range(-8, 9));
        }

        ObjectManager.instance.FieldElementNum++;
        //Debug.Log(ObjectManager.instance.FieldElementNum);

        switch (randomElement)
        {
            case 0: //불속성
                Instantiate(FireBall, randomPos, Quaternion.identity);
                return;
            case 1: //물속성
                Instantiate(WaterBall, randomPos, Quaternion.identity);
                return;
            case 2: //바람속성
                Instantiate(WindBall, randomPos, Quaternion.identity);
                return;
            case 3: //땅속성
                Instantiate(EarthBall, randomPos, Quaternion.identity);
                return;
            case 4: //방해속성
                Instantiate(NullBall, randomPos, Quaternion.identity);
                return;
        }
    }

    //GridSize단위의 좌표로 바꿔주는 함수
    Vector2 Coordinate(Vector2 c)
    {
        c.x = transform.position.x;
        c.y = transform.position.y;
        if (c.x >= 0)
            c.x = (int)(c.x / GridSize + 0.5);
        else
            c.x = (int)(c.x / GridSize - 0.5);
        if (c.y >= 0)
            c.y = (int)(c.y / GridSize + 0.5);
        else
            c.y = (int)(c.y / GridSize - 0.5);
        return c;
    }
}
