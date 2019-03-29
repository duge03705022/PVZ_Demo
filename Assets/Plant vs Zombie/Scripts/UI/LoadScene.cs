using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public AudioClip bgmMusic;
    public GameObject logo;
    public GameObject loadingLayer;
    public GameObject title;
    public GameObject loadBar;
    public UnityEngine.UI.Button button;
    public UnityEngine.UI.Text text;

    private LoadBar loadBarScript;
    private AsyncOperation async;

	void Awake () {
        Color color = Color.white;
        color.a = 0;
        logo.GetComponent<SpriteRenderer>().color = color;
        loadBarScript = loadBar.GetComponent<LoadBar>();
        button.enabled = false;
        //Screen.SetResolution(900, 600, false);
	}

    void Start() {
        StartCoroutine(Workflow());
        AudioManager.GetInstance().PlayMusic(bgmMusic);
    }

    IEnumerator Workflow() {
        FadeIn fadeIn = logo.AddComponent<FadeIn>();
        fadeIn.time = 1.0f;
        fadeIn.Begin();
        yield return new WaitForSeconds(2f);

        FadeOut fadeOut = logo.AddComponent<FadeOut>();
        fadeOut.time = 1.0f;
        fadeOut.Begin();
        yield return new WaitForSeconds(1f);

        logo.SetActive(false);
        loadingLayer.SetActive(true);
        yield return new WaitForEndOfFrame();

        MoveBy move = title.AddComponent<MoveBy>();
        move.offset = new Vector3(0, -2f, 0);
        move.time = 1f;
        move.Begin();
        yield return new WaitForSeconds(1f);

        async = SceneManager.LoadSceneAsync("MainScene");
        async.allowSceneActivation = false;
        yield return StartCoroutine(Loading());

        text.text = "开始游戏";
        button.enabled = true;
    }

    IEnumerator Loading() {
        float curProgress = 0f;
        while (curProgress <= 1f) {
            float toProgress = async.progress / 0.9f;
            while (curProgress < toProgress) {
                curProgress += 0.01f;
                loadBarScript.SetProgress(curProgress);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void OnStartGame() {
        async.allowSceneActivation = true;
    }
}
