using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public GameObject Fireshot;
    public GameObject Watershot;
    public GameObject Windshot;
    public GameObject Sandshot;

    public GameObject EarthWater;
    public GameObject EarthWind;
    public GameObject EarthFire;

    public bool buttonDown;
    public bool buttonUP;
    float chargeTimer;
    
    int[] Chargelist = new int[5];

    PlayerController PlayerRef;

    // Use this for initialization
    void Start () {
        PlayerRef = GetComponent<PlayerController>();
        chargeTimer = 0;
        buttonUP = true;
        buttonDown = false;
    }
	
	// Update is called once per frame
	void Update () {

        //연결 이펙트
        GetComponent<PlayerController>().ChainEffect(Chargelist[4]);
        if (Chargelist[4] > 0) Debug.Log(Chargelist[4] + "개 차지중");

        //공격
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MagicDistinguisher();
        }

        //Chargelist에 플레이어 뒤 3개의 원소 저장
        if (Input.GetKeyDown(KeyCode.Space) || (buttonDown && buttonUP))
        {
            for (int i = 0; i < 2; i++)
            {
                if (PlayerRef.GetTailLength() - PlayerRef.NullElementNum < i + 1) break; // 꼬리 길이가 3개보다 적으면 패스

                //불 = 0, 물 = 1, 바람 = 2, 땅 = 3
                if (PlayerRef.GetElement(i) == 0)
                {
                    if (Chargelist[1] > 0) break;
                    Chargelist[0] += 1;
                    Chargelist[4]++; //차지리스트에 몇개의 원소가 들어갔는지 저장
                }

                else if (PlayerRef.GetElement(i) == 1)
                {
                    if (Chargelist[0] > 0) break;
                    Chargelist[1] += 1;
                    Chargelist[4]++;
                }

                else if (PlayerRef.GetElement(i) == 2)
                {
                    Chargelist[2] += 1;
                    Chargelist[4]++;
                }
                else if (PlayerRef.GetElement(i) == 3)
                {
                    Chargelist[3] += 1;
                    Chargelist[4]++;
                }
                else if (PlayerRef.GetElement(i) == 4)
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
                ChargeEffect.instance.ChangeE(ChargeEffect.instance.third_Canvas, PlayerRef.GetElement(2));
                ChargeEffect.instance.Chargethird();
            }

            else if (chargeTimer > 1 && ChargeEffect.instance.charge_stat == 1 && Chargelist[4] > 1)
            {
                ChargeEffect.instance.ChangeE(ChargeEffect.instance.ssecond_Canvas, PlayerRef.GetElement(1));
                ChargeEffect.instance.ChangeE(ChargeEffect.instance.second_Canvas, PlayerRef.GetElement(1));
                ChargeEffect.instance.ChargeSSecond();
            }

            else if (ChargeEffect.instance.charge_stat == 0 && Chargelist[4] > 0)
            {
                ChargeEffect.instance.ChangeE(ChargeEffect.instance.first_Canvas, PlayerRef.GetElement(0));
                ChargeEffect.instance.Chargefirst();
            }
        }

    }
    //마법 판별기
    public void MagicDistinguisher()
    {
        // 불 바람 물 땅 마법 사용
        /*
        if (chargeTimer > 2)
        {
            if (Chargelist[0] == 3) //불3
            {
            }
            else if (Chargelist[1] == 3)//물3
            {
            }
            else if (Chargelist[2] == 3)//바람3
            {
            }
            else if (Chargelist[3] == 3)//흙3
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[2] == 2 || Chargelist[0] == 2 && Chargelist[2] == 1)//불+바람강화판
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[3] == 2 || Chargelist[0] == 2 && Chargelist[3] == 1)//불+흙
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[2] == 2 || Chargelist[1] == 2 && Chargelist[2] == 1)//물+바람
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[3] == 2 || Chargelist[1] == 2 && Chargelist[3] == 1)//물+흙
            {
            }
            else if (Chargelist[2] == 1 && Chargelist[3] == 2 || Chargelist[2] == 2 && Chargelist[3] == 1)//바람+흙
            {
            }
            else if (Chargelist[0] == 1 && Chargelist[2] == 1 && Chargelist[3] == 1 )//불+바람+흙
            {
            }
            else if (Chargelist[1] == 1 && Chargelist[2] == 1 && Chargelist[3] == 1)//물+바람+흙
            {
            }
            
        }
        */

        if (chargeTimer > 1)//1초이상 차지 하였을때
        {
            if (Chargelist[0] == 2)//불+불
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 0);
                Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                Instantiate(Fireshot, new Vector3(transform.position.x - 0.02f, transform.position.y, 0), transform.rotation);
                GameManager.instance.PlaySE("Fire");
            }

            else if (Chargelist[1] == 2)//물+물
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 1);
                Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                Instantiate(Watershot, new Vector3(transform.position.x-0.02f, transform.position.y, 0), transform.rotation);
                GameManager.instance.PlaySE("Water");
            }

            else if (Chargelist[2] == 2)//바람+바람
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 2);
                Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                Instantiate(Windshot, new Vector3(transform.position.x - 0.02f, transform.position.y, 0), transform.rotation);
                GameManager.instance.PlaySE("Wind");
            }

            else if (Chargelist[3] == 2)//흙+흙
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 3);
                if (PlayerRef.immortal_timer == 0)
                    Instantiate(Sandshot, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

                PlayerRef.immortal_timer = 3;
                PlayerRef.immortal = true;
                GameManager.instance.PlaySE("Sand");
            }

            else if (Chargelist[0] == 1 && Chargelist[2] == 1)//불+바람
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 0);
                Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                GameManager.instance.PlaySE("Fire");
            }

            else if (Chargelist[0] == 1 && Chargelist[3] == 1)//불+흙
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 3);
                if (PlayerRef.immortal_timer == 0)
                    Instantiate(EarthFire, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                PlayerRef.immortal_timer = 2;
                PlayerRef.immortal = true;
                GameManager.instance.PlaySE("Sand");
            }

            else if (Chargelist[1] == 1 && Chargelist[2] == 1)//물+바람
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 1);
                Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
                GameManager.instance.PlaySE("Water");
            }

            else if (Chargelist[1] == 1 && Chargelist[3] == 1)//물+흙
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 3);
                if (PlayerRef.immortal_timer == 0)
                    Instantiate(EarthWater, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                PlayerRef.immortal_timer = 2;
                PlayerRef.immortal = true;
                GameManager.instance.PlaySE("Sand");
            }

            else if (Chargelist[2] == 1 && Chargelist[3] == 1)//바람+흙
            {
                PlayerRef.attack_ing = true;
                PlayerRef.animaitor.SetInteger("Attack_element", 3);
                if (PlayerRef.immortal_timer == 0)
                    Instantiate(EarthWind, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                PlayerRef.immortal_timer = 2;
                PlayerRef.immortal = true;
                GameManager.instance.PlaySE("Sand");
            }

            if (PlayerRef.attack_ing)
            {
                ResetAttack(Chargelist[4]);
                return;
            }
        }
        
        if (PlayerRef.GetElement(0) == 0)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 0);
            Instantiate(Fireshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Fire");
        }
        else if (PlayerRef.GetElement(0) == 1)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 1);
            Instantiate(Watershot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Water");
        }
        else if (PlayerRef.GetElement(0) == 2)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 2);
            Instantiate(Windshot, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            GameManager.instance.PlaySE("Wind");
        }
        else if (PlayerRef.GetElement(0) == 3)
        {
            PlayerRef.attack_ing = true;
            PlayerRef.animaitor.SetInteger("Attack_element", 3);
            if (PlayerRef.immortal_timer == 0)
                Instantiate(Sandshot, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            PlayerRef.immortal_timer = 3;
            PlayerRef.immortal = true;
            GameManager.instance.PlaySE("Sand");
        }
        ResetAttack(1);//아직 조합이 안되서 1로 고정
    }
    
    //UI상의 버튼이 눌렸는지 안눌렸는지 확인
    public void OnPointerDown()
    {
        buttonDown = true;
    }

    public void OnPointerUp()
    {
        buttonDown = false;
    }
    
    public void ResetAttack(int charge_num)
    {
        // 꼬리에 몇개의 원소를 자를건지 결정
        for (int i = 0; i < charge_num; i++)
            PlayerRef.RemoveDrop();

        //버튼 누른 시간, 원소조합 초기화
        chargeTimer = 0;
        for (int j = 0; j < 5; j++)
            Chargelist[j] = 0;

        //버튼 주위 원소 비활성화
        ChargeEffect.instance.ChargeReset();
        buttonUP = true;
        buttonDown = false;
    }

}
