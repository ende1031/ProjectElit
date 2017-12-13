using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public GameObject Fireshot;
    public GameObject Watershot;
    public GameObject Windshot;

    float feverTimer;
    
    PlayerController PlayerRef;

    // Use this for initialization
    void Start () {
        PlayerRef = GetComponent<PlayerController>();
        feverTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {        
        //공격
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MagicDistinguisher();
        }
        //피버 타이머
        if (feverTimer > 0)
        {
            feverTimer -= Time.deltaTime;
        }
 
    }
    //마법 판별기
    public void MagicDistinguisher()
    {
        if (PlayerRef.GetElement(0) == 0 || PlayerRef.GetElement(0) == 5)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 0);
            Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Fire");
        }
        else if (PlayerRef.GetElement(0) == 1 || PlayerRef.GetElement(0) == 6)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 1);
            Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Water");
        }
        else if (PlayerRef.GetElement(0) == 2 || PlayerRef.GetElement(0) == 7)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 2);
            Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Wind");
        }

        if (PlayerRef.GetElement(0) > 4 && PlayerRef.GetFever(0) == false)
        {
            ResetAttack(1);
            feverTimer = 5;
            PlayerRef.SetFever(0, true);
        }

        ResetAttack(1);//1개의 원소 삭제
    }
    
    public void ResetAttack(int charge_num)
    {
        if (feverTimer>0)
        {
            return;
        }
        PlayerRef.RemoveDrop();
    }

}
