using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public static ObjectManager instance = null;

    //public class ObjectSet
    //{
    //    public ObjectSet(bool t, Vector2 c, Vector2 s)
    //    {
    //        isTriger = t;
    //        Coord = c;
    //        Size = s;
    //    }
    //    public bool isTriger;
    //    public Vector2 Coord; //왼쪽 아래를 기준으로
    //    public Vector2 Size; //왼쪽 아래를 기준으로 몇*몇칸인지
    //}

    //List<ObjectSet> Objectlist = new List<ObjectSet>(); //충돌가능한 모든 오브젝트를 가지고 있는 리스트

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
        FieldElementNum = 0;
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
