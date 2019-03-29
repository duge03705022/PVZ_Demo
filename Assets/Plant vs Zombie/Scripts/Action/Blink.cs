using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Blink : MonoBehaviour {

    private new SpriteRenderer renderer;
    private float time;
    private float curTime;

	void Awake () {
        enabled = false;
        renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        curTime += Time.deltaTime;
        Color color = renderer.color;
        if (curTime < time) {
            color.a = 1 - curTime / time;
        } else {
            color.a = curTime / time - 1;
            if (curTime > time * 2)
                enabled = false;             
        }
        renderer.color = color;
	}

    public void Begin(float t) {
        enabled = true;
        time = t / 2;
        curTime = 0f;
    }
}
