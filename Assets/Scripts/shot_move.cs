using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot_move : MonoBehaviour {

    private Animator anim;
    private Quaternion angle = Quaternion.identity;
    public float moveSpeed = 10.0f;
    public int dir = 0;
    PlayerController move;

    bool isHit;
    float aliveTime;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        move = GameObject.Find("Player").GetComponent<PlayerController>();

        isHit = false;
        aliveTime = 0;

        if (move.getDirection() == 0) // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
        {
            dir = 0;
            transform.Rotate(0, 0, -90);
        }
        else if (move.getDirection() == 1)
        {
            dir = 1;
            transform.Rotate(0, 0, 90);
        }
        else if (move.getDirection() == 2)
        {
            dir = 2;
        }
        else if (move.getDirection() == 3)
        {
            dir = 3;
            transform.Rotate(0, 0, 180);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "tree" || col.gameObject.tag == "monster")
        {
            anim.SetBool("hit", true);
            isHit = true;

            //이펙트를 항상 위쪽에 나오도록. 오브젝트에 가리지 않게.
            GetComponent<AutoPositionZ>().PositionZ = false;
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isHit)
        {
            if (dir == 0) // 0 : 위, 1 : 아래, 2 : : 왼쪽, 3 : 오른쪽
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
            else if (dir == 1)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
            else if (dir == 2)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
            else if (dir == 3)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }

            aliveTime += Time.deltaTime;
            if (aliveTime > 5.0f) //폭발 안한게 계속 메모리상에 남아있는걸 방지
                Destroy();
        }
    }

    void Destroy() //애니메이터에서도 사용하는 함수임.
    {
        Destroy(this.gameObject);
    }
}
