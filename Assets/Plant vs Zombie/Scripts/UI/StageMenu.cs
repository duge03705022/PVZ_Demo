using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageMenu : MonoBehaviour {

    public void RestartStage() {
        //SceneManager.LoadSceneAsync(Application.loadedLevel);
    }

    public void BackToHome() {
        SceneManager.LoadSceneAsync("MainScene");
    }
}
