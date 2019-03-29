using UnityEngine;
using System.Collections;

public class PumpkinSpriteDisplay : MonoBehaviour, SpriteDisplay {

    public int frontOrderOffset = 1;
    public int backOrderOffset = -1;

    private SpriteRenderer front;
    private SpriteRenderer back;

	void Awake () {
        front = transform.Find("front").GetComponent<SpriteRenderer>();
        back = transform.Find("back").GetComponent<SpriteRenderer>();
	}
	
	public void SetAlpha(float a) {
        Color color = Color.white;
        color.a = a;
        front.color = color;
        back.color = color;
    }

    public void SetOrder(int order) {
        front.sortingOrder = order;
        back.sortingOrder = order - 1;
    }

    public void SetOrderByRow(int row) {
        front.sortingOrder = 1000 * (row + 1) + frontOrderOffset;
        back.sortingOrder = 1000 * (row + 1) + backOrderOffset;
    }
}
