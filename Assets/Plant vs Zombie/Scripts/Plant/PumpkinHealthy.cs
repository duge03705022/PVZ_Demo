using UnityEngine;
using System.Collections;

public class PumpkinHealthy : PlantHealthy {

    private Animator frontAnimator, backAnimator;
    private PumpkinGrow pumpkinGrow;

	new void Awake () {
        model = GameModel.GetInstance();
        frontAnimator = transform.Find("front").GetComponent<Animator>();
        backAnimator = transform.Find("back").GetComponent<Animator>();
        pumpkinGrow = GetComponent<PumpkinGrow>();
	}

    public override void Damage(int val) {
        base.Damage(val);
        frontAnimator.SetInteger("hp", hp);
        backAnimator.SetInteger("hp", hp);
    }

    public override void Die() {
        model.map[pumpkinGrow.row, pumpkinGrow.col] = pumpkinGrow.innerPlant;
        Destroy(gameObject);
    }
}
