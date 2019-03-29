using UnityEngine;
using System.Collections;

public class ScaleUp : MonoBehaviour {

	public Vector3 offset;
	public float time = 0.5f;

	private Vector3 scaleSpeed;
	private float curTime = 0;

	void Awake () {
		enabled = false;
	}

	void Update () {
		if (curTime < time) {
			curTime += Time.deltaTime;
			transform.localScale = transform.localScale + scaleSpeed * Time.deltaTime;
		} else {
			Destroy(this);
		}
	}

	public void Begin() {
		enabled = true;
		scaleSpeed = new Vector3(0.9f, 0.9f, 0.9f) / time;
	}
}
