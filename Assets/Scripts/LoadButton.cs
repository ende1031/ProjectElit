using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public AnimatorOverrideController FadeOut, FadeIn;
    public GameObject[] ChapterNum = new GameObject[3];
    float tempTime;
    int chapterNow, chapterBefore;

    void Start()
    {
        chapterNow = 0;
        chapterBefore = -1;
        tempTime = 0.1f;
    }

    void Update()
    {
        if (tempTime > 0)
        {
            tempTime -= Time.deltaTime;
        }

        if (tempTime <= 0 && chapterNow != chapterBefore)//이전 챕터와 다를때만 적용
        {
            chapterBefore = chapterNow;
            for (int i = 0; i < 3; i++)
            {
                if (chapterNow == i)
                {
                    FadeInButton(chapterNow);
                }
                else
                {
                    FadeOutButton(i);
                }
            }
        }
    }

    public void FadeButton(int x, float y)//x 는 페이드 인 할 챕터 번호, y 는 애니메이션 적용 지연시간
    {
        tempTime = y;
        chapterNow = (x - 1);
    }

    void FadeInButton(int x)
    {
        ChapterNum[chapterNow].SetActive(false);
        ChapterNum[chapterNow].SetActive(true);
        ChapterNum[x].GetComponent<Animator>().runtimeAnimatorController = FadeIn;
    } 

    void FadeOutButton(int x)
    {
        ChapterNum[x].GetComponent<Animator>().runtimeAnimatorController = FadeOut;
    }
    
    
}