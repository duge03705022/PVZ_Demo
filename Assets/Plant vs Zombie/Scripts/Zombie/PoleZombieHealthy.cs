using UnityEngine;
using System.Collections;

public class PoleZombieHealthy : ZombieHealthy {

    protected override void LostHead() {
        base.LostHead();
        PoleZombieMove move = GetComponent<PoleZombieMove>();
        move.speed = move.walkSpeed;
        GetComponent<ZombieAttack>().enabled = true;
    }
}
