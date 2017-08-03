using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animaitor;

    // 공격 애니메이션 -> 기본 애니메이션으로 돌아오기 위한 변수
    bool attack_ing = false;
    float attack_delay = 0;

    bool hit_ing = false;
    float hit_delay = 0;

    bool immortal; //무적
    float immortal_timer;

    public class Coordset
    {
        public Coordset(Vector2 o, Vector2 n)
        {
            Old = o;
            New = n;
        }
        public Vector2 Old;
        public Vector2 New;
    }

    public float MoveSpeed; //플레이어 속도
    public float GridSize; //한 칸의 크기 (단위 : 100분의 1픽셀)
    float orgMoveSpeed; //플레이어의 속도를 임시로 저장 (원래 속도가 얼마였는지 알기 위해)

    public GameObject FireDrop;
    public GameObject WaterDrop;
    public GameObject WindDrop;
    public GameObject EarthDrop;
    public GameObject NullDrop;

    public GameObject Fireshot;
    public GameObject Watershot;
    public GameObject Windshot;
    public GameObject Sandshot;

    public GameObject NormalCanvas;
    public GameObject GameOverCanvas;

    Vector2 Coordinate; //좌표
    int Direction; // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽

    Vector3 MoveVec; //방향 벡터
    Vector2 DirCoord; //방향전환용 임시좌표
    bool setDirCoord;
    int inputDirection;//방향전환용
    bool canCangeDir; //현재 방향전환 가능한지

    Vector3 mousePos_start; //드래그 시작점
    Vector3 mousePos_end; //드래그 끝날때

    List<GameObject> Droplist = new List<GameObject>(); //꼬리 리스트
    List<Coordset> Coordlist = new List<Coordset>(); //지나온 좌표를 저장하는 리스트
    List<int> Dirlist = new List<int>(); //지나온 방향을 저장하는 리스트.

    Coordset PlayerCoodset; //꼬리를 움직이게 하기 위한 변수들
    Vector2 OldCoord;
    float percentOfCoord;

    bool isHit; //충돌중인지
    float hitTimer;
    int HitDirection; //맞았을 때 방향

    int NullElementNum;//맨 앞 방해구슬 개수만큼 무시

    bool GameOver;

    float chargeTimer;
    bool buttonDown;
    bool buttonUP;
    int[] Chargelist = new int[5];

    // Use this for initialization
    void Start()
    {
        Direction = 2;
        inputDirection = 2;
        SetCoordinate();
        setDirCoord = false;

        isHit = false;
        orgMoveSpeed = MoveSpeed;
        hitTimer = 0;

        canCangeDir = true;

        PlayerCoodset = new Coordset(Coordinate, Coordinate);
        OldCoord = PlayerCoodset.Old;
        animaitor = this.GetComponent<Animator>();
        animaitor.SetInteger("direction", Direction);

        NullElementNum = 0;

        GameOver = false;
        immortal = false;
        immortal_timer = 0;
        chargeTimer = 0;

        buttonDown = false;
        buttonUP = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameOver)
        {
            //일정시간 지나면 무적 풀림
            if (immortal)
            {
                immortal_timer -= Time.deltaTime;
                if (immortal_timer < 0)
                {
                    immortal = false;
                    immortal_timer = 0;
                }
            }

            //맞았을 때 매 프레임 실행
            if (isHit && !immortal) Hit();
            else hitTimer = 0;

            //충돌
            Collision_ball();
            Collision_mob();
            Collision_tail();

            //이동
            if (!isHit) Move();
            MoveTails();
            if (canCangeDir) InputKey();
            SetCoordinate();

            //공격
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.Space) || (buttonDown && buttonUP))
            {
                for (int i = 0; i < 3; i++)
                {
                    if (GetTailLength() - NullElementNum < i + 1) break; // 꼬리 길이가 3개보다 적으면 패스

                    //불 = 0, 물 = 1, 바람 = 2, 땅 = 3
                    if (GetElement(i) == 0)
                    {
                        if (Chargelist[1] > 0) break;
                        Chargelist[0] += 1;
                        Chargelist[4]++;
                    }

                    else if (GetElement(i) == 1)
                    {
                        if (Chargelist[0] > 0) break;
                        Chargelist[1] += 1;
                        Chargelist[4]++;
                    }

                    else if (GetElement(i) == 2)
                    {
                        Chargelist[2] += 1;
                        Chargelist[4]++;
                    }
                    else if (GetElement(i) == 3)
                    {
                        Chargelist[3] += 1;
                        Chargelist[4]++;
                    }
                    else if (GetElement(i) == 4)
                    {
                        break;
                    }
                }
                buttonUP = false;
            }

            if (Input.GetKey(KeyCode.Space) || buttonDown)
            {
                chargeTimer += Time.deltaTime;

                // 차지 이펙트 추가 관련 코드
                if (chargeTimer > 2 && ChargeEffect.instance.charge_stat == 2 && Chargelist[4] > 2)
                {
                    ChargeEffect.instance.ChangeE(ChargeEffect.instance.third_charge_image, GetElement(2));
                    ChargeEffect.instance.Chargethird();
                }

                else if (chargeTimer > 1 && ChargeEffect.instance.charge_stat == 1 && Chargelist[4] > 1)
                {
                    ChargeEffect.instance.ChangeE(ChargeEffect.instance.ssecond_charge_image, GetElement(1));
                    ChargeEffect.instance.ChangeE(ChargeEffect.instance.second_charge_image, GetElement(1));
                    ChargeEffect.instance.ChargeSSecond();
                }

                else if (ChargeEffect.instance.charge_stat == 0 && Chargelist[4] > 0)
                {
                    ChargeEffect.instance.ChangeE(ChargeEffect.instance.first_charge_image, GetElement(0));
                    ChargeEffect.instance.Chargefirst();
                }
            }

            //맨 앞의 방해구슬 개수 체크
            if (GetTailLength() > 0 && GetTailLength() != NullElementNum) //꼬리가 0개보다 많을 때,맨 앞 방해구슬보다 많을 때 만 검사
            {
                if (GetTailLength() < NullElementNum)
                {
                    NullElementNum--;
                }
                else if (Droplist[NullElementNum].GetComponent<ElementDrop>().Element == 4)
                {
                    NullElementNum++;
                }
            }
        }

        // 공격 애니메이션 -> 기본 애니메이션으로 돌아오기 위한 변수

        if (attack_ing)
        {
            attack_delay += Time.deltaTime;
        }

        if (attack_delay > 1.0f)
        {
            animaitor.SetInteger("Attack_element", 10);
            attack_ing = false;
            attack_delay = 0;
        }

        if (hit_ing)
        {
            hit_delay += Time.deltaTime;
        }

        if (hit_delay > 0.5f)
        {
            animaitor.SetBool("Hitted", false);
            hit_ing = false;
            hit_delay = 0;
        }
    }

    //맞았을 때 실행
    void Hit()
    {
        hitTimer += Time.deltaTime;
        if (hitTimer > 0.5f) // 이 시간 주기로 꼬리 하나씩 감소
        {
            animaitor.SetBool("Hitted", true);
            hit_ing = true;

            if (GetTailLength() == 0 || GetTailLength() == NullElementNum)
            {
                Die();
            }
            else if (GetTailLength() > 0)
            {
                RemoveDrop();
            }
            hitTimer = 0;
            ResetAttack(Chargelist[4]);
        }
    }

    // 아이템과 같은 좌표에 있을 때
    void Collision_ball()
    {
        if (ObjectManager.instance.isPlace(Coordinate, "fire_ball"))
        {
            InsertDrop(0);
            Destroy(ObjectManager.instance.PlacedObject(Coordinate, "fire_ball"));
            ObjectManager.instance.FieldElementNum--;
        }
        else if (ObjectManager.instance.isPlace(Coordinate, "water_ball"))
        {
            InsertDrop(1);
            Destroy(ObjectManager.instance.PlacedObject(Coordinate, "water_ball"));
            ObjectManager.instance.FieldElementNum--;
        }
        else if (ObjectManager.instance.isPlace(Coordinate, "wind_ball"))
        {
            InsertDrop(2);
            Destroy(ObjectManager.instance.PlacedObject(Coordinate, "wind_ball"));
            ObjectManager.instance.FieldElementNum--;
        }
        else if (ObjectManager.instance.isPlace(Coordinate, "sand_ball"))
        {
            InsertDrop(3);
            Destroy(ObjectManager.instance.PlacedObject(Coordinate, "sand_ball"));
            ObjectManager.instance.FieldElementNum--;
        }
        else if (ObjectManager.instance.isPlace(Coordinate, "null_ball"))
        {
            InsertDrop(4);
            Destroy(ObjectManager.instance.PlacedObject(Coordinate, "null_ball"));
            ObjectManager.instance.FieldElementNum--;
        }
    }

    //몬스터, 나무를 통과하지 못하게
    void Collision_mob()
    {
        Vector2 temp = Coordinate;
        //int tempDir = Direction;

        switch (Direction)
        {
            case 0: //위
                temp.y += 1;
                break;
            case 1: //아래
                temp.y -= 1;
                break;
            case 2: //왼
                temp.x -= 1;
                break;
            case 3: //오른
                temp.x += 1;
                break;
        }

        if (ObjectManager.instance.isPlace(temp, "monster") || ObjectManager.instance.isPlace(temp, "tree"))
        {
            isHit = false;
            MoveSpeed = orgMoveSpeed;
            if (transform.position.y / GridSize >= Coordinate.y - 0.1 && Direction == 0)
                Colliding();
            if (transform.position.y / GridSize <= Coordinate.y + 0.1 && Direction == 1)
                Colliding();
            if (transform.position.x / GridSize <= Coordinate.x + 0.1 && Direction == 2)
                Colliding();
            if (transform.position.x / GridSize >= Coordinate.x - 0.1 && Direction == 3)
                Colliding();
        }
        else
        {
            isHit = false;
            MoveSpeed = orgMoveSpeed;
        }
    }

    //꼬리 자르기
    void Collision_tail()
    {
        if (ObjectManager.instance.isPlace(Coordinate, "tail"))
        {
            int temp = ObjectManager.instance.PlacedObject(Coordinate, "tail").GetComponent<ElementDrop>().DropNumber;
            Debug.Log(temp + "번째 꼬리와 충돌");

            for (int i = 0; i < Droplist.Count; i++)
            {
                if (i >= temp)
                {
                    RemoveDropAt(temp);
                }
            }
        }
    }

    //몬스터, 나무에 충돌중일때 프레임마다 실행
    void Colliding()
    {
        isHit = true;
        if (Direction != inputDirection)
        {
            Vector2 temp = Coordinate;
            switch (inputDirection)
            {
                case 0: //위
                    temp.y += 1;
                    break;
                case 1: //아래
                    temp.y -= 1;
                    break;
                case 2: //왼
                    temp.x -= 1;
                    break;
                case 3: //오른
                    temp.x += 1;
                    break;
            }
            if (ObjectManager.instance.isPlace(temp, "monster") || ObjectManager.instance.isPlace(temp, "tree"))
            {
                return; //구석에 박혀서 방향전환하려는 방향에도 벽이 있을 경우 그쪽으론 방향전환 안됨
            }
            else
            {
                MoveSpeed = orgMoveSpeed;
                ChangeDir_hit();
            }
        }
    }

    //충돌시 방향전환
    void ChangeDir_hit()
    {
        if (!setDirCoord) //어디까지 이동 후 방향전환 하는지 설정
        {
            DirCoord = Coordinate;
        }
        //다음 그리드까지 이동 후 방향전환
        if (transform.position.y / GridSize < DirCoord.y && Direction == 0)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.y / GridSize > DirCoord.y && Direction == 1)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.x / GridSize > DirCoord.x && Direction == 2)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.x / GridSize < DirCoord.x && Direction == 3)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else
        {
            setDirCoord = false;
            transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
            Direction = inputDirection;
            animaitor.SetInteger("direction", Direction);
            canCangeDir = true;
        }
    }

    public void OnPointerDown()
    {
        buttonDown = true;
    }
    public void OnPointerUp()
    {
        buttonDown = false;
    }

    //공격. 버튼UI에서 이 함수 실행
    public void Attack()
    {

        // 불 바람 물 땅 마법 사용

        /*
        bool attack_poss = false;
        if (chargeTimer > 2)
        {
            if (Chargelist[0] == 3)
            {
            }
            else if (Chargelist[1] == 3)
            {
            }
            else if (Chargelist[2] == 3)
            {
            }
            else if (Chargelist[3] == 3)
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[2] == 2 || Chargelist[0] == 2 && Chargelist[2] == 1)
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[3] == 2 || Chargelist[0] == 2 && Chargelist[3] == 1)
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[2] == 2 || Chargelist[1] == 2 && Chargelist[2] == 1)
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[3] == 2 || Chargelist[1] == 2 && Chargelist[3] == 1)
            {
            }
            else if (Chargelist[2] == 1 && Chargelist[3] == 2 || Chargelist[2] == 2 && Chargelist[3] == 1)
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[2] == 1 && Chargelist[3] == 1 )
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[2] == 1 && Chargelist[3] == 1)
            {
            }

        }
        if (chargeTimer > 1)
        {
            if (Chargelist[0] == 2)//불+불
            {
                attack_poss = true;
            }
            else if (Chargelist[1] == 2)//물+물
            {
                attack_poss = true;
            }
            else if (Chargelist[2] == 2)//바람+바람
            {
                attack_poss = true;
            }
            else if (Chargelist[3] == 2)//흙+흙
            {
                attack_poss = true;
            }
            else if (Chargelist[0] == 1 && Chargelist[2] == 1)//불+바람
            {
                attack_poss = true;
            }
            else if (Chargelist[0] == 1 && Chargelist[3] == 1)//불+흙
            {
                attack_poss = true;
            }
            else if (Chargelist[1] == 1 && Chargelist[2] == 1)//물+바람
            {
                attack_poss = true;
            }
            else if (Chargelist[1] == 1 && Chargelist[3] == 1)//물+흙
            {
                attack_poss = true;
            }
            else if (Chargelist[2] == 1 && Chargelist[3] == 1)//바람+흙
            {
                attack_poss = true;
            }

            if (attack_poss)
            {
                RemoveDrop();
                RemoveDrop();
                chargeTimer = 0;
                for (int j = 0; j < 5; j++)
                    Chargelist[j] = 0;
                attack_poss = false;
                return;
            }
        }
        */
        if (GetElement(0) == 0)
        {
            attack_ing = true;
            animaitor.SetInteger("Attack_element", 0);
            Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Fire");
        }
        else if (GetElement(0) == 1)
        {
            attack_ing = true;
            animaitor.SetInteger("Attack_element", 1);
            Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Water");
        }
        else if (GetElement(0) == 2)
        {
            attack_ing = true;
            animaitor.SetInteger("Attack_element", 2);
            Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Wind");
        }
        else if (GetElement(0) == 3)
        {
            attack_ing = true;
            animaitor.SetInteger("Attack_element", 3);
            if (immortal_timer == 0)
                Instantiate(Sandshot, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            immortal_timer = 3;
            immortal = true;
            GameManager.instance.PlaySE("Sand");
        }
        ResetAttack(1);
    }

    void ResetAttack(int charge_num)
    {
        for (int i = 0; i < charge_num; i++) 
            RemoveDrop(); 
        chargeTimer = 0;
        for (int j = 0; j < 5; j++)
            Chargelist[j] = 0;
        ChargeEffect.instance.ChargeReset();
        buttonUP = true;
        buttonDown = false;
    }

    //나중에 터치&드래그로 바꾸기
    void InputKey()
    {
        if (Input.GetMouseButtonDown(0)) //드래그 시작
        {
            mousePos_start = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        }

        if (Input.GetMouseButtonUp(0)) //드래그 끝. 방향전환 적용
        {
            mousePos_end = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);

            if (Mathf.Abs(mousePos_start.x - mousePos_end.x) < Mathf.Abs(mousePos_start.y - mousePos_end.y)) // y축 이동
            {
                if (mousePos_start.y < mousePos_end.y) //아래쪽 이동
                {
                    if (Direction == 1 || Direction == 0)
                        return;
                    inputDirection = 0;
                }
                else if (mousePos_start.y > mousePos_end.y) //위쪽 이동
                {
                    if (Direction == 0 || Direction == 1)
                        return;
                    inputDirection = 1;
                }
                else //마우스를 움직인 값이 0일때
                {
                    return;
                }
            }
            else //x축 이동
            {
                if (mousePos_start.x > mousePos_end.x) //왼쪽 이동
                {
                    if (Direction == 3 || Direction == 2)
                        return;
                    inputDirection = 2;
                }
                else if (mousePos_start.x < mousePos_end.x) //오늘쪽 이동
                {
                    if (Direction == 2 || Direction == 3)
                        return;
                    inputDirection = 3;
                }
                else //마우스를 움직인 값이 0일때
                {
                    return;
                }
            }
        }

        //키보드로 방향전환(PC 테스트용)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Direction == 1 || Direction == 0)
                return;
            inputDirection = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Direction == 0 || Direction == 1)
                return;
            inputDirection = 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Direction == 3 || Direction == 2)
                return;
            inputDirection = 2;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Direction == 2 || Direction == 3)
                return;
            inputDirection = 3;
        }
    }

    //Direction 값 반환. 다른 클래스에서 사용할것같음
    public int getDirection()
    {
        return Direction;
    }

    //꼬리 움직이기
    void MoveTails()
    {
        //현재 이동 방향별 좌표세트 설정
        PlayerCoodset = new Coordset(Coordinate, Coordinate);
        switch (Direction)
        {
            case 0:
                if (transform.position.y / GridSize >= Coordinate.y)
                    PlayerCoodset.New.y++;
                else
                    PlayerCoodset.Old.y--;
                percentOfCoord = (PlayerCoodset.New.y - transform.position.y / GridSize);
                break;
            case 1:
                if (transform.position.y / GridSize <= Coordinate.y)
                    PlayerCoodset.New.y--;
                else
                    PlayerCoodset.Old.y++;
                percentOfCoord = (transform.position.y / GridSize - PlayerCoodset.New.y);
                break;
            case 2:
                if (transform.position.x / GridSize <= Coordinate.x)
                    PlayerCoodset.New.x--;
                else
                    PlayerCoodset.Old.x++;
                percentOfCoord = (transform.position.x / GridSize - PlayerCoodset.New.x);
                break;
            case 3:
                if (transform.position.x / GridSize >= Coordinate.x)
                    PlayerCoodset.New.x++;
                else
                    PlayerCoodset.Old.x--;
                percentOfCoord = (PlayerCoodset.New.x - transform.position.x / GridSize);
                break;
        }

        //좌표가 달라지면 리스트에 좌표 추가
        if (OldCoord != PlayerCoodset.Old)
        {
            Coordlist.Insert(0, PlayerCoodset);
            Dirlist.Insert(0, Direction);
            OldCoord = PlayerCoodset.Old;
        }

        //꼬리 움직이기
        for (int i = 0; i < Droplist.Count; i++)
        {
            Droplist[i].GetComponent<ElementDrop>().DropNumber = i;

            if (i < Coordlist.Count)
            {
                Vector3 temp = Coordlist[i].Old * GridSize;
                if (Dirlist[i] == 0) //위로
                {
                    temp.y = (Coordlist[i].Old.y - percentOfCoord) * GridSize;
                }
                else if (Dirlist[i] == 1) //아래로
                {
                    temp.y = (Coordlist[i].Old.y + percentOfCoord) * GridSize;
                }
                else if (Dirlist[i] == 2) //왼쪽
                {
                    temp.x = (Coordlist[i].Old.x + percentOfCoord) * GridSize;
                }
                else if (Dirlist[i] == 3) //오른쪽
                {
                    temp.x = (Coordlist[i].Old.x - percentOfCoord) * GridSize;
                }
                Droplist[i].transform.position = temp;
            }
        }

        //방향 리스트에 방향 저장.
        for (int i = 0; i < Droplist.Count; i++)
        {
            Droplist[i].GetComponent<ElementDrop>().Direction = Dirlist[i];
        }

        //필요 없어진 옛날 좌표는 리스트에서 제거
        if (Coordlist.Count > Droplist.Count + 10)
        {
            Coordlist.RemoveAt(Droplist.Count + 10);
            Dirlist.RemoveAt(Droplist.Count + 10);
        }
    }

    //Position을 GridSize단위의 좌표로 바꿔주는 함수
    void SetCoordinate()
    {
        Coordinate.x = transform.position.x;
        Coordinate.y = transform.position.y;
        if (Coordinate.x >= 0)
            Coordinate.x = (int)(Coordinate.x / GridSize + 0.5);
        else
            Coordinate.x = (int)(Coordinate.x / GridSize - 0.5);
        if (Coordinate.y >= 0)
            Coordinate.y = (int)(Coordinate.y / GridSize + 0.5);
        else
            Coordinate.y = (int)(Coordinate.y / GridSize - 0.5);
    }

    //방향전환
    void ChangeDir()
    {
        if (!setDirCoord) //어디까지 이동 후 방향전환 하는지 설정
        {
            DirCoord = Coordinate;
            if (transform.position.y / GridSize >= DirCoord.y && Direction == 0)
                DirCoord.y += 1;
            if (transform.position.y / GridSize <= DirCoord.y && Direction == 1)
                DirCoord.y -= 1;
            if (transform.position.x / GridSize <= DirCoord.x && Direction == 2)
                DirCoord.x -= 1;
            if (transform.position.x / GridSize >= DirCoord.x && Direction == 3)
                DirCoord.x += 1;
            setDirCoord = true;
        }
        //다음 그리드까지 이동 후 방향전환
        if (transform.position.y / GridSize < DirCoord.y && Direction == 0)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.y / GridSize > DirCoord.y && Direction == 1)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.x / GridSize > DirCoord.x && Direction == 2)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else if (transform.position.x / GridSize < DirCoord.x && Direction == 3)
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
        else
        {
            setDirCoord = false;
            transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
            Direction = inputDirection;
            canCangeDir = true;
        }
    }

    //움직임
    void Move()
    {
        if (Direction != inputDirection) // 방향전환 있을 경우
        {
            ChangeDir(); //방향전환
        }
        if (Direction == inputDirection) // 방향전환 없을 경우
        {
            animaitor.SetInteger("direction", Direction);
            switch (Direction) // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
            {
                case 0:
                    MoveVec = new Vector3(0, 1, 0);
                    break;
                case 1:
                    MoveVec = new Vector3(0, -1, 0);
                    break;
                case 2:
                    MoveVec = new Vector3(-1, 0, 0);
                    break;
                case 3:
                    MoveVec = new Vector3(1, 0, 0);
                    break;
            }
            transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
            //GetComponent<Rigidbody2D>().MovePosition(transform.position + MoveVec * MoveSpeed * Time.deltaTime); //Rigidbody를 사용할경우 이걸로
        }
    }

    //맨 뒤에 구슬 하나 추가
    void InsertDrop(int element)
    {
        Vector3 temp = Coordlist[Droplist.Count].Old * GridSize;

        if (Droplist.Count < Coordlist.Count)
        {
            if (Coordlist[Droplist.Count].New.y > Coordlist[Droplist.Count].Old.y) //위로
            {
                temp.y = (Coordlist[Droplist.Count].Old.y - percentOfCoord) * GridSize;
            }
            else if (Coordlist[Droplist.Count].New.y < Coordlist[Droplist.Count].Old.y) //아래로
            {
                temp.y = (Coordlist[Droplist.Count].Old.y + percentOfCoord) * GridSize;
            }
            else if (Coordlist[Droplist.Count].New.x < Coordlist[Droplist.Count].Old.x) //왼쪽
            {
                temp.x = (Coordlist[Droplist.Count].Old.x + percentOfCoord) * GridSize;
            }
            else if (Coordlist[Droplist.Count].New.x > Coordlist[Droplist.Count].Old.x) //오른쪽
            {
                temp.x = (Coordlist[Droplist.Count].Old.x - percentOfCoord) * GridSize;
            }
            //Droplist[Droplist.Count].transform.position = temp;
        }

        switch (element)
        {
            case 0: //불속성
                Droplist.Add(Instantiate(FireDrop, temp, transform.rotation));
                break;
            case 1: //물속성
                Droplist.Add(Instantiate(WaterDrop, temp, transform.rotation));
                break;
            case 2: //바람속성
                Droplist.Add(Instantiate(WindDrop, temp, transform.rotation));
                break;
            case 3: //땅속성
                Droplist.Add(Instantiate(EarthDrop, temp, transform.rotation));
                break;
            case 4: //방해속성
                Droplist.Add(Instantiate(NullDrop, temp, transform.rotation));
                break;
        }
        MoveTails();
    }

    //맨 앞에있는 구슬 제거 (방해구슬은 무시함)
    void RemoveDrop()
    {
        if (Droplist.Count > NullElementNum)
        {
            Destroy(Droplist[NullElementNum]);
            Droplist.RemoveAt(NullElementNum);
        }
    }

    //e번째 꼬리 구슬 제거
    void RemoveDropAt(int e)
    {
        if (Droplist.Count > e)
        {
            Destroy(Droplist[e]);
            Droplist.RemoveAt(e);
        }
    }

    //유 다희
    void Die()
    {
        animaitor.SetBool("Alive", false);
        GameOver = true;
        NormalCanvas.SetActive(false);
        Invoke("DeadScreen", 1.3f);
    }

    void DeadScreen()
    {
        GameOverCanvas.SetActive(true);
    }

    //꼬리 길이를 리턴
    public int GetTailLength()
    {
        return Droplist.Count;
    }

    //방해구슬을 제외한 e+1번째 꼬리의 속성을 리턴. 0을 넣으면 첫번째를 리턴
    public int GetElement(int e)
    {
        if (Droplist.Count == NullElementNum) return 5; //임시방편으로 반환할 값이 없으면 5(비어있는 속성값)를 반환.
        return Droplist[e + NullElementNum].GetComponent<ElementDrop>().Element;
    }

    //플레이어가 죽었는지 안죽었는지 다른 스크립트에서 알고싶을때 사용
    public bool GetGameOver()
    {
        return GameOver;
    }

    //무적 상태 리턴
    public bool GetImmortal()
    {
        return immortal;
    }
}