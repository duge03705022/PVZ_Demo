using UnityEngine;
using System.Collections;

public class PlantHealthy : MonoBehaviour {

    public int hp;
    protected Animator animator;
    protected GameModel model;
    protected PlantGrow grow;
	private SpriteRenderer plant;
	public Sprite crater;

    protected void Awake() {
        animator = GetComponentInChildren<Animator>();
        model = GameModel.GetInstance();
        grow = GetComponent<PlantGrow>();
	}

    public virtual void Damage(int val) {
        hp -= val;
		if (hp < 0) diebefore();
    }

	public virtual void diebefore(){
		model.map[grow.row, grow.col] = null;
		GetComponent<PlantShoot>().enabled = false;
		plant = transform.Find("plant").GetComponent<SpriteRenderer>();
		plant.GetComponent<Animator> ().enabled = false;
		plant.sprite = crater;
	}

    public virtual void Die() {
        model.map[grow.row, grow.col] = null;
        Destroy(gameObject);
    }
}
