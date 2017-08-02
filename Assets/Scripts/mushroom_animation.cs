using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroom_animation : MonoBehaviour
{

    private Animator animaitor;

    public AnimatorOverrideController Left_Ani;
    public AnimatorOverrideController Right_Ani;


    int mush_direction = 1;

    private void Start()
    {
        animaitor = GetComponent<Animator>();
    }

    void Update()
    {
        mush_direction = animaitor.GetInteger("Direction");

        if (mush_direction == 2)
            GetComponent<Animator>().runtimeAnimatorController = Left_Ani;

        if (mush_direction == 3)
            GetComponent<Animator>().runtimeAnimatorController = Right_Ani;
    }
}
