using UnityEngine;
using System.Collections;

public class Shovel : MonoBehaviour {

    public Transform shovel;
    private Vector3 oriPos;
    private Quaternion oriRot;
	void Awake () {
        oriPos = shovel.position;
        oriRot = shovel.rotation;
	}
	
	void OnSelect () {
        shovel.Rotate(0, 0, 45f);
	}

    public void CancelSelected() {
        shovel.position = oriPos;
        shovel.rotation = oriRot;
    }
}
