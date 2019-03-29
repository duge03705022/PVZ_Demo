using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

    public float time;
    private new SpriteRenderer renderer;
    private float curTime = 0;

    void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        enabled = false;
    }

    public void Begin() {
        enabled = true;
    }

    void Update() {
        curTime += Time.deltaTime;
        if (curTime <= time) {
            renderer.color = new Color(1, 1, 1, curTime / time);
        } else {
            Destroy(this);
        }
    }
}
