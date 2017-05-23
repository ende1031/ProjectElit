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

    //움직임 및 방향전환 관련 변수
    Vector2 Coordinate; //좌표
    Vector3 MoveVec; //방향 벡터
    int Direction; // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
    int inputDirection;
    Vector2 DirCoord; //방향전환용 임시좌표
    bool setDirCoord;

    //꼬리 리스트
    List<GameObject> Droplist = new List<GameObject>();
    //지나온 좌표를 저장하는 리스트
    List<Coordset> Coordlist = new List<Coordset>();

    //꼬리를 움직이게 하기 위한 변수
    Coordset PlayerCoodset;
    Vector2 OldCoord;
    float percentOfCoord;

    // Use this for initialization
    void Start()
    {
        Direction = 2;
        inputDirection = 2;
        SetCoordinate();
        setDirCoord = false;

        PlayerCoodset = new Coordset(Coordinate, Coordinate);
        OldCoord = PlayerCoodset.Old;
        animaitor = this.GetComponent<Animator>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        SetCoordinate();
        Move();
        InputKey();
        MoveTails();

        //======꼬리 추가 및 제거 예시=====
        if (Input.GetKeyUp(KeyCode.A))
            InsertDrop(0); //맨 뒤에 불속성(0) 꼬리 하나 추가

        if (Input.GetKeyUp(KeyCode.S))
            RemoveDrop(); //맨 앞에있는 꼬리 하나 제거

        if (Input.GetKeyUp(KeyCode.X)) // 불 바람 물 땅 마법 사용
        {

            if (GetFirstElement() == 0)
            {
                Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            else if (GetFirstElement() == 1)
            {
                Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            else if (GetFirstElement() == 2)
            {
                Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
            else if (GetFirstElement() == 3)
            {
                Instantiate(Sandshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            }
                RemoveDrop();
        }
    }

    //나중에 터치&드래그로 바꾸기
    void InputKey()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animaitor.SetInteger("direction", 2);
            inputDirection = 2;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animaitor.SetInteger("direction", 3);
            inputDirection = 3;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            animaitor.SetInteger("direction", 1);
            inputDirection = 0;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animaitor.SetInteger("direction", 0);
            inputDirection = 1;
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
        if(OldCoord != PlayerCoodset.Old)
        {
            Coordlist.Insert(0, PlayerCoodset);
            OldCoord = PlayerCoodset.Old;
        }

        //꼬리 움직이기
        for (int i = 0; i < Droplist.Count; i++)
        {
            if (i < Coordlist.Count)
            {
                Vector3 temp = Coordlist[i].Old * GridSize;
                if (Coordlist[i].New.y > Coordlist[i].Old.y) //위로
                {
                    temp.y = (Coordlist[i].Old.y - percentOfCoord) * GridSize;
                }
                else if (Coordlist[i].New.y < Coordlist[i].Old.y) //아래로
                {
                    temp.y = (Coordlist[i].Old.y + percentOfCoord) * GridSize;
                }
                else if (Coordlist[i].New.x < Coordlist[i].Old.x) //왼쪽
                {
                    temp.x = (Coordlist[i].Old.x + percentOfCoord) * GridSize;
                }
                else if (Coordlist[i].New.x > Coordlist[i].Old.x) //오른쪽
                {
                    temp.x = (Coordlist[i].Old.x - percentOfCoord) * GridSize;
                }
                Droplist[i].transform.position = temp;
            }
        }

        //필요 없어진 옛날 좌표는 리스트에서 제거
        if (Coordlist.Count > Droplist.Count + 10)
        {
            Coordlist.RemoveAt(Droplist.Count + 10);
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
        if (Direction == inputDirection) // 방향전환 없을 경우
        {
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
        }
        else // 방향전환 있을 경우
        {
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
                }
            }
        }
    }

    //맨 뒤에 구슬 하나 추가
    void InsertDrop(int element)
    {
        switch (element)
        {
            case 0: //불속성
                Droplist.Add(Instantiate(FireDrop, transform.position, transform.rotation));
                break;
            case 1: //물속성
                Droplist.Add(Instantiate(WaterDrop, transform.position, transform.rotation));
                break;
            case 2: //바람속성
                Droplist.Add(Instantiate(WindDrop, transform.position, transform.rotation));
                break;
            case 3: //땅속성
                Droplist.Add(Instantiate(EarthDrop, transform.position, transform.rotation));
                break;
            case 4: //방해속성
                Droplist.Add(Instantiate(NullDrop, transform.position, transform.rotation));
                break;
        }
        MoveTails();
    }

    //맨 앞에있는 구슬 제거
    void RemoveDrop()
    {
        Destroy(Droplist[0]);
        Droplist.RemoveAt(0);
    }

    //꼬리 길이를 리턴
    int GetTailLength()
    {
        return Droplist.Count;
    }

    //첫 번째 꼬리의 속성을 리턴
    int GetFirstElement()
    {
        return Droplist[0].GetComponent<ElementDrop>().Element;
    }

    //두 번째 꼬리의 속성을 리턴(속성 조합시 사용)
    int GetSecondElement()
    {
        return Droplist[1].GetComponent<ElementDrop>().Element;
    }
}