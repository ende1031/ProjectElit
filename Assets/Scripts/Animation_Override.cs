using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Override : MonoBehaviour
{

    private Animator Animaitor;

    public AnimatorOverrideController Left_Ani;
    public AnimatorOverrideController Right_Ani;
    public AnimatorOverrideController Up_Ani;
    public AnimatorOverrideController Down_Ani;

    int Direction = 1;
    bool isAttack;
    bool isHit;

    private void Start()
    {
        Animaitor = GetComponent<Animator>();
    }

    void Update()
    {
        Direction = Animaitor.GetInteger("direction");

        if (Direction == 0)
            GetComponent<Animator>().runtimeAnimatorController = Up_Ani;

        if (Direction == 1)
            GetComponent<Animator>().runtimeAnimatorController = Down_Ani;

        if (Direction == 2)
            GetComponent<Animator>().runtimeAnimatorController = Left_Ani;

        if (Direction == 3)
            GetComponent<Animator>().runtimeAnimatorController = Right_Ani;


        Animaitor.SetBool("isAttack", isAttack);
        Animaitor.SetBool("isHit", isAttack);
    }
}
