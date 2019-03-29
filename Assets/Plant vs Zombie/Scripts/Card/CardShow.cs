using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Card))]
public class CardShow : MonoBehaviour {

    private Card card;
    private TextMesh cdText;
	void Awake () {
        card = GetComponent<Card>();
	}

    void Start() {
        int order = GetComponent<SpriteRenderer>().sortingOrder + 1;
        Transform priceText = transform.Find("Price");
        priceText.GetComponent<MeshRenderer>().sortingOrder = order;
        priceText.GetComponent<TextMesh>().text = card.price.ToString();

        Transform cd = transform.Find("CD");
        cd.GetComponent<MeshRenderer>().sortingOrder = order;
        cdText = cd.GetComponent<TextMesh>();
    }

    void Update() {
        if ((card.state & Card.State.CD) != 0) {        
            cdText.text = card.CdTime.ToString("F1") + "s";
        } else {
            cdText.text = "";
        }
    }
}
