using UnityEngine;
using System.Collections;

public class MoveBy : MonoBehaviour {

    public Vector3 offset;
    public float time;

    private Vector3 speed;
    private float curTime = 0;

	void Awake () {
        enabled = false;
	}
	
	void Update () {
        if (curTime < time) {
            curTime += Time.deltaTime;
            transform.Translate(speed * Time.deltaTime);
        } else {
            Destroy(this);
        }
	}

    public void Begin() {
        enabled = true;
        speed = offset / time;
    }
}
