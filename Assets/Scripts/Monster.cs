using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public int MonsterType; //0:위습, 1:버섯, 2:풀도치, 3:기사, 4:흑마법사, 5:거대골렘
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

    bool isAttack = false;
    int AttackDir;

    // Use this for initialization
    void Start ()
    {
        HP = MaxHP;
        Size = GetComponent<CoordinateCollider>().Size;
        GridSize = GameObject.Find("Player").GetComponent<PlayerController>().GridSize;
        oldCoord = SetCoordinate();

        if(this.GetComponent<Animator>() != null)
            animaitor = this.GetComponent<Animator>();
        else //일부 몬스터(위습)은 자식오브젝트("render")에 애니메이터가 있으므로
            animaitor = transform.Find("render").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (MonsterType != 2 && isAttack == false) //풀도치는 안움직임
            Move();

        if (MonsterType == 0) //일단 위습만 공격하게 해놓음
            WispAttack();

        if (HP <= 0)
            Die();
    }

    //위습 공격. 수정해서 다른 몬스터도 사용 가능하게 만들 예정
    void WispAttack()
    {
        Vector2 temp_coord = SetCoordinate();

        if (ObjectManager.instance.isPlace(new Vector2(temp_coord.x, temp_coord.y + 1), "Player")) //위쪽
        {
            isAttack = true;
            AttackDir = 0;
            animaitor.SetBool("isAttack", isAttack);
            animaitor.SetInteger("AttackDir", 0);
        }
        else if (ObjectManager.instance.isPlace(new Vector2(temp_coord.x, temp_coord.y - 1), "Player")) //아래
        {
            isAttack = true;
            AttackDir = 1;
            animaitor.SetBool("isAttack", isAttack);
            animaitor.SetInteger("AttackDir", 1);
        }
        else if (ObjectManager.instance.isPlace(new Vector2(temp_coord.x - 1, temp_coord.y), "Player")) //왼쪽
        {
            isAttack = true;
            AttackDir = 2;
            animaitor.SetBool("isAttack", isAttack);
            animaitor.SetInteger("AttackDir", 2);
        }
        else if (ObjectManager.instance.isPlace(new Vector2(temp_coord.x + 1, temp_coord.y), "Player")) //오른쪽
        {
            isAttack = true;
            AttackDir = 3;
            animaitor.SetBool("isAttack", isAttack);
            animaitor.SetInteger("AttackDir", 3);
        }
    }

    //공격을 마치고 다시 이동 시작
    public void AttackEnd()
    {
        isAttack = false;
        animaitor.SetBool("isAttack", isAttack);
    }

    //공격중 피격판정이 생기는 타이밍에 실행되는 함수
    public void HitPlayer()
    {
        Vector2 temp_coord = SetCoordinate();
        bool attackSuccess = false;

        if (AttackDir == 0 && ObjectManager.instance.isPlace(new Vector2(temp_coord.x, temp_coord.y + 1), "Player"))
        {
            attackSuccess = true;
        }
        else if (AttackDir == 1 && ObjectManager.instance.isPlace(new Vector2(temp_coord.x, temp_coord.y - 1), "Player"))
        {
            attackSuccess = true;
        }
        else if (AttackDir == 2 && ObjectManager.instance.isPlace(new Vector2(temp_coord.x - 1, temp_coord.y), "Player"))
        {
            attackSuccess = true;
        }
        else if (AttackDir == 3 && ObjectManager.instance.isPlace(new Vector2(temp_coord.x + 1, temp_coord.y), "Player"))
        {
            attackSuccess = true;
        }

        if (attackSuccess)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().Hit_attack();
        }
    }

    //일단 일정 범위를 반복해서 움직이는 함수
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
