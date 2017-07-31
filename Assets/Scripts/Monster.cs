using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public int MaxHP;
    int HP;

    Vector2 Size;
    float GridSize;

    public GameObject FireBall;
    public GameObject WaterBall;
    public GameObject WindBall;
    public GameObject SandBall;

    // Use this for initialization
    void Start ()
    {
        HP = MaxHP;
        Size = GetComponent<CoordinateCollider>().Size;
        GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (HP <= 0)
            Die();
	}

    public void Hit(int damage)
    {
        HP -= damage;
    }

    void Die()
    {
        DropItem();
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        Vector3 temp = new Vector3(SetCoordinate().x * GridSize, SetCoordinate().y * GridSize, 0);

        int randomElement = (int)Random.Range(0, 5);

        if (randomElement == 0)
            Instantiate(FireBall, temp, Quaternion.identity);
        else if (randomElement == 1)
            Instantiate(WaterBall, temp, Quaternion.identity);
        else if (randomElement == 2)
            Instantiate(WindBall, temp, Quaternion.identity);
        else if (randomElement == 3)
            Instantiate(SandBall, temp, Quaternion.identity);
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
}
