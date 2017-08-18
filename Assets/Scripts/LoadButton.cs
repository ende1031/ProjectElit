using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public AnimatorOverrideController FadeOut, FadeIn;

    public void FadeButton(float x)//x 는 페이드 아웃 지연시간
    {
        FadeOutButton();
        Invoke("FadeInButton", x);
    }

    void FadeInButton()
    {
        this.GetComponent<Animator>().runtimeAnimatorController = FadeIn;
    }

    void FadeOutButton()
    {
        this.GetComponent<Animator>().runtimeAnimatorController = FadeOut;
    }
}