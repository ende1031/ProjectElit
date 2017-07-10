using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animaitor;

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
    float orgMoveSpeed; //플레이어의 속도를 임시로 저장 (원래 속도가 얼마였는지 알기 위해)
    public float GridSize; //한 칸의 크기 (단위 : 100분의 1픽셀)

    public GameObject FireDrop;
    public GameObject WaterDrop;
    public GameObject WindDrop;
    public GameObject EarthDrop;
    public GameObject NullDrop;

    public GameObject Fireshot;
    public GameObject Watershot;
    public GameObject Windshot;
    public GameObject Sandshot;

    public AudioClip fireSound;
    public AudioClip windSound;
    public AudioClip waterSound;
    public AudioClip sandSound;

    //움직임 및 방향전환 관련 변수
    Vector2 Coordinate; //좌표
    Vector3 MoveVec; //방향 벡터
    int Direction; // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
    Vector2 DirCoord; //방향전환용 임시좌표
    bool setDirCoord;
    int HitDirection; // 맞았을 때 방향

    //키입력 관련
    int inputDirection;

    //꼬리 리스트
    List<GameObject> Droplist = new List<GameObject>();
    //지나온 좌표를 저장하는 리스트
    List<Coordset> Coordlist = new List<Coordset>();
    //지나온 방향을 저장하는 리스트 이걸로 버그가 해결된다면 좋겠습니다.
    List<int> Dirlist = new List<int>();

    //꼬리를 움직이게 하기 위한 변수
    Coordset PlayerCoodset;
    Vector2 OldCoord;
    float percentOfCoord;

    bool isHit;
    float hitTimer;

    bool canCangeDir;

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
    }

    //충돌처리
    void OnTriggerEnter2D(Collider2D col)
    {
        //꼬리 구슬 추가
        if (col.gameObject.tag == "fire_ball")
        {
            Destroy(col.gameObject);
            InsertDrop(0);
        }
        else if (col.gameObject.tag == "wind_ball")
        {
            Destroy(col.gameObject);
            InsertDrop(2);
        }
        else if (col.gameObject.tag == "water_ball")
        {
            Destroy(col.gameObject);
            InsertDrop(1);
        }
        else if (col.gameObject.tag == "sand_ball")
        {
            Destroy(col.gameObject);
            InsertDrop(3);
        }

        else if (col.gameObject.tag == "tree" || col.gameObject.tag == "monster")
        {
            hitTimer = 1.0f;
            isHit = true;
            HitDirection = Direction;

            canCangeDir = true;
            transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
        }
    }

    //맞았을 때 실행
    void Hit()
    {
        Debug.Log("충돌중");
        hitTimer += Time.deltaTime;
        if (HitDirection == inputDirection || Direction + inputDirection == 1 || Direction + inputDirection == 5 || HitDirection + inputDirection == 1 || HitDirection + inputDirection == 5)
        {
            MoveSpeed = 0;
            percentOfCoord = 0;
            if (hitTimer > 0.5f) // 이 시간 주기로 꼬리 하나씩 감소
            {
                RemoveDrop();
                hitTimer = 0;
            }
        }
        else if (hitTimer > 0.3f)
        {
            MoveSpeed = orgMoveSpeed;
            isHit = false;
            Direction = inputDirection;
            //transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
        }

        if (hitTimer > 0.2f)
        {
            //canCangeDir = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Direction + " / " + inputDirection);
        //맞았을 때 매 프레임 실행
        if (isHit)
        {
            Hit();
        }
        //else
        {
            MoveTails();
        }

        if(canCangeDir)
        {
            InputKey();
        }

        SetCoordinate();


        if (Input.GetKeyUp(KeyCode.Space)) // 불 바람 물 땅 마법 사용
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (GetElement(0) == 0)
        {
            Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), transform.rotation);
            SoundManager.instance.RandomizeSfx(fireSound);//마법 사용시에 불, 바람, 물, 땅 사운드 출력
        }
        else if (GetElement(0) == 1)
        {
            Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), transform.rotation);
            SoundManager.instance.RandomizeSfx(waterSound);
        }
        else if (GetElement(0) == 2)
        {
            Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), transform.rotation);
            SoundManager.instance.RandomizeSfx(windSound);
        }
        else if (GetElement(0) == 3)
        {
            Instantiate(Sandshot, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), transform.rotation);
            SoundManager.instance.RandomizeSfx(sandSound);
        }
        RemoveDrop();
    }

    //움직임 관련은 물리시간마다 호출
    void FixedUpdate()
    {
        //if (!isHit)
            Move();
    }

    //나중에 터치&드래그로 바꾸기
    void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(Direction == 3 || Direction == 2)
                return;

            inputDirection = 2;
            Debug.Log("왼쪽입력");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Direction == 2 || Direction == 3)
                return;

            inputDirection = 3;
            Debug.Log("오른쪽입력");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Direction == 1 || Direction == 0)
                return;

            inputDirection = 0;
            Debug.Log("위쪽입력");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Direction == 0 || Direction == 1)
                return;

            inputDirection = 1;
            Debug.Log("아래쪽입력");
        }
    }

    public int getDirection() //Direction 값 반환
    {
        return Direction;
    }

    //움직여라 꼬리꼬리 (매 프레임마다 실행)
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

    //Position을 좌표로 바꿔주는 함수 (매 프레임마다 실행)
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

    //움직임 (매 프레임마다 실행)
    void Move()
    {
        if (Direction != inputDirection) // 방향전환 있을 경우
        {
            canCangeDir = false;
            if (Direction + inputDirection == 1 || Direction + inputDirection == 5) //이동방향과 입력방향이 정반대
            {
                //Direction = inputDirection; //그냥 바로 뒤돌아서 감(X)
                transform.Translate(MoveVec * MoveSpeed * Time.deltaTime); //180도 뒤도는건 못함(꼬리가 있기 때문). 그냥 원래 가던 방향으로 감.
            }
            else //그리드단위 이동 후 방향전환
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
                if (!isHit)
                {
                    //다음 그리드까지 이동 후 방향전환
                    if (transform.position.y / GridSize <= DirCoord.y && Direction == 0)
                        transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
                    else if (transform.position.y / GridSize >= DirCoord.y && Direction == 1)
                        transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
                    else if (transform.position.x / GridSize >= DirCoord.x && Direction == 2)
                        transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
                    else if (transform.position.x / GridSize <= DirCoord.x && Direction == 3)
                        transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
                    else
                    {
                        setDirCoord = false;
                        transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
                        Direction = inputDirection;
                        canCangeDir = true;
                    }
                }
                else
                {
                    setDirCoord = false;
                    Direction = inputDirection;
                    transform.position = new Vector3(Coordinate.x * GridSize, Coordinate.y * GridSize, transform.position.z);
                    canCangeDir = true;
                    return;
                }
            }
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
            //transform.Translate(MoveVec * MoveSpeed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(transform.position + MoveVec * MoveSpeed * Time.deltaTime);
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

    //맨 앞에있는 구슬 제거
    void RemoveDrop()
    {
        if (Droplist.Count > 0)
        {
            Destroy(Droplist[0]);
            Droplist.RemoveAt(0);
        }
    }

    //꼬리 길이를 리턴
    int GetTailLength()
    {
        return Droplist.Count;
    }

    //e+1번째 꼬리의 속성을 리턴. 0을 넣으면 첫번째를 리턴
    int GetElement(int e)
    {
        return Droplist[e].GetComponent<ElementDrop>().Element;
    }
}