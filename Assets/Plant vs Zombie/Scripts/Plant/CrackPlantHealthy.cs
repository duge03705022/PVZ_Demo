using UnityEngine;
using System.Collections;

public class CrackPlantHealthy : PlantHealthy {

    public override void Damage(int val) {
        base.Damage(val);
        animator.SetInteger("hp", hp);
    }

    void AfterGrow() { }
}
