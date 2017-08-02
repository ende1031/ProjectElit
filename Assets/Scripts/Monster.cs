using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    private Animator animaitor;
    public int StartDirection; //어느 방향으로
    public int StepNumber; //몇발자국
    public float MoveSpeed; //얼마나 빨리
    public int MaxHP;

    int HP;
    Vector2 Size;
    float GridSize;
    Vector3 MoveVec;
    bool turnBack = false;
    int MoveCount = 0;
    Vector2 oldCoord;
    float idleTimer = 0;

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
        oldCoord = SetCoordinate();
        animaitor = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        if (HP <= 0)
            Die();
    }

    void Move()
    {
        animaitor.SetBool("Run",true);

        if(!turnBack)
        {

         animaitor.SetInteger("Direction", StartDirection);

            if (StartDirection == 0)
                MoveVec = new Vector3(0, 1, 0);
            else if (StartDirection == 1)
                MoveVec = new Vector3(0, -1, 0);
            else if (StartDirection == 2)
                MoveVec = new Vector3(-1, 0, 0);
            else if (StartDirection == 3)
                MoveVec = new Vector3(1, 0, 0);
        }
        else //되돌아올땐 MoveVec가 반대
        {
            if (StartDirection == 0)
                MoveVec = new Vector3(0, -1, 0);
            else if (StartDirection == 1)
                MoveVec = new Vector3(0, 1, 0);
            else if (StartDirection == 2)
            {

                animaitor.SetInteger("Direction", StartDirection + 1);
                MoveVec = new Vector3(1, 0, 0);
            }
            else if (StartDirection == 3)
            {
                animaitor.SetInteger("Direction", StartDirection - 1);
                MoveVec = new Vector3(-1, 0, 0);
            }
        }

        if (idleTimer <= 0) //움직임
        {
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        }
        else //대기
        {
            idleTimer -= Time.deltaTime;
        }

        //한칸 이동하면 MoveCount증가
        if (Mathf.Abs(oldCoord.x - transform.position.x) >= GridSize || Mathf.Abs(oldCoord.y - transform.position.y) >= GridSize)
        {
            MoveCount++;
            oldCoord = SetCoordinate();
        }

        //StepNumber만큼 이동하면 방향전환
        if (MoveCount >= StepNumber)
        {
            MoveCount = 0;
            if(turnBack)
                turnBack = false;
            else
                turnBack = true;
            idleTimer = 1.0f;
        }
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
        // 4가 나오면 노드랍
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
