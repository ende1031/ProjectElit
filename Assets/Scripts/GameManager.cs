using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public CanvasGroup faderCanvasGroup; //페이드인아웃 캔바스
    public float fadeDuration = 1f;
    private bool isFading;

    public string startingSceneName = "Title"; //첫 씬 이름.

    public AudioSource Sound;

    public AudioClip fireSound;
    public AudioClip windSound;
    public AudioClip waterSound;
    public AudioClip sandSound;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySE(AudioClip clip)//효과음
    {
        Sound.clip = clip;
        Sound.Play();
    }

    public void PlaySE(string clip)
    {
        switch (clip)
        {
            case "Fire":
                PlaySE(fireSound);
                break;

            case "Wind":
                PlaySE(windSound);
                break;

            case "Water":
                PlaySE(waterSound);
                break;

            case "Sand":
                PlaySE(sandSound);
                break;
        }
    }

    //첫 씬 로드
    private IEnumerator Start()
    {
        faderCanvasGroup.alpha = 1f;
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName));
        StartCoroutine(Fade(0f));
    }

    //신 체인지 할때 이 함수를 불러올 것
    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        if (sceneName != "persistent")
        {
            yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        }

        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //Debug.Log(sceneName);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;

        faderCanvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        faderCanvasGroup.blocksRaycasts = false;

        isFading = false;
    }
}
