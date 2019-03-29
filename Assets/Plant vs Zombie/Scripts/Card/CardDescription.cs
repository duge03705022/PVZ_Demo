using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Card))]
public class CardDescription : MonoBehaviour {

    [Multiline]
    public string description;

    private Card card;

    void Awake() {
        card = GetComponent<Card>();
        enabled = false;
    }

    void OnGUI() {
        GUI.skin.box.fontSize = 16;
        GUI.skin.box.fontStyle = FontStyle.Normal;
        GUI.skin.box.alignment = TextAnchor.MiddleCenter;
        GUI.skin.box.richText = true;
        Vector3 screen = Camera.main.WorldToScreenPoint(transform.position);

        string desc = description;
        float height = 90f;
        if ((card.state & Card.State.NO_SUN) != 0) {
            desc += "\n<color=red>阳光不足</color>";
            height += 20;
        }
            
        if ((card.state & Card.State.CD) != 0) {
            desc += "\n<color=yellow>冷却中...</color>";
            height += 20;
        }

        Rect rect = new Rect(screen.x + 50, Camera.main.pixelHeight - screen.y - 20, 200, height);
        GUI.Box(rect, desc);
    }

    public void OnMouseEnter() {
        enabled = true;
    }

    public void OnMouseExit() {
        enabled = false;
    }


}
