using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float speed;
 
	void Update () {
        transform.Rotate(0, 0, transform.rotation.z + speed * Time.deltaTime);
	}
}
