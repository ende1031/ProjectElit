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

    // Use this for initialization
    void Start()
    {
        ObjectManager.instance.FieldElementNum = 0;
        InvokeRepeating("SpawnElement", 3, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnElement()
    {
        if (ObjectManager.instance.FieldElementNum > 50) return;
        int tryNum = 0;
        Vector2 randomPos = new Vector3(Random.Range(-14, 15), Random.Range(-8, 9));
        int randomElement = (int)Random.Range(0, 5);
        while (ObjectManager.instance.isPlace(randomPos, "tree") || ObjectManager.instance.isPlace(randomPos, "monster") || ObjectManager.instance.isPlace(randomPos, "tail") 
            || ObjectManager.instance.isPlace(randomPos, "null_ball")
            || ObjectManager.instance.isPlace(randomPos, "fire_ball") || ObjectManager.instance.isPlace(randomPos, "water_ball")
            || ObjectManager.instance.isPlace(randomPos, "sand_ball") || ObjectManager.instance.isPlace(randomPos, "wind_ball"))
        {
            if (tryNum > 10) return;
            tryNum++;
            randomPos.Set(Random.Range(-14, 15), Random.Range(-8, 9));
        }

        tryNum = 0;
        ObjectManager.instance.FieldElementNum++;

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
}
