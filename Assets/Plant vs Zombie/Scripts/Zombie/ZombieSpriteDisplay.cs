using UnityEngine;
using System.Collections;

public class ZombieSpriteDisplay : MonoBehaviour, SpriteDisplay {

    public int orderOffset = 2;

    private SpriteRenderer shadow;
    private SpriteRenderer zombie;

	void Awake () {
        shadow = transform.Find("shadow").GetComponent<SpriteRenderer>();
        zombie = transform.Find("zombie").GetComponent<SpriteRenderer>();
	}
	
	public void SetAlpha(float a) {
        Color color = zombie.color;
        color.a = a;
        shadow.color = color;
        zombie.color = color;
    }

    public void SetColor(float r, float g, float b) {
        Color color = new Color(r, g, b, zombie.color.a);
        zombie.color = color;
    }

    public void SetOrder(int order) {
        zombie.sortingOrder = order;
    }

    public void SetOrderByRow(int row) {
        zombie.sortingOrder = 1000 * (row + 1) + orderOffset;
    }
}
