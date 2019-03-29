using UnityEngine;
using System.Collections;

public class FollowWithCamera : MonoBehaviour {

	void Update () {
        Vector3 pos = Camera.main.transform.position;
        pos.z = 0;
        transform.position = pos;

	}
}
