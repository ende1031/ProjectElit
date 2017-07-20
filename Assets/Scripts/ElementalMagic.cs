using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalMagic : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public int Element; // 0:불, 1:물, 2:바람, 3:땅, 4이후:속성조합마법(추후 지정)
    public GameObject ExplosionEff;

    private Quaternion angle = Quaternion.identity;
    int dir = 0;
    PlayerController move;
    Vector3 MoveVec; //방향 벡터
    Vector2 Coordinate; //좌표
    float GridSize;
    float aliveTime;

    GameObject RenderObject_start;
    GameObject RenderObject_shooting;

    Vector2 effect_offset;

    // Use this for initialization
    void Start()
    {
        RenderObject_start = transform.Find("start").gameObject;
        RenderObject_shooting = transform.Find("shooting").gameObject;
        move = GameObject.Find("Player").GetComponent<PlayerController>();
        GridSize = move.GridSize;
        aliveTime = 0;
        dir = move.getDirection();

        Vector3 tempPosition;

        if(Element == 0)
        {
            effect_offset.x = 0.72f;
            effect_offset.y = 0.03f;
        }
        else if (Element == 1)
        {
            effect_offset.x = 0.16f;
            effect_offset.y = 0;
        }
        else if (Element == 2)
        {
            effect_offset.x = 0.18f;
            effect_offset.y = 0;
        }


        switch (Element)
        {
            case 3:
                //실드 애니메이션 여기에
                break;

            default:
                if (dir == 0) // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
                {
                    MoveVec = new Vector3(0, 1, 0);
                    RenderObject_start.transform.Rotate(0, 0, -90);
                    RenderObject_shooting.transform.Rotate(0, 0, -90);
                    tempPosition = new Vector3(RenderObject_start.transform.position.x + effect_offset.y, RenderObject_start.transform.position.y - effect_offset.x, RenderObject_start.transform.position.z);
                    RenderObject_shooting.transform.position = tempPosition;
                }
                else if (dir == 1)
                {
                    MoveVec = new Vector3(0, -1, 0);
                    RenderObject_start.transform.Rotate(0, 0, 90);
                    RenderObject_shooting.transform.Rotate(0, 0, 90);
                    tempPosition = new Vector3(RenderObject_start.transform.position.x - effect_offset.y, RenderObject_start.transform.position.y + effect_offset.x, RenderObject_start.transform.position.z);
                    RenderObject_shooting.transform.position = tempPosition;
                }
                else if (dir == 2)
                {
                    MoveVec = new Vector3(-1, 0, 0);
                    tempPosition = new Vector3(RenderObject_start.transform.position.x + effect_offset.x, RenderObject_start.transform.position.y + effect_offset.y, RenderObject_start.transform.position.z);
                    RenderObject_shooting.transform.position = tempPosition;
                }
                else if (dir == 3)
                {
                    MoveVec = new Vector3(1, 0, 0);
                    RenderObject_start.transform.Rotate(0, 0, 180);
                    RenderObject_shooting.transform.Rotate(0, 0, 180);
                    tempPosition = new Vector3(RenderObject_start.transform.position.x - effect_offset.x, RenderObject_start.transform.position.y - effect_offset.y, RenderObject_start.transform.position.z);
                    RenderObject_shooting.transform.position = tempPosition;
                }
                break;
        }
    }

    //시작이펙트 끝나고 지속이펙트로 넘어감
    public void Starting_end()
    {
        RenderObject_start.SetActive(false);
        RenderObject_shooting.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SetCoordinate();

        switch (Element)
        {
            case 3:
                shield();
                break;
            default:
                Fireball();
                break;
        }
    }

    // 0 ~ 2 : 발사체 공격
    void Fireball()
    {
        transform.Translate(MoveVec * moveSpeed * Time.deltaTime);

        if (ObjectManager.instance.isPlace(Coordinate, "tree") || ObjectManager.instance.isPlace(Coordinate, "monster"))
        {
            //ObjectManager.instance.PlacedObject(Coordinate, "monster").GetComponent<Monster>().Hit();
            Instantiate(ExplosionEff, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            Destroy(this.gameObject);
        }

        aliveTime += Time.deltaTime;
        if (aliveTime > 5.0f) //메모리상에 남아있는걸 방지
            Destroy(this.gameObject);
    }

    // 3 : 흙마법 실드
    void shield()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().GetImmortal())
        {
            transform.position = GameObject.Find("Player").transform.position;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
}
