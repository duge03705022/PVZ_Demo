using UnityEngine;
using System.Collections;

public class Utility {

    static public Vector3 GetMouseWorldPos() {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}
