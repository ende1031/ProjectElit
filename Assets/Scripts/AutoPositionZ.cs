


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPositionZ : MonoBehaviour {

    Vector3 tempPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tempPos = transform.position;
        tempPos.z = tempPos.y / 100;
        transform.position = tempPos;
    }
}
