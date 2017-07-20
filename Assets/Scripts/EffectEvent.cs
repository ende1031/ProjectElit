using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이펙트 애니메이션에서 사용하는 함수 모음
public class EffectEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }

    void Shooting()
    {
        transform.parent.gameObject.GetComponent<ElementalMagic>().Starting_end();
    }
	
    void Destroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
