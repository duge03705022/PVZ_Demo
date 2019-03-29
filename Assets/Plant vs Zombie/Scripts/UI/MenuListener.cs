using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour {

    public AudioClip clickSound;

    public void OnStageButton(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }

	public void OnQuitBtn() {
		Debug.Log ("onQuitBtn");
		Application.Quit ();
	}

    public void PlayClickSound() {
        AudioManager.GetInstance().PlaySound(clickSound);
    }
}
