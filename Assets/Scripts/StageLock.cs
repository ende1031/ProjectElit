﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StageLock : MonoBehaviour {

    public Image[] Stage = new Image[15];
    public Sprite Locked;
    public Sprite UnClear;

    int UnlockedStage;

	// Use this for initialization
	void Start () {
        UnlockedStage = GameObject.Find("GameManager").GetComponent<SaveManager>().OutputStage();
        Debug.Log(UnlockedStage);
        SelectLock();
    }

    void SelectLock()
    {
        for (int i = 0; i < 15; i++)
        {
            if (Stage[i] == null) continue;
            if (i > UnlockedStage)
            {
                Stage[i].sprite = Locked;
            }
            if (i == UnlockedStage) Stage[i].sprite = UnClear;
        }
    }
}
