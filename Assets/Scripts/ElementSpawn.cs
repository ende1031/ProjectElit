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
        if (ObjectManager.instance.FieldElementNum > 5) return;
        int tryNum = 0;
        Vector2 randomPos = new Vector3(Random.Range(-14, 15), Random.Range(-8, 9));
        int randomElement = (int)Random.Range(0, 5);
        int randomNullElement = (int)Random.Range(0, 5);
        while (ObjectManager.instance.isPlace(randomPos, "tree") || ObjectManager.instance.isPlace(randomPos, "monster")
            || ObjectManager.instance.isPlace(randomPos, "tail") || ObjectManager.instance.isPlace(randomPos, "null_ball")
            || ObjectManager.instance.isPlace(randomPos, "fire_ball") || ObjectManager.instance.isPlace(randomPos, "water_ball")
            || ObjectManager.instance.isPlace(randomPos, "sand_ball") || ObjectManager.instance.isPlace(randomPos, "wind_ball"))
        {
            if (tryNum > 10) return;
            tryNum++;
            randomPos.Set(Random.Range(-14, 15), Random.Range(-8, 9));
        }

        tryNum = 0;
        ObjectManager.instance.FieldElementNum++;

        if (randomElement == 0)
            Instantiate(FireBall, randomPos, Quaternion.identity);
        else if (randomElement == 1)
            Instantiate(WaterBall, randomPos, Quaternion.identity);
        else if (randomElement == 2)
            Instantiate(WindBall, randomPos, Quaternion.identity);
        else if (randomElement == 3)
            Instantiate(SandBall, randomPos, Quaternion.identity);

        else if (randomElement == 4)
        {
            if (randomNullElement == 0)
                Instantiate(NullBall_fire, randomPos, Quaternion.identity);
            else if (randomNullElement == 1)
                Instantiate(NullBall_water, randomPos, Quaternion.identity);
            else if (randomNullElement == 2)
                Instantiate(NullBall_wind, randomPos, Quaternion.identity);
            else if (randomNullElement == 3)
                Instantiate(NullBall_sand, randomPos, Quaternion.identity);
        }
        return;
    }
}
