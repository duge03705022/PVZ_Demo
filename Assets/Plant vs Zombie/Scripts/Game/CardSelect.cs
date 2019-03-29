using UnityEngine;
using System.Collections;

public class CardSelect : MonoBehaviour {

    public GameObject[] cards;
    public int maxCardNumber;

    private float xOffset = 1.1f, yOffset = 0.6f;
    private ArrayList selectedCards = new ArrayList();
    private ArrayList barCardList = new ArrayList();
    //private GameObject gameController;
    private GameObject cardBar;

    void Awake() {
        //gameController = GameObject.Find("GameController");
        cardBar = GameObject.Find("Cards");

        Transform text = transform.Find("Text");
        text.GetComponent<MeshRenderer>().sortingOrder =
            GetComponent<SpriteRenderer>().sortingOrder + 1;
        text.GetComponent<TextMesh>().text += "<color=yellow>" + maxCardNumber + "</color>";
    }

	void Start() {
        Transform container = transform.Find("CardContainer");
        for (int i = 0; i < cards.Length; ++i) {
            float x = (i % 4) * xOffset;
            float y = -(i / 4) * yOffset;
            GameObject card = Instantiate(cards[i]);
            card.transform.parent = container;
            card.transform.localPosition = new Vector3(x, y, 0);
            card.GetComponent<Card>().enabled = false;
            card.tag = "SelectingCard";
        }
	}

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Collider2D collider = Physics2D.OverlapPoint(Utility.GetMouseWorldPos());
            if (collider != null) {
                if (collider.gameObject.tag == "SelectingCard") {
                    GameObject card = collider.gameObject;
                    if (selectedCards.Contains(card)) {
                        selectedCards.Remove(card);
                        card.GetComponent<Card>().SetSprite(true);
                        UpdateCardBar();
                    } else if (selectedCards.Count < maxCardNumber) {
                        selectedCards.Add(card);
                        card.GetComponent<Card>().SetSprite(false);
                        UpdateCardBar();
                    }                   
                }
            }
        }
    }

    void UpdateCardBar() {
        RemoveAllBarCards();

        float xOff = -0.6f;   
        for (int i = 0; i < selectedCards.Count; ++i) {
            GameObject prefab = selectedCards[i] as GameObject;
            GameObject card = Instantiate(prefab);
            card.tag = "Card";
            card.transform.parent = cardBar.transform;
            card.transform.localPosition = new Vector3(0, i * xOff, 0);
            barCardList.Add(card);
        }
    }

    void RemoveAllBarCards() {
        object[] barCards = barCardList.ToArray();
        foreach (GameObject card in barCards) {
            Destroy(card);
        }
        barCardList.Clear();
    }

    public void OnGUI() {
        float xOff = 50f, yOff = 280f;
        float width = 70f, height = 35f;
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (GUI.Button(new Rect(pos.x - xOff - width / 2, pos.y + yOff - height / 2, width, height),
            "提交")) {
                Submit();
        }

        if (GUI.Button(new Rect(pos.x + xOff - width / 2, pos.y + yOff - height / 2, width, height),
            "重置")) {
                Reset();
        }
    }

    void Submit() {
        foreach (GameObject card in barCardList) {
            card.GetComponent<Card>().enabled = true;
        }
//        gameController.GetComponent<GameController>().AfterSelectCard();
    }

    void Reset() {
        foreach (GameObject card in selectedCards) {
            card.GetComponent<Card>().SetSprite(true);
        }
        selectedCards.Clear();
        RemoveAllBarCards();
    }	
}
