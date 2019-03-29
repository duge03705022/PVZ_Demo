using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    public UnityEngine.UI.Toggle musicToggle;
    public UnityEngine.UI.Toggle soundToggle;
    public UnityEngine.UI.Slider musicSlider;
    public UnityEngine.UI.Slider soundSlider;

    private AudioManager am;

    void Awake() {
        am = AudioManager.GetInstance();
    }

    void Start() {
        am.Clear();
        musicToggle.isOn = am.musicOn;
        soundToggle.isOn = am.soundOn;
        musicSlider.value = am.musicVolume;
        soundSlider.value = am.soundVolume;
    }

    public void OnMusicChanged() {
        am.musicOn = musicToggle.isOn;
        if (musicToggle.isOn) {
            am.ResumeMusic();
        } else {
            am.PauseMusic();
        }  
    }

    public void OnSoundChanged() {
        am.soundOn = soundToggle.isOn;
        if (soundToggle.isOn) {
            am.ResumeAllSounds();
        } else {
            am.PauseAllSounds();
        }
    }

    public void OnMusicVolume() {
        am.musicVolume = musicSlider.value;
    }

    public void OnSoundVolume() {
        am.soundVolume = soundSlider.value;
    }
}
