using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {

    public AudioClip pauseSound;
    public UnityEngine.UI.Text text;

    private AudioManager am;

    void Awake() {
        am = AudioManager.GetInstance();
    }
	
	public void OnClick () {
        if ("暂停游戏" == text.text) {
            pauseGame();
        } else {
            resumeGame();
        }        
	}

    public void pauseGame() {
        text.text = "继续游戏";
        am.PauseAllSounds();
        am.PlaySound(pauseSound);
        Time.timeScale = 0;
        am.PauseMusic();
    }

    public void resumeGame() {
        text.text = "暂停游戏";
        Time.timeScale = 1;
        am.ResumeAllSounds();
        am.PlaySound(pauseSound);
        am.ResumeMusic();
    }
}
