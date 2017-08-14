using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public static ObjectManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public int FieldElementNum;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //해당 좌표에 해당 태그의 오브젝트가 있는지
    public bool isPlace(Vector2 coord, string tag)
    {
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject it in tempObj)
        {
            for (int x = 0; x < it.GetComponent<CoordinateCollider>().Size.x; x++)
            {
                for (int y = 0; y < it.GetComponent<CoordinateCollider>().Size.y; y++)
                {
                    Vector2 temp = coord;
                    temp.x -= x;
                    temp.y -= y;
                    if (it.GetComponent<CoordinateCollider>().GetCoordinate() == temp)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public GameObject PlacedObject(Vector2 coord, string tag)
    {
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject it in tempObj)
        {
            for (int x = 0; x < it.GetComponent<CoordinateCollider>().Size.x; x++)
            {
                for (int y = 0; y < it.GetComponent<CoordinateCollider>().Size.y; y++)
                {
                    Vector2 temp = coord;
                    temp.x -= x;
                    temp.y -= y;
                    if (it.GetComponent<CoordinateCollider>().GetCoordinate() == temp)
                    {
                        return it;
                    }
                }
            }
        }

        return null;
    }
}
