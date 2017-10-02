using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public float destroy_delay;

	// Use this for initialization
	void Start () {
        Invoke("DestroyObject", destroy_delay);
    }

    void DestroyObject()
    {
        ObjectManager.instance.FieldElementNum--;
        Destroy(this.gameObject);
    }
}
