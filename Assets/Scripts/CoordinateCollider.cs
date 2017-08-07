using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateCollider : MonoBehaviour {

    public Vector2 Size;

    Vector2 Coordinate;
    float GridSize;

    // Use this for initialization
    void Start ()
    {
        if (GameObject.Find("Player") == null) return;
        GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Size.x % 2 == 1) //홀수
        {
            Coordinate = SetCoordinate();
            Coordinate.x -= (int)(Size.x / 2);
        }
        else //짝수
        {
            Coordinate = SetCoordinate();
            if (transform.position.x < 0)
            {
                Coordinate.x -= (int)(Size.x / 2 - 1);
            }
            if (transform.position.x > 0)
            {
                Coordinate.x -= (int)(Size.x / 2);
            }
        }
    }

    Vector2 SetCoordinate()
    {
        Vector2 c;
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

    public Vector2 GetCoordinate()
    {
        return Coordinate;
    }
}
