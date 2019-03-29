using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour{

    public bool musicOn = true;
    public bool soundOn = true;
    private float _musicVolume = 1.0f;
    private float _soundVolume = 1.0f;

    private GameObject obj;
    private AudioSource mainMusic;
    private ArrayList sounds = new ArrayList();

    public float musicVolume {
        get { return _musicVolume; }
        set {
            if (value != _musicVolume) {
                _musicVolume = value;
                mainMusic.volume = value;
            }
        }
    }

    public float soundVolume {
        get { return _soundVolume; }
        set {
            if (value != _soundVolume) {
                _soundVolume = value;
                foreach (AudioSource src in sounds) {
                    src.volume = value;
                }
            }
        }
    }

    public void PlayMusic(AudioClip music) {
        PlayMusic(music, true);
    }

    public void PlayMusic(AudioClip music, bool loop) {
        mainMusic.Stop();
        mainMusic.clip = music;
        mainMusic.volume = musicVolume;
        mainMusic.loop = loop;
        if (musicOn && Time.timeScale != 0) {
            mainMusic.Play();
        }
    }

    public void StopMusic() {
        mainMusic.Stop();
    }

    public void PauseMusic() {
        mainMusic.Pause();
    }

    public void ResumeMusic() {
        if (musicOn && Time.timeScale != 0) {
            mainMusic.Play();
        }
    }

    public AudioSource PlaySound(AudioClip sound) {
        return PlaySound(sound, false);
    }

    public AudioSource PlaySound(AudioClip sound, bool loop) {
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = sound;
        source.volume = soundVolume;
        source.loop = loop;
        sounds.Add(source);
        if (soundOn && Time.timeScale != 0) {
            source.Play();
        }
        if (!loop) {
            StartCoroutine(DoDestroy(source, sound.length));
        }
        return source;
    }

    public void StopSound(AudioSource sound) {
        StartCoroutine(DoDestroy(sound, 0));
    }

    public void PauseSound(AudioSource sound) {
        sound.Pause();
    }

    public void ResumeSound(AudioSource sound) {
        if (soundOn && Time.timeScale != 0) {
            sound.Play();
        }
    }

    public void PauseAllSounds() {
        foreach (AudioSource src in sounds) {
            src.Pause();
        }
    }

    public void ResumeAllSounds() {
        if (soundOn && Time.timeScale != 0) {
            foreach (AudioSource src in sounds) {
                src.Play();
            }
        }
    }

    IEnumerator DoDestroy(AudioSource sound, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(sound);
        sounds.Remove(sound);
    }

    public void Clear() {
        foreach (AudioSource src in sounds) {
            Destroy(src);
        }
        sounds.Clear();
    }

    static AudioManager instance;
    static public AudioManager GetInstance() {
        if (instance == null) {
            GameObject obj = new GameObject("AudioManager");
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<AudioManager>();
            instance.obj = obj;
            obj.AddComponent<FollowWithCamera>();
            instance.mainMusic = obj.AddComponent<AudioSource>();
        }
        return instance;
    }
}
