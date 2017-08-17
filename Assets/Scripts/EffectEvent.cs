using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각종 유니티 애니메이션에서 사용하는 함수 모음
//어디에 쓰는건지 잊어버릴 확률이 높으므로 주석 꼭 달기
public class EffectEvent : MonoBehaviour {

    //파이어볼 등의 이펙트가 발사이펙트에서 지속이펙트로 넘어가게 해줌
    void Shooting()
    {
        transform.parent.gameObject.GetComponent<ElementalMagic>().Starting_end();
    }
	
    //파이어볼 폭발 이펙트 종료시 부모오브젝트 삭제
    void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }

    //현재 오브젝트 삭제
    void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    //몬스터가 공격을 마치고 다시 움직이게 해줌
    void MonsterAttackEnd()
    {
        transform.parent.GetComponent<Monster>().AttackEnd();
    }

    //몬스터가 공격하는 중 피격판정이 발생하는 타이밍에 실행
    void AttackHit()
    {
        transform.parent.GetComponent<Monster>().HitPlayer();
    }
}
