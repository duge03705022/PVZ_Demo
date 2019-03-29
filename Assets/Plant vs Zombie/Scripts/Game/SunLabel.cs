using UnityEngine;
using System.Collections;

public class SunLabel : MonoBehaviour {

    private GameObject text;
    private GameModel model;
    void Awake() {
        text = transform.Find("Text").gameObject;
        text.GetComponent<MeshRenderer>().sortingOrder
            = GetComponent<SpriteRenderer>().sortingOrder + 1;
        model = GameModel.GetInstance();
    }

    void Update() {
        text.GetComponent<TextMesh>().text = model.sun.ToString();
    }
}
