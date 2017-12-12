using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCount : MonoBehaviour {

    public static CoinCount instance;
    public Text scoretext;
    private int score = 0;

    void Awake()
    {
        if (!instance) 
        instance = this; 
    }

    public void AddScore(int num) 
    {
        score += num; 
        scoretext.text = " " + score; 
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
