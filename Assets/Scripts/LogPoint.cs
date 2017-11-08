using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPoint : MonoBehaviour {

    public GameObject Dialogue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDialogue()
    {
        Dialogue.SetActive(true);
    }
}
